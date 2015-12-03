using UnityEngine;

namespace AdventureFVTC {
    /**
     * A derivative camera for AdventureFVTC. Specifies
     * a height, drawback, and drawside that tells the camera      
     * where it will be, relative to a subject, when it is locked.
     * Allows free setting of offsets and relative position.
     * Defines how the camera moves to different locations while
     * still looking at a subject.
     *
     * @author  Ryan
     * @date    03 Dec 2015
     * @see     Camera
     */
    public class FreeCamera:CameraBase {  
        private float transitionTime;
        private Vector3 transitionToPosition;
        private Vector3 transitionFromPosition;
        private Vector3 transitionToRotation;
        private Vector3 transitionFromRotation;
        private bool isTransitioning = false;
        private bool isTransitioningRelativeToSubject = true;
        private bool isRotating = false;

        private bool hasChangedSubject = false;
        protected bool isSubjectChangeStillTransitioning = false;
        private bool isReturning = false;
        private float yRotationStepped = 0.0f;
        private float zRotationStepped = 0.0f;
        private float xRotationStepped = 0.0f;
        private float yMovementStepped = 0.0f;
        private float zMovementStepped = 0.0f;
        private float xMovementStepped = 0.0f;
        private Vector3 maxRotation;
        private Vector3 maxMovement;

        [SerializeField] protected float height = 0.0f;
        [SerializeField] protected float drawback = 0.0f;
        [SerializeField] protected float drawside = 0.0f;

        #region Accessors/Mutators
        /**
         * The maximum the camera's rotation can be offset by.
         * If the received value is equal to the current max rotation or the current offset rotation,
         * proceed to setting a restricted max rotation.
         * If the subject has just been changed, set the max rotation as the current offset. Else,
         * continue on to the next step.
         * If the subject has a facing direction and the subject hasn't just been changed, set the max rotation 
         * to the halfway point between the subject and its facing direction.
         * If the subject doesn't have a facing direction and the subject hasn't just been changed, set the max 
         * to the camera's current default rotation.
         * 
         * @param   value   The value to check what the maxRotation needs to be set to, and in rare cases, the 
         *                  value that maxRotation will be set to.
         * @return          The maxRotation.
         */
        private Vector3 MaxRotation {
            get {
                return maxRotation;
            }
            set {
                // If the subject has just been changed.
                if (hasChangedSubject)
                    // Set the amount the camera can rotate to the difference of the two rotations.
                    maxRotation = value;        
                // Else the subject hasn't just been changed.
                else
                {
                    // If the subject has a facing direction.
                    if (subjectFacingDirection != null) {
                        // Calculate the new max rotation to be halfway between the subject and its facing direction.
                        float rotationY = (subjectFacingDirection.transform.position.y - subject.transform.position.y) / 2;
                        float rotationZ = (subjectFacingDirection.transform.position.z - subject.transform.position.z) / 2;
                        float rotationX = (subjectFacingDirection.transform.position.x - subject.transform.position.x) / 2;
                        maxRotation = new Vector3(rotationX, rotationY, rotationZ);
                    }
                    // Else the subject doesn't have a facing direction.
                    else
                        // Set the max rotation to the default rotation, 
                        maxRotation = defaultOffsetPosition;            
                }
            }
        }

        /**
         * The maximum the camera's position can change by.
         * 
         * @param   value   The value to set as the maxMovement.
         * @return          The maxMovement.
         */
        private Vector3 MaxMovement {
            get {    
                return maxMovement;
            }
            set {
                maxMovement = value;
            }
        }

        /**
         * The height is how high above the subject the
         * camera is when the camera is locked.
         * If the camera is transitioning relative to a subject,
         * set the camera's relative y axis to height.
         * Else set the camera's y axis position to height.
         * 
         * @param   value   The value to set as the height.
         * @return          The height.
         */
        public float Height {
            get {
                return height;
            }
            set {
                height = value;
                if (enabled) {
                    if (isTransitioningRelativeToSubject)
                        relativePosition.y = height;
                    else
                        transform.position.Set(transform.position.x, height, transform.position.z);
                }                 
            }
        }

