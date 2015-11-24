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
        private bool transitioning = false;
        private bool isRotating = false;

        private bool hasChangedSubject = false;
        private bool isReturning = false;
        private float yRotationStepped = 0;
        private float zRotationStepped = 0;
        private float xRotationStepped = 0;
        private float yMaxRotation;
        private float xMaxRotation;
        private float zMaxRotation;

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
            setMaxRotations();
        }

        /**
         * This override of Update adds the functionality
         * of the camera transitioning from one point to
         * another to the update method.
         */
        protected override void Update() {
            base.Update();

            // Are we transitioning?
            if (transitioning) {
                /** 
                 * If the camera is rotating and the subject has a facing 
                 * direction, transition the camera's focus to the to either
                 * the camera's max rotation or its default.
                 */
                if (isRotating && GetMax() != Vector3.zero) {
                    // Used as flags once the rotation has been reached.
                    bool yRotationReached = false;
                    bool zRotationReached = false;
                    bool xRotationReached = false;
                    
                    // Get the camera's max rotation.
                    Vector3 maxRotation = GetMax();

                    // Get the rotation rates relative to the camera's max rotation, insuring that
                    // the rotation rates are not dependend on the camera's current position.
                    float rotationRateY = maxRotation.y / transitionTime * Time.deltaTime;
                    float rotationRateZ = maxRotation.z / transitionTime * Time.deltaTime;
                    float rotationRateX = maxRotation.x / transitionTime * Time.deltaTime;

                    // Keep track of how far the camera has rotated so we can stop it later.
                    yRotationStepped += rotationRateY;
                    zRotationStepped += rotationRateZ;
                    xRotationStepped += rotationRateX;

                    // If the y-axis has rotated to (or passed) its maximum allowed rotation.
                    if (yRotationStepped >= yMaxRotation)
                    {
                        // Reduce the y-axis rotation rate.
                        rotationRateY -= (yRotationStepped - yMaxRotation);
                        yRotationReached = true;
                    }
                    // If the z-axis has rotated to (or passed) its maximum allowed rotation.
                    if (zRotationStepped >= zMaxRotation)
                    {
                        // Reduce the z-axis rotation rate.
                        rotationRateZ -= (zRotationStepped - zMaxRotation);
                        zRotationReached = true;
                    }
                    // If the x-axis has rotated to (or passed) its maximum allowed rotation.
                    if (xRotationStepped >= xMaxRotation) {
                        // Reduce the x-axis rotation rate.
                        rotationRateX -= (xRotationStepped - xMaxRotation);
                        xRotationReached = true;
                    }

                    // Once all rotations have been flagged, stop the transition.
                    if (yRotationReached && zRotationReached && xRotationReached)
                        isRotating = false;

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
                float yChangeRate = (transitionPosition.y - DefaultCameraPosition.y) / transitionTime * Time.deltaTime;
                float zChangeRate = (transitionPosition.z - DefaultCameraPosition.z) / transitionTime * Time.deltaTime;
                float xChangeRate = (transitionPosition.x - DefaultCameraPosition.x) / transitionTime * Time.deltaTime;

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
                    transitioning = false;
                }

                Height += yChangeRate; // try to move the camera to the next y position step (frame).
                Drawback -= zChangeRate; // try to move the camera to the next z position step (frame).
                Drawside += xChangeRate; // try to move the camera to the next x position step (frame).              
            }    
        }
        #endregion

        /**
         * Saves the max rotation for each axis if the subject's facing distance isn't null. 
         * Else sets each max to 0.
         */
        private void setMaxRotations() {
            if (subjectFacingDirection != null) {
                // Set the max as half the distance between the subject and its facing direction.
                yMaxRotation = (subjectFacingDirection.transform.position.y - subject.transform.position.y) / 2;
                zMaxRotation = (subjectFacingDirection.transform.position.z - subject.transform.position.z) / 2;
                xMaxRotation = (subjectFacingDirection.transform.position.x - subject.transform.position.x) / 2;
            }
            else {
                yMaxRotation = 0;
                zMaxRotation = 0;
                xMaxRotation = 0;
            }           
        }

        /**
        * Get half the distance between the subject and its facing direction.
        */
        private Vector3 GetMax()
        {
            return new Vector3(xMaxRotation, yMaxRotation, zMaxRotation);
        }

        /**
        * Invert the amount rotated in preperation for rotating if full rotation was not achieved
        * last rotate.
        * Else reset the rotations.
        */
        private void TransitionSetUp()
        {
            // If our OffsetRotation is between our default and our midway point.
            if (OffsetRotation != defaultCameraRotation && OffsetRotation != GetMax()) {
                // We're swapping the direction, so invert the rotationStepped.
                yRotationStepped = yMaxRotation - yRotationStepped;
                zRotationStepped = zMaxRotation - zRotationStepped;
                xRotationStepped = xMaxRotation - xRotationStepped;
            }
            // Else our OffsetRotation has finished last rotation and is at our default or midway point.
            else {
                yRotationStepped = 0;
                zRotationStepped = 0;
                xRotationStepped = 0;
            }    
        }

        /**
         * Stores the returning boolean to allow inverting the rotation.
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

            isReturning = returnTo;
            TransitionSetUp();

            transitionPosition = position;
            transitionTime = time;
            isRotating = true;
            transitioning = true;
        }

        /** 
         * ToDo: Disinct this method from TransitionWithPlayer
         * by removing the camera's subject and add functionality
         * of the camera spinning around to get good view of the map.
         * 
         * Stores the returning boolean to allow inverting the rotation.
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

            isReturning = returnTo;
            TransitionSetUp();

            LockPosition = false;
            transitionPosition = position;
            transitionTime = time;
            isRotating = true;
            transitioning = true;         
        }

        /**
         * ToDo: Change the relativeposition of the camera to its current position
         * relative to the new subject.
         *
         * Changes the RelativePosition of the camera to its current
         * position relative to the new subject.
         * Calls the transition with subject method.
         *
         * @see     RelativePosition
         * @see     TransitionWithSubject()
         */
        public override void ChangeSubject() {
            base.ChangeSubject();


        }
    }  
}