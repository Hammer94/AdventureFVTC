using UnityEngine;

namespace AdventureFVTC {
    /**
     * A derivative camera for AdventureFVTC. Specifies
     * a height, drawback, and drawside that tells where the camera 
     * will be when it is locked.
     * Allows free setting of offsets and relative position.
     * Defines how the camera moves to different locations while
     * still looking at the subject.
     *
     * @author  Ryan
     * @date    24 Nov 2015
     * @see     Camera
     */
    public class FreeCamera : Camera {  
        private float transitionTime;
        private Vector3 transitionPosition;
        private bool isTransitioning = false;
        private bool isRotating = false;

        private bool hasChangedSubject = false;
        private bool isChangeStillTransitioning = false;
        private bool isReturning = false;
        private float yRotationStepped = 0;
        private float zRotationStepped = 0;
        private float xRotationStepped = 0;
        private Vector3 maxRotation;

        [SerializeField] protected float height = 1.0f;
        [SerializeField] protected float drawback = 5.0f;
        [SerializeField] protected float drawside = 0.0f;

        #region Accessors/Mutators
        /**
         * Saves drawside, height, and drawback as this camera's default position.
         * Saves the defaultCameraRotation as this cameras initial offset rotation.
         */
        private void setCameraDefault() {
            DefaultCameraPosition.Set(drawside, height, drawback);
            defaultCameraRotation = OffsetRotation;
        }

       /**
        * If the subject does have a facing direction and the subject hasn't just been changed, 
        * set the max rotation to the halfway point between the subject's facing direction and itself.
        * If the subject does have a facing direction and the subject has just been changed, set the max 
        * rotation to the camera's current offset rotation.
        * If the subject doesn't have a facing direction, set the max to the camera's current default rotation.
        * 
        * @return          The maxRotation.
        */
        private Vector3 MaxRotation {
            get {
                return maxRotation;
            }
            set {
                if (subjectFacingDirection != null) {
                    // If the subject has just been changed.
                    if (hasChangedSubject)
                        maxRotation = offsetRotation;
                    // Else the subject hasn't just been changed.
                    else {
                        float rotationY = (subjectFacingDirection.transform.position.y - subject.transform.position.y) / 2;
                        float rotationZ = (subjectFacingDirection.transform.position.z - subject.transform.position.z) / 2;
                        float rotationX = (subjectFacingDirection.transform.position.x - subject.transform.position.x) / 2;

                        maxRotation = new Vector3(rotationX, rotationY, rotationZ);
                    }              
                }
                else {
                    maxRotation = defaultCameraRotation;
                }
            }
        }

        /**
         * The height is how high above the subject the
         * camera is when the camera is locked.
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
                if (enabled)
                    relativePosition.y = height;
            }
        }

        /**
         * The drawback is how far behind the camera
         * positions itself when the camera is locked.
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
                if (enabled)
                    relativePosition.z = -drawback;
            }
        }

        /**
         * The drawside is how far right the camera
         * positions itself when the camera is locked.
         * Positive values move the camera to the right,
         * negative values move the camrera to the left.
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
                if (enabled)
                    relativePosition.x = drawside;
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
         * This override of Start adds height, drawback, 
         * drawside, setCameraDefault, and setMaxRotations
         * to the start method.
         */
        protected override void Start() {
            base.Start();

            Height = height;
            Drawback = drawback;
            Drawside = drawside;
            setCameraDefault();
            MaxRotation = MaxRotation;
        }