        /**
         * The drawback is how far behind the camera
         * positions itself when the camera is locked.
         * If the camera is transitioning relative to a subject,
         * set the camera's relative z axis to drawback.
         * Else set the camera's z axis position to drawback.
         * 
         * @param   value   The value to use as the drawback.
         * @return          The drawback.
         */
        public float Drawback {
            get {
                return drawback;
            }
            set {
                drawback = value;
                if (enabled) {
                    if (isTransitioningRelativeToSubject)
                        relativePosition.z = drawback;
                    else
                        transform.position.Set(transform.position.x, transform.position.y, drawback);
                }                                 
            }
        }

        /**
         * The drawside is how far right the camera
         * positions itself when the camera is locked.
         * Positive values move the camera to the right,
         * negative values move the camrera to the left.
         * If the camera is transitioning relative to a subject,
         * set the camera's relative x axis to drawside.
         * Else set the camera's x axis position to drawside.
         * 
         * @param   value   The value to use as the drawside.
         * @return          The drawside.
         */
        public float Drawside {
            get {
                return drawside;
            }
            set {
                drawside = value;
                if (enabled) {
                    if (isTransitioningRelativeToSubject)
                        relativePosition.x = drawside;
                    else
                        transform.position.Set(drawside, transform.position.y, transform.position.z);
                }                   
            }
        }

        /**
         * Public accessor/mutator to offsetPosition in Camera.
         *
         * @param value  The value to set as the offset.
         * @return       The offset position.
         */
        public Vector3 OffsetPosition {
            get {
                return offsetPosition;
            }
            set {
                offsetPosition = value;
            }
        }

        /**
         * Public accessor/mutator to offsetRotation in Camera.
         *
         * @param value  The value to set as the offset.
         * @return       The offset rotation.
         */
        public Vector3 OffsetRotation {
            get {
                return offsetRotation;
            }
            set {
                offsetRotation = value;
            }
        }
        #endregion

        #region Overrides
        /**
         * This override of LockPosition removes the updating
         * of relativePosition.
         * 
         * @param   value   Whether the position is locked.
         * @return          True if the position is locked, false otherwise.
         */
        public override bool LockPosition {
            get {
                return base.LockPosition;
            }
            set {
                lockPosition = value;
            }
        }

        /**
        * Stores the returnTo boolean to allow inverting the rotation.
        * Performs transition setup.
        * Stores the location the camera needs to travel to, how much time it 
        * should take, and then signals that the transition is ready.
        *
        * @param   position    The position the camera needs to move to.
        * @param   time        The amount of time it should take for the 
        *                      camera moving from one point to another. 
        * @param   returnTo    The flag if the camera should return to
        *                      its default position.
        * @see     TransitionSetup()       
        */
        public override void TransitionWithSubject(Vector3 position, float time, bool returnTo) {
            base.TransitionWithSubject(position, time, returnTo);

            // Has the change subject transition finished?
            if (!isSubjectChangeStillTransitioning) {
                // If the subject was just changed.
                if (hasChangedSubject)
                    // Mark the transition as in progress.
                    isSubjectChangeStillTransitioning = true;

                isTransitioning = false;
                isReturning = returnTo;
                transitionToPosition = position;
                isTransitioningRelativeToSubject = true;
                TransitionSetUp();
                transitionFromPosition = relativePosition;
                transitionTime = time;

                
                isTransitioning = true;
            }
        }

