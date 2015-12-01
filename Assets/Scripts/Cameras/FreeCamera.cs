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
     * @date    30 Nov 2015
     * @see     Camera
     */
    public class FreeCamera:CameraBase {  
        private float transitionTime;
        private Vector3 transitionPosition;
        private bool isTransitioning = false;
        private bool isTransitioningRelativeToSubject = true;
        private bool isRotating = false;

        private bool hasChangedSubject = false;
        protected bool isSubjectChangeStillTransitioning = false;
        private bool isReturning = false;
        private float yRotationStepped = 0;
        private float zRotationStepped = 0;
        private float xRotationStepped = 0;
        private float yMovementStepped = 0.0f;
        private float zMovementStepped = 0.0f;
        private float xMovementStepped = 0.0f;
        private Vector3 maxRotation;
        private Vector3 maxMovement;
        private Vector3 rotationDifference = Vector3.zero;
        private Vector3 positionDifference = Vector3.zero;

        [SerializeField] protected float height = 0.0f;
        [SerializeField] protected float drawback = 0.0f;
        [SerializeField] protected float drawside = 0.0f;

        #region Accessors/Mutators
        /**
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
                    maxRotation = rotationDifference;        
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
        private Vector3 MaxMovement {
            get {    
                return maxMovement;
        }
            set {
                // If the value equals the current maxMovement or the current positionDifference.
                if (value == positionDifference) {
                    // If the subject has just been changed.
                    if (hasChangedSubject)
                        maxMovement = positionDifference;
                    // Else the subject hasn't just been changed (This will only be called once
                    // on initialization).
                    //else
                    //{
                    //    // Set the maximum the camera can move default camera position.
                    //    maxMovement = defaultRelativeCameraPosition;
                    //}
                }
                // Else the value is a new position the camera needs to move to.
                else
                    // Set the max movement as the new value.
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

        /**
         * Public accessor/mutator to relativePosition in Camera.
         *
         * @param value  The value to set as the position
         * @return       The relative position.
         */
        public Vector3 RelativePosition {
            get {
                return relativePosition;
            }
            set {
                relativePosition = value;
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

            Debug.Log("Inside TransitionWithSubject");

            // Has the change subject transition finished?
            if (!isSubjectChangeStillTransitioning) {
                // If the subject was just changed.
                if (hasChangedSubject)
                    // Mark the transition as in progress.
                    isSubjectChangeStillTransitioning = true;

                isReturning = returnTo;
                transitionPosition = position;
                transitionTime = time;

                isTransitioningRelativeToSubject = true;
                TransitionSetUp();
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

                isReturning = returnTo;
                transitionPosition = position;
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
            // Remove the subject's facing direction.
            subjectFacingDirection = null;

            // Rotate towards the current subject before subjectchange.
            //lookAtSubject();
            transform.LookAt(subject.transform.position + offsetPosition, offsetRotation);

            // Get the current rotation before subjectchange.
            Vector3 oldRotation = transform.rotation.eulerAngles;
            Debug.Log("oldRotation = " + (oldRotation).ToString());

            Debug.Log("positionDifference = " + positionDifference);
            // If the subject has a behind direction.
            if (newBehindDirection != null)
                // Get the new position difference of the new subject's new behind position and the camera.
                positionDifference = newBehindDirection.transform.position - transform.position;
            else
                // Get the new position difference of the new subject position and the camera.
                positionDifference = newsubject.transform.position - transform.position;
            Debug.Log("positionDifference = " + positionDifference);

            if (positionDifference.x < 0)
                positionDifference.x *= -1;
            if (positionDifference.y < 0)
                positionDifference.y *= -1;
            if (positionDifference.z < 0)
                positionDifference.z *= -1;

            // Set the relativePosition to the difference so the camera keep its position relative to its new subject.
            relativePosition = relativePosition + positionDifference;
            Debug.Log("relativePosition = " + relativePosition);
            // Set the current behind direction to the new behind direction.
            subjectBehindDirection = newBehindDirection;
            // Set the current subject to the new subject.
            subject = newsubject;

            // Rotate towards the new subject after subjectchange.
            //lookAtSubject();
            transform.LookAt(subject.transform.position + offsetPosition, offsetRotation);

            // Get the new rotation after looking at the new subject.
            Vector3 newRotation = transform.rotation.eulerAngles;
            Debug.Log("newRotation = " + (newRotation).ToString());
            // Get the difference between the old and new rotation, the new offset.
            rotationDifference = oldRotation - newRotation;
            Debug.Log("rotationDifference = " + (rotationDifference).ToString());

            // Update the camera's new rotation.
            OffsetPosition = rotationDifference;

            // Get the subject's facing direction.
            subjectFacingDirection = newFacingDirection;

            if (rotationDifference.x < 0)
                rotationDifference.x *= -1;
            if (rotationDifference.y < 0)
                rotationDifference.y *= -1;
            if (rotationDifference.z < 0)
                rotationDifference.z *= -1;

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
                Debug.Log("isSubjectChangeStillTransitioning = " + (isSubjectChangeStillTransitioning == true).ToString());
                /** 
                 * If the camera is rotating, transition the camera's rotation to 
                 * either the camera's mid way point or its default.
                 * If the camera's subject has changed, transition the camera's rotation
                 * to the camera's new max rotation.
                 */
                if (isRotating || hasChangedSubject) {
                    // Used to flag once the rotations have been reached.
                    bool yRotationReached = false;
                    bool zRotationReached = false;
                    bool xRotationReached = false;

                    // Get the rotation rates of the camera's max rotation, insuring that
                    // the camera's rotation rate is is relative to how far the camera is allowed to rotate per full rotation.
                    // Rotate the camera in three-quarters the time it takes to transition the camera's position (0.75 amount of time).
                    // This way the camera will be looking at the player before the camera gets to the player.
                    float rotationRateY = maxRotation.y / (transitionTime * 0.75f) * Time.deltaTime;
                    float rotationRateZ = maxRotation.z / (transitionTime * 0.75f) * Time.deltaTime;
                    float rotationRateX = maxRotation.x / (transitionTime * 0.75f) * Time.deltaTime;

                    // Keep track of how far the camera has rotated so we can stop it later.
                    yRotationStepped += rotationRateY;
                    zRotationStepped += rotationRateZ;
                    xRotationStepped += rotationRateX;

                    // If the y-axis has rotated to (or passed) its maximum allowed rotation.
                    if (yRotationStepped >= maxRotation.y) {
                        // Reduce the y-axis rotation rate.
                        rotationRateY -= (yRotationStepped - maxRotation.y);
                        yRotationReached = true;
                    }
                    // If the z-axis has rotated to (or passed) its maximum allowed rotation.
                    if (zRotationStepped >= maxRotation.z) {
                        // Reduce the z-axis rotation rate.
                        rotationRateZ -= (zRotationStepped - maxRotation.z);
                        zRotationReached = true;
                    }
                    // If the x-axis has rotated to (or passed) its maximum allowed rotation.
                    if (xRotationStepped >= maxRotation.x) {
                        // Reduce the x-axis rotation rate.
                        rotationRateX -= (xRotationStepped - maxRotation.x);
                        xRotationReached = true;
                    }

                    // Once all rotations have been flagged. 
                    if (yRotationReached && zRotationReached && xRotationReached) {
                        isRotating = false; // Stop the transition.
                        // If the subject has just been changed.
                        if (hasChangedSubject) {
                            // Signal the subject change rotation has finished.
                            hasChangedSubject = false;
                            // Reset the camera's max rotation to its default rotation or the position
                            // relative to the subject and its facing direction.
                            MaxRotation = maxRotation;
                        }       
                    }
                       
                    // If the camera is rotating back towards the subject's facing position.
                    if (isReturning) {
                        // Invert all the rotation rates.
                        rotationRateY *= -1;
                        rotationRateZ *= -1;
                        rotationRateX *= -1;
                    }

                    // Try to rotate the camera to the next x, y, and z offset rotation step (frame).
                    //OffsetPosition.Set(OffsetPosition.x + rotationRateX, OffsetPosition.y + rotationRateY, OffsetPosition.z + rotationRateZ);
                    OffsetPosition = new Vector3(OffsetPosition.x + rotationRateX, OffsetPosition.y + rotationRateY, OffsetPosition.z + rotationRateZ);
                }

                // Used as flags once the destination has been reached.
                bool yReached = false;
                bool zReached = false;
                bool xReached = false;

                // Calculate the y, z, and x position increase rates per frame.
                float yChangeRate = (transitionPosition.y - relativePosition.y) / transitionTime * Time.deltaTime;
                float zChangeRate = (transitionPosition.z - relativePosition.z) / transitionTime * Time.deltaTime;
                float xChangeRate = (transitionPosition.x - relativePosition.x) / transitionTime * Time.deltaTime;

                // If the y change rate is negative.
                if (yChangeRate < 0)
                    // Increase the amount stepped by the opposite of y's value (a positive value).
                    yMovementStepped += -1 * yChangeRate;
                // Else the y change rate is positive or 0.
                else if (yChangeRate >= 0)
                    // Increase the amount stepped by y's value (a positive value).
                    yMovementStepped += yChangeRate;
                // If the z change rate is negative.

                if (zChangeRate < 0)
                    // Increase the amount stepped by the opposite of z's value (a positive value).
                    zMovementStepped += -1 * zChangeRate;
                // Else the z change rate is positive or 0.
                else if (zChangeRate >= 0)
                    // Increase the amount stepped by z's value (a positive value).
                    zMovementStepped += zChangeRate;
                // If the x change rate is negative.

                if (xChangeRate < 0)
                    // Increase the amount stepped by the opposite of x's value (a positive value).
                    xMovementStepped += -1 * xChangeRate;
                // Else the x change rate is positive or 0.
                else if (yChangeRate >= 0)
                    // Increase the amount stepped by x's value (a positive value).
                    xMovementStepped += xChangeRate;

                // The camera has reached its y destination (or passed it), flag that it has been reached.
                if (yMovementStepped >= MaxMovement.y) {
                    yChangeRate -= (yMovementStepped - MaxMovement.y);  // shrink the movement rate a bit.
                    yReached = true;
                }
                // The camera has reached its z destination (or passed it), flag that it has been reached.
                if (zMovementStepped >= MaxMovement.z) {
                    zChangeRate -= (zMovementStepped - MaxMovement.z);  // shrink the movement rate a bit.
                    zReached = true;
                }
                // The camera has reached its x destination (or passed it), flag that it has been reached.
                if (xMovementStepped >= MaxMovement.x) {
                    xChangeRate -= (xMovementStepped - MaxMovement.z); // shrink the movement rate a bit.
                    xReached = true;
                }

                // Once all destinations have been flagged.
                if (yReached && zReached && xReached) {
                    isTransitioning = false; // Stop the transition.
                    
                    // If the subject change was still transitioning.
                    if (isSubjectChangeStillTransitioning) {
                        isSubjectChangeStillTransitioning = false; // Signal that the subject change transition has finished.                      
                    }
                }

                Height += yChangeRate; // try to move the camera to the next y position step (frame).
                Drawback += zChangeRate; // try to move the camera to the next z position step (frame).
                Drawside += xChangeRate; // try to move the camera to the next x position step (frame).              
            }    
        }       
        #endregion

        /**
         * Saves drawside, height, and drawback as this camera's default position.
         * Saves this camera's initial offset rotation as its defaultCameraRotation.
         */
        private void setCameraDefault() {
            DefaultRelativePosition.Set(drawside, height, drawback);
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
            Debug.Log("Inside TransitionSetUp");

            // If the camera is moving relative to the subject and to a new relative position.
            if (isTransitioningRelativeToSubject) {
                Debug.Log("Inside TransitioningRelativeToSubject");
                // Move the camera with the subject.
                LockPosition = true;
                
                // Get the difference between where the camera relatively needs to go to and where the camera relatively is.
                float positiony = transitionPosition.y - relativePosition.y;
                float positionz = transitionPosition.z - relativePosition.z;
                float positionx = transitionPosition.x - relativePosition.x;
                
                // Get the positive difference for each axis. This is how far
                // the camera is allowed to move on each axis.
                if (positiony < 0)
                    positiony = -1 * positiony;
                if (positionz < 0)
                    positionz = -1 * positionz;
                if (positionx < 0)
                    positionx = -1 * positionx;

                Debug.Log("positiony = " + (positiony).ToString());
                Debug.Log("positionz = " + (positionz).ToString());
                Debug.Log("positionx = " + (positionx).ToString());

                // Reset the counters for how far each axis has moved.
                yMovementStepped = 0;
                zMovementStepped = 0;
                xMovementStepped = 0;
                // Update the new max the camera is allowed to move.
                MaxMovement = new Vector3(positionx, positiony, positionz);
                Debug.Log("MaxMovement = " + (MaxMovement).ToString());
            }
            // Else the camera is not moving relative to the subject and is moving to a new position.
            else {
                // Set the camera to not move with the player.
                lockPosition = false;

                // Get the difference between the where the camera needs to go to and where the camera is.
                float positiony = transitionPosition.y - transform.position.y;
                float positionz = transitionPosition.z - transform.position.z;
                float positionx = transitionPosition.x - transform.position.x;

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
            
            // If our OffsetRotation is between our default and our midway point.
            if (OffsetPosition != defaultOffsetPosition && OffsetPosition != MaxRotation) {
                Debug.Log("We're between our default and midwaypoint!");
                // We're ready to rotate.
                isRotating = true;
                // We're swapping the direction, so invert the rotationStepped.
                yRotationStepped = MaxRotation.y - yRotationStepped;
                zRotationStepped = MaxRotation.z - zRotationStepped;
                xRotationStepped = MaxRotation.x - xRotationStepped;
            }
            // Else the camera has finished its last rotation and is at its default rotation or max rotation.
            else {
                // If the camera is trying to rotate towards its defaultOffsetPosition but the camera is already there.
                if (isReturning && OffsetPosition == defaultOffsetPosition) {
                    // We shouldn't rotate.
                    isRotating = false;
                    Debug.Log("We're at default and are not rotating!");
                }
                // If the camera is trying to rotate towards its max rotation but the camera is already there.
                else if (!isReturning && OffsetPosition == MaxRotation) { 
                // We shouldn't rotate. 
                    isRotating = false;
                    Debug.Log("We're at max and are not rotating!");
                }
                // Else we're trying to rotate towards a rotation that we're not at yet.
                else {
                    Debug.Log("We're rotating! towards a direction we're not at!");
                    // We're ready to rotate.
                    isRotating = true;
                    // Reset the amount rotated.
                    yRotationStepped = 0;
                    zRotationStepped = 0;
                    xRotationStepped = 0;
                }              
            }    
        }    
    }
}