        /**
         * This override of Update adds the functionality
         * of the camera transitioning from one point to
         * another to the update method.
         *
         * This override also adds the functionality of the camera's
         * relative position being updated while the camera is locked 
         * so when the camera unlocks it can perform a smooth transition 
         * back to its subject instead of snapping there.
         */
        protected override void Update() {
            base.Update();

            // Is the camera currently unlocked?
            if (!lockPosition) {
                // Update the camera's relative position.
                relativePosition = subject.transform.position - transform.position;
            }

            // Is the camera transitioning?
            if (isTransitioning) {
                /** 
                 * If the camera is rotating, transition the camera's rotation to 
                 * either the camera's mid way point or its default.
                 * If the camera's subject has changed, transition the camera's rotation
                 * to the camera's 
                 */
                if (isRotating || hasChangedSubject) {
                    // Used to flag once the rotations have been reached.
                    bool yRotationReached = false;
                    bool zRotationReached = false;
                    bool xRotationReached = false;
                  
                    // Get the rotation rates relative to the camera's max rotation, insuring that
                    // the rotation rates are not dependent on the camera's current rotation.
                    float rotationRateY = maxRotation.y / transitionTime * Time.deltaTime;
                    float rotationRateZ = maxRotation.z / transitionTime * Time.deltaTime;
                    float rotationRateX = maxRotation.x / transitionTime * Time.deltaTime;

                    // Keep track of how far the camera has rotated so we can stop it later.
                    yRotationStepped += rotationRateY;
                    zRotationStepped += rotationRateZ;
                    xRotationStepped += rotationRateX;

                    // If the y-axis has rotated to (or passed) its maximum allowed rotation.
                    if (yRotationStepped >= maxRotation.y)
                    {
                        // Reduce the y-axis rotation rate.
                        rotationRateY -= (yRotationStepped - maxRotation.y);
                        yRotationReached = true;
                    }
                    // If the z-axis has rotated to (or passed) its maximum allowed rotation.
                    if (zRotationStepped >= maxRotation.z)
                    {
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

                    // Once all rotations have been flagged, stop the transition.
                    if (yRotationReached && zRotationReached && xRotationReached) {
                        isRotating = false;
                        hasChangedSubject = false;
                    }
                       
                    // If the camera is rotating back towards the subjects facing position.
                    if (isReturning) {
                        // Invert all the rotation rates.
                        rotationRateY *= -1;
                        rotationRateZ *= -1;
                        rotationRateX *= -1;
                    }

                    // Try to rotate the camera to the next x, y, and z offset rotation step (frame).
                    OffsetRotation.Set(OffsetRotation.x + rotationRateX, OffsetRotation.y + rotationRateY, OffsetRotation.z + rotationRateZ);
                }

                // Used as flags once the destination has been reached.
                bool yReached = false;
                bool zReached = false;
                bool xReached = false;

                // Calculate the y, z, and x position increase rates per frame.
                // Rotate the camera in three-quarters the time it takes to transition the camera's position (0.75).
                // This way the camera will be looking at the player before the camera gets to the player.
                float yChangeRate = (transitionPosition.y - relativePosition.y) / (transitionTime * 0.75f) * Time.deltaTime;
                float zChangeRate = (transitionPosition.z - relativePosition.z) / (transitionTime * 0.75f) * Time.deltaTime;
                float xChangeRate = (transitionPosition.x - relativePosition.x) / (transitionTime * 0.75f) * Time.deltaTime;

                // The camera has reached its y destination (or passed it), flag that it has been reached.
                if (height >= transitionPosition.y) {
                    yChangeRate -= (height - transitionPosition.y);  // shrink the rotation rate a bit.
                    yReached = true;
                }
                // The camera has reached its z destination (or passed it), flag that it has been reached.
                if (drawback >= transitionPosition.z) {
                    zChangeRate -= (drawback - transitionPosition.z);  // shrink the rotation rate a bit.
                    zReached = true;
                }
                // The camera has reached its x destination (or passed it), flag that it has been reached.
                if (drawside >= transitionPosition.x) {
                    xChangeRate -= (drawside - transitionPosition.z); // shrink the rotation rate a bit.
                    xReached = true;
                }

                // Once all destinations have been flagged, stop the transition.
                if (yReached && zReached && xReached) {
                    isTransitioning = false;
                    // If the subject change was still transitioning.
                    if (isChangeStillTransitioning) {               
                        isChangeStillTransitioning = false; // Signal that the subject change transition has finished.
                        hasChangedSubject = false;  // Reset the subject has changed tracker.                      
                        MaxRotation = MaxRotation;  // Reset the max rotation back to the mid way point.
                    }
                }

                Height += yChangeRate; // try to move the camera to the next y position step (frame).
                Drawback -= zChangeRate; // try to move the camera to the next z position step (frame).
                Drawside += xChangeRate; // try to move the camera to the next x position step (frame).              
            }    
        }
        #endregion

        /**
        * If the camera didn't achieve its full rotation last rotate, invert the amount rotated in the 
        * last rotation and flag that the camera is ready to rotate.
        * Else check the direction the camera wants to rotate to and the camera's current rotation.
        * If the camera wants to rotate towards a rotation its not currently at, flag that the camera
        * is ready to rotate and reset the amounts rotated.
        */
        private void TransitionSetUp()
        {           
            // If our OffsetRotation is between our default and our midway point.
            if (OffsetRotation != defaultCameraRotation && OffsetRotation != MaxRotation) {
                // We're ready to rotate.
                isRotating = true;
                // We're swapping the direction, so invert the rotationStepped.
                yRotationStepped = MaxRotation.y - yRotationStepped;
                zRotationStepped = MaxRotation.z - zRotationStepped;
                xRotationStepped = MaxRotation.x - xRotationStepped;
            }
            // Else the camera has finished its last rotation and is at its default or midway point.
            else {
                // If the camera is trying to rotate towards its defaultRotation but the camera is already there.
                if (isReturning && OffsetRotation == defaultCameraRotation)
                    // We shouldn't rotate.
                    isRotating = false;
                // If the camera is trying to rotate towards its midway point but the camera is already there.
                else if (!isReturning && OffsetRotation == MaxRotation)
                    // We shouldn't rotate.
                    isRotating = false;
                // Else we're trying to rotate towards a rotation that we're not at yet.
                else {
                    // We're ready to rotate.
                    isRotating = true;
                    // Reset the rotationStepped.
                    yRotationStepped = 0;
                    zRotationStepped = 0;
                    xRotationStepped = 0;
                }              
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
            if (!isChangeStillTransitioning) {
                // If the subject was just changed.
                if (hasChangedSubject) 
                    // Mark the transition as in progress.
                    isChangeStillTransitioning = true;
                                    
                isReturning = returnTo;
                TransitionSetUp();

                transitionPosition = position;
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
            if (!isChangeStillTransitioning) {
                // If the subject was just changed.
                if (hasChangedSubject) 
                    // Mark the transition as in progress.
                    isChangeStillTransitioning = true;
                                  
                isReturning = returnTo;
                TransitionSetUp();

                LockPosition = false;
                transitionPosition = position;
                transitionTime = time;
                isTransitioning = true;
            }      
        }

        /**
         * ToDo: 
         * Change the subject to a new subject. 
         * Change the subject facing direction to a new subject facing direction. 
         * Change the relativeposition of the camera to its current position
         * relative to the new subject.
         * Change the offsetRotation of the camera to its current rotation
         * relative to the new subject. 
         *
         * Changes the RelativePosition of the camera to its current
         * position relative to the new subject.
         * Calls the transition with subject method.
         *
         * @see     RelativePosition
         * @see     TransitionWithSubject()
         */
        public override void ChangeSubject(GameObject newsubject, GameObject facingdirectionName) {
            base.ChangeSubject(newsubject, facingdirectionName);

            // Signal that a subject was just changed.
            hasChangedSubject = true;
            // Get the current rotation before subjectchange.
            Vector3 oldRotation = transform.rotation.eulerAngles;
            // Reset the offset to default.
            offsetRotation = defaultCameraRotation;

            // Change the subject.
            subject = newsubject;
            // Get the subject's facing direction.
            subjectFacingDirection = facingdirectionName;
            
            // Get the new rotation after looking at the new subject.
            Vector3 newRotation = transform.rotation.eulerAngles;
            // Calculate the difference between the old and new rotation, the new offset.
            Vector3 newOffset = oldRotation - newRotation;

            // Set the new values of the camera relative to the player.
            offsetRotation = newOffset;
            relativePosition = subject.transform.position - transform.position;

            // Set the MaxRotation to the current offsetRotation;
            MaxRotation = MaxRotation;
        }
    }  
}