        /** 
         * ToDo: Disinct this method from TransitionWithPlayer
         * by removing the camera's subject and add functionality
         * of the camera spinning around to get good view of the map.
         * 
         * Stores the returnTo boolean to allow inverting the rotation.
         * Performs transition setup.
         * Unlocks the camera so it no longer moves relative to the player.
         * Stores the location the camera needs to travel to,  how much time it 
         * should take, and then signals that the transition is ready.
         *
         * @param   position    The position the camera needs to move to.
         * @param   time        The amount of time it should take for the 
         *                      camera moving from one point to another. 
         * @param   returnTo    The flag if the camera should return to
         *                      its default position.
         * @see     TransitionSetup()      
         */
        public override void TransitionToPoint(Vector3 position, float time, bool returnTo) {
            base.TransitionToPoint(position, time, returnTo);

            // Has the change subject transition finished?
            if (!isSubjectChangeStillTransitioning) {
                // If the subject was just changed.
                if (hasChangedSubject)
                    // Mark the transition as in progress.
                    isSubjectChangeStillTransitioning = true;

                isTransitioning = false;
                isReturning = returnTo;
                transitionToPosition = position;
                transitionFromPosition = transform.position;
                transitionTime = time;

                isTransitioningRelativeToSubject = false;
                TransitionSetUp();        
                isTransitioning = true;
            }
        }

        /**
         * Signals that the subject has just been changed.
         * Changes the relativePosition of the camera to its position
         * relative of the new subject or its backside.
         * Changes the the camera's current subject and behind direction
         * to the new subject and behind direction.
         * Changes the OffsetRotation of the camera equal to the difference
         * of its old rotation from its new rotation.
         * Changes the camera's facing direction to the new facing direction.
         * Sets the new max amounts the camera can rotate and move.
         *
         * @param   newSubject              The new subject the camera will look to.
         * @param   newFacingDirection      The new subject facing direction the camera will use to calculate max rotations.
         * @param   newBehindDirection      The new subject behind direction the camera will move relative to.
         * @see     MaxRotation
         * @see     MaxMovement
         */
        public override void ChangeSubject(GameObject newsubject, GameObject newFacingDirection, GameObject newBehindDirection) {
            base.ChangeSubject(newsubject, newFacingDirection, newBehindDirection);

            // Signal that the subject has just been changed.
            hasChangedSubject = true;
            // Signal that a new subject needs to be transitioned to.
            isSubjectChangeStillTransitioning = false;
            // Holds the position difference between the old and new subject.
            Vector3 positionDifference;

            // Get the current subject's position before subjectChange.
            Vector3 oldSubjectPosition = subject.transform.position;
            // Get the new subject's position after subjectChange.
            Vector3 newSubjectPosition = newsubject.transform.position;
            // Get the difference between the old and new position, the new offset.
            Vector3 rotationDifference = oldSubjectPosition - newSubjectPosition;

            if (newBehindDirection != null)
                // Get the position difference of the newsubject's behind position and the subject's behind position.
                positionDifference = transform.position - newBehindDirection.transform.position;
            else
                // Get the position difference of the newsubject's position and the subject's position.
                positionDifference = transform.position - newsubject.transform.position;

            // Set the relativePosition to the difference so the camera keep its position relative to its new subject.
            relativePosition = positionDifference;

            // Set the current behind direction to the new behind direction.
            subjectBehindDirection = newBehindDirection;
            // Set the current subject to the new subject.
            subject = newsubject;
            // Get the subject's facing direction.
            subjectFacingDirection = newFacingDirection;

            // Update the camera's new rotation.
            OffsetPosition = rotationDifference;

            if (rotationDifference.x < 0)
                rotationDifference.x *= -1;
            if (rotationDifference.y < 0)
                rotationDifference.y *= -1;
            if (rotationDifference.z < 0)
                rotationDifference.z *= -1;

            if (positionDifference.x < 0)
                positionDifference.x *= -1;
            if (positionDifference.y < 0)
                positionDifference.y *= -1;
            if (positionDifference.z < 0)
                positionDifference.z *= -1;

            // Set the max the camera can rotate to the current rotationDifference.
            MaxRotation = rotationDifference;
            // Set the max the camera can move to the current positionDifference.
            MaxMovement = positionDifference;
        }

        /**
         * This override of Start adds height, drawback, 
         * drawside  setCameraDefault, MaxRotation, and
         * MaxMovement to the start method.
         */
        protected override void Start() {
            base.Start();

            Height = height;
            Drawback = drawback;
            Drawside = drawside;
            setCameraDefault();
            MaxRotation = maxRotation;
            MaxMovement = maxMovement;
        }

        /**
         * This override of Update adds the functionality
         * of the camera transitioning to the update method.
         *
         * This override also adds the functionality of the camera's
         * relative position being updated while the camera is locked 
         * so when the camera unlocks it can perform a smooth transition 
         * back to its subject instead of snapping there.
         */
        protected override void Update() {
            base.Update();

            // Is the camera transitioning?
            if (isTransitioning) {
                /** 
                 * If the camera is rotating, transition the camera's rotation to 
                 * either the camera's mid way point or its default.
                 * If the camera's subject has changed, transition the camera's rotation
                 * to the camera's new max rotation.
                 */
                if (isRotating) {
                    // Used to flag once the rotations have been reached.
                    bool yRotationReached = false;
                    bool zRotationReached = false;
                    bool xRotationReached = false;
                    float yRotationRate;
                    float zRotationRate;
                    float xRotationRate;
                    
                    /// Get the y, z, and x rotation rates per frame.
                    // Rotate the camera in three-quarters the time it takes to transition the camera's position (0.50 amount of time).
                    // This way the camera will be looking at the player before the camera gets to the player.
                    // If amount the y rotation needs to rotate is zero.
                    if (MaxRotation.y == 0) {
                        yRotationRate = 0; // Set the rate of change to zero.
                        yRotationReached = true; // Signal that the position has been reached.
                    }
                    else
                        //rotationRateY = Mathf.Min((transitionToRotation.y - transitionFromRotation.y) / (transitionTime * 0.3f) * Time.deltaTime, MaxRotation.y);
                        yRotationRate = (transitionToRotation.y - transitionFromRotation.y) / (transitionTime * 0.3f) * Time.deltaTime;
                    // If amount the z rotation needs to rotate is zero.
                    if (MaxRotation.z == 0)
                    {
                        zRotationRate = 0; // Set the rate of change to zero.
                        zRotationReached = true; // Signal that the position has been reached.
                    }
                    else
                        //rotationRateZ = Mathf.Min((transitionToRotation.z - transitionFromRotation.z) / (transitionTime * 0.3f) * Time.deltaTime, MaxRotation.z);
                        zRotationRate = (transitionToRotation.z - transitionFromRotation.z) / (transitionTime * 0.3f) * Time.deltaTime;
                    // If amount the x rotation needs to rotate is zero.
                    if (MaxRotation.x == 0)
                    {
                        xRotationRate = 0; // Set the rate of change to zero.
                        xRotationReached = true; // Signal that the position has been reached.
                    }
                    else
                        //rotationRateX = Mathf.Min((transitionToRotation.x - transitionFromRotation.x) / (transitionTime * 0.3f) * Time.deltaTime, MaxRotation.x);
                        xRotationRate = (transitionToRotation.x - transitionFromRotation.x) / (transitionTime * 0.3f) * Time.deltaTime;

                    // If yRotationRate is negative.
                    if (yRotationRate < 0)
                        yRotationStepped += -1 * yRotationRate;
                    else
                        yRotationStepped += yRotationRate;
                    // If zRotationRate is negative.
                    if (zRotationRate < 0)
                        zRotationStepped += -1 * zRotationRate;
                    else
                        zRotationStepped += zRotationRate;
                    // If xRotationRate is negative.
                    if (xRotationRate < 0)
                        xRotationStepped += -1 * xRotationRate;
                    else
                        xRotationStepped += xRotationRate;

                    // The camera has reached its y destination (or passed it) and hasn't already signaled it has reached its location.
                    if (yRotationStepped >= MaxRotation.y)
                    {
                        //yRotationRate = 0; // shrink the movement rate a bit.
                        yRotationRate -= (yRotationStepped - MaxRotation.y);  // shrink the movement rate a bit.
                        yRotationReached = true; // Signal that the y axis position has been reached.
                    }
                    // The camera has reached its z destination (or passed it) and hasn't already signaled it has reached its location.
                    if (zRotationStepped >= MaxRotation.z)
                    {
                        //zRotationRate = 0; // shrink the movement rate a bit.
                        zRotationRate -= (zRotationStepped - MaxRotation.z);  // shrink the movement rate a bit.
                        zRotationReached = true; // Signal that the z axis position has been reached.
                    }
                    // The camera has reached its x destination (or passed it) and hasn't already signaled it has reached its location.
                    if (xRotationStepped >= MaxRotation.x)
                    {
                        //xRotationRate = 0; // shrink the movement rate a bit.
                        xRotationRate -= (xRotationStepped - MaxRotation.x); // shrink the movement rate a bit.
                        xRotationReached = true; // Signal that the x axis position has been reached.
                    }

                    // Once all rotations have been reached. 
                    if (yRotationReached && zRotationReached && xRotationReached) {       
                        isRotating = false; // Stop the transition.

                        // If the subject has just been changed.
                        if (hasChangedSubject) {
                            hasChangedSubject = false; // Signal the subject change rotation has finished.                              
                            MaxRotation = maxRotation; // Reset the camera's max rotation to relative to its new subject.
                        }       
                    }

                    // Try to rotate the camera to the next x, y, and z offset rotation step (frame).
                    float offSetY = offsetPosition.y + yRotationRate;
                    offsetPosition.y = offSetY;
                    float offSetZ = offsetPosition.z + zRotationRate;
                    offsetPosition.z = offSetZ;
                    float offSetX = offsetPosition.x + xRotationRate;
                    offsetPosition.x = offSetX;
                }

                // Used as flags once the destination has been reached.
                bool yReached = false;
                bool zReached = false;
                bool xReached = false;
                float yChangeRate;
                float zChangeRate;
                float xChangeRate;

                // Get the y, z, and x position increase rates per frame.
                // If amount the y position needs to move is zero.
                if (MaxMovement.y == 0) {                  
                    yChangeRate = 0; // Set the rate of change to zero.
                    yReached = true; // Signal that the position has been reached.
                }   
                else
                    yChangeRate = Mathf.Min((transitionToPosition.y - transitionFromPosition.y) / transitionTime * Time.deltaTime, MaxMovement.y);
                // If amount the z position needs to move is zero.
                if (MaxMovement.z == 0) {                
                    zChangeRate = 0; // Set the rate of change to zero.
                    zReached = true; // Signal that the position has been reached.
                }
                else
                    zChangeRate = Mathf.Min((transitionToPosition.z - transitionFromPosition.z) / transitionTime * Time.deltaTime, MaxMovement.z);
                // If amount the x position needs to move is zero.
                if (MaxMovement.x == 0)
                {
                    xChangeRate = 0; // Set the rate of change to zero.
                    xReached = true; // Signal that the position has been reached.
                }
                else
                    xChangeRate = Mathf.Min((transitionToPosition.x - transitionFromPosition.x) / transitionTime * Time.deltaTime, MaxMovement.x);

                // If yChangeRate is negative.
                if (yChangeRate < 0)
                    yMovementStepped += -1 * yChangeRate;
                else
                    yMovementStepped += yChangeRate;
                // If zChangeRate is negative.
                if (zChangeRate < 0)
                    zMovementStepped += -1 * zChangeRate;
                else
                    zMovementStepped += zChangeRate;
                // If xChangeRate is negative.
                if (xChangeRate < 0)
                    xMovementStepped += -1 * xChangeRate;
                else
                    xMovementStepped += xChangeRate;

                // The camera has reached its y destination (or passed it) and hasn't already signaled it has reached its location.
                if (yMovementStepped >= MaxMovement.y) {
                    yChangeRate -= (yMovementStepped - MaxMovement.y);  // shrink the movement rate a bit.
                    yReached = true; // Signal that the y axis position has been reached.
                }
                // The camera has reached its y destination (or passed it) and hasn't already signaled it has reached its location.
                if (zMovementStepped >= MaxMovement.z) {
                    zChangeRate -= (zMovementStepped - MaxMovement.z);  // shrink the movement rate a bit.
                    zReached = true; // Signal that the z axis position has been reached.
                }
                // The camera has reached its y destination (or passed it) and hasn't already signaled it has reached its location.
                if (xMovementStepped >= MaxMovement.x) {
                    xChangeRate -= (xMovementStepped - MaxMovement.x); // shrink the movement rate a bit.
                    xReached = true; // Signal that the x axis position has been reached.
                }

                // Once all destinations have been reached.
                if (yReached && zReached && xReached) {
                    isTransitioning = false; // Stop the transition.
                    
                    // If the subject change was still transitioning.
                    if (isSubjectChangeStillTransitioning) {                                       
                        isSubjectChangeStillTransitioning = false; // Signal that the subject change transition has finished.                      
                    }
                }

                if (isTransitioningRelativeToSubject)
                {
                    // Try to move the camera to the next x, y, and z position step (frame).
                    float relativeY = relativePosition.y + yChangeRate;
                    relativePosition.y = relativeY;
                    float relativeZ = relativePosition.z + zChangeRate;
                    relativePosition.z = relativeZ;
                    float relativeX = relativePosition.x + xChangeRate;
                    relativePosition.x = relativeX;
                }
                else
                {
                    // Try to move the camera to the next x, y, and z position step (frame).
                    float positionY = transform.position.y + yChangeRate;
                    float positionZ = transform.position.z + zChangeRate;
                    float positionX = transform.position.x + xChangeRate;
                    transform.position = new Vector3(positionX, positionY, positionZ);
                }              
            }    
        }       
        #endregion

        /**
         * Saves drawside, height, and drawback as this camera's default position.
         * Saves this camera's initial offset rotation as its defaultCameraRotation.
         */
        private void setCameraDefault() {
            DefaultRelativePosition.Set(relativePosition.x, relativePosition.y, relativePosition.z);
            defaultOffsetPosition = OffsetPosition;
        }

        /**
        * If the camera is moving relative to a subject, get the amount the camera is allowed to move
        * relative to a subject.
        * Else the camera is moving to a point, get the amount the camera is allowed to move to get to 
        * a certain point.
        * If the camera didn't achieve its full rotation last rotate, invert the amount rotated in the 
        * last rotation and flag that the camera is ready to rotate.
        * Else check the direction the camera wants to rotate to and the camera's current rotation.
        * If the camera wants to rotate towards a rotation its not currently at, flag that the camera
        * is ready to rotate and reset the amounts rotated.
        */
        private void TransitionSetUp() {
            // If the camera is moving relative to the subject and to a new relative position.
            if (isTransitioningRelativeToSubject) {
                // Move the camera with the subject.
                if (!LockPosition) {
                    if (subjectBehindDirection != null)
                        relativePosition = new Vector3(transform.position.x - subjectBehindDirection.transform.position.x, 
                            transform.position.y - subjectBehindDirection.transform.position.y, 
                            transform.position.z - subjectBehindDirection.transform.position.z); // Update the camera's new relative position.
                    else
                        relativePosition = new Vector3(transform.position.x - subject.transform.position.x,
                            transform.position.y - subject.transform.position.y,
                            transform.position.z - subject.transform.position.z); // Update the camera's new relative position.
                    LockPosition = true;
                }
                
                // Get the difference between where the camera relatively needs to go to and where the camera relatively is.
                float positiony = transitionToPosition.y - relativePosition.y;
                float positionz = transitionToPosition.z - relativePosition.z;
                float positionx = transitionToPosition.x - relativePosition.x;
                
                // Get the positive difference for each axis. This is how far
                // the camera is allowed to move on each axis.
                if (positiony < 0)
                    positiony = -1 * positiony;
                if (positionz < 0)
                    positionz = -1 * positionz;
                if (positionx < 0)
                    positionx = -1 * positionx;

                // Reset the counters for how far each axis has moved.
                yMovementStepped = 0;
                zMovementStepped = 0;
                xMovementStepped = 0;
                // Update the new max the camera is allowed to move.
                MaxMovement = new Vector3(positionx, positiony, positionz);
            }
            // Else the camera is not moving relative to the subject and is moving to a new position.
            else {
                // Set the camera to not move with the player.
                lockPosition = false;

                // Get the difference between the where the camera needs to go to and where the camera is.
                float positiony = transitionToPosition.y - transform.position.y;
                float positionz = transitionToPosition.z - transform.position.z;
                float positionx = transitionToPosition.x - transform.position.x;

                // Get the positive difference for each axis. This is how far
                // the camera is allowed to move on each axis.
                if (positiony < 0)
                    positiony = -1 * positiony;
                if (positionz < 0)
                    positionz = -1 * positionz;
                if (positionx < 0)
                    positionx = -1 * positionx;

                // Reset the counters for how far each axis has moved.
                yMovementStepped = 0;
                zMovementStepped = 0;
                xMovementStepped = 0;
                // Update the new max the camera is allowed to move.
                MaxMovement = new Vector3(positionx, positiony, positionz);
            }

            // If the subject has just changed, perform check if rotations are necessary.
            if (hasChangedSubject) {
                transitionFromRotation = OffsetPosition + defaultOffsetPosition; // Get the from rotation relative to the difference between the old subject and
                                                                                 // new subject position, plus the defaultOffset (where we will be rotating to).
                // Get the positive values of the OffsetPosition.
                Vector3 offSet = OffsetPosition;
                if (offSet.x < 0)
                    offSet.x *= -1;
                if (offSet.y < 0)
                    offSet.y *= -1;
                if (offSet.z < 0)
                    offSet.z *= -1;

                // If our OffsetPosition is between our default and our midway point.
                if (offSet != defaultOffsetPosition && offSet != MaxRotation)
                {
                    // Swap the direction.
                    Vector3 swap = transitionFromRotation;
                    transitionFromRotation = transitionToRotation;
                    transitionToRotation = swap;
                    // We're swapping the direction, so invert the rotationStepped.
                    yRotationStepped = MaxRotation.y - yRotationStepped;
                    zRotationStepped = MaxRotation.z - zRotationStepped;
                    xRotationStepped = MaxRotation.x - xRotationStepped;

                    // We're ready to rotate.
                    isRotating = true;
                }
                // Else the camera has finished its last rotation and is at its default rotation or max rotation.
                else
                {
                    // If the camera is trying to rotate towards its defaultOffsetPosition but the camera is already there.
                    if (isReturning && offSet == defaultOffsetPosition)
                    {
                        // We shouldn't rotate.
                        isRotating = false;
                    }
                    // If the camera is trying to rotate towards its max rotation but the camera is already there.
                    else if (!isReturning && offSet == MaxRotation)
                    {
                        // We shouldn't rotate. 
                        isRotating = false;
                    }
                    // Else we're trying to rotate towards a rotation that we're not at yet.
                    else
                    {
                        // If the camera is returning to its subject.
                        if (isReturning)
                        {
                            transitionToRotation = defaultOffsetPosition; // Set the rotation we need to rotate to as the initial default (the closest distance to the player).
                        }
                        // Else the camera is looking away from the subject.
                        else
                        {
                            transitionToRotation = MaxRotation; // Set the rotation we ned to rotate to as the MaxRotation (the specified distance from the subject).
                        }

                        // Reset the amount rotated.
                        yRotationStepped = 0;
                        zRotationStepped = 0;
                        xRotationStepped = 0;
                        // We're ready to rotate.
                        isRotating = true;

                    }
                }
            }
            //else {
            //    transitionFromRotation = OffsetPosition; // Else get the current OffsetPosition and move from there.
            //}              
        }    
    }
}