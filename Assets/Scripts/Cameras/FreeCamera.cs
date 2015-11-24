using UnityEngine;

namespace AdventureFVTC {
    /**
    * A derivative camera for AdventureFVTC. Specifies
    * a height, drawback, and drawside that specify where the camera 
    * will be when it is locked.
    * Allows free setting of offsets and relative position.
    * Defines how the camera moves to different locations while
    * still looking at the subject.
    *
    * @author Ryan
    * @date	  19 Nov 2015
    * @see    Camera
    */
    public class FreeCamera : Camera {  
        private float transitionTime;
        private Vector3 transitionPosition;
        private bool transitioning = false;

        [SerializeField] protected float height = 1.0f;
        [SerializeField] protected float drawback = 5.0f;
        [SerializeField] protected float drawside = 0.0f;

        #region Accessors/Mutators
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
        * This override of setCameraDefault Saves 
        * Height, DrawBack, and Drawside as this 
        * camera's default position.
        */
        protected override void setCameraDefault() {
            DefaultCameraPosition.y = Height;
            DefaultCameraPosition.z = Drawback;
            DefaultCameraPosition.x = Drawside;
        }

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
        * drawside and setCameraDefault to the start method.
        */
        protected override void Start() {
            base.Start();

            Height = height;
            Drawback = drawback;
            Drawside = drawside;
            setCameraDefault();
        }

        /**
        * This override of Update adds the functionality
        * of the camera transitioning from one point to
        * another to the update method.
        * ToDo: reference lookWithPlayer
        */
        protected override void Update() {
            base.Update();

            if (transitioning) {
                // used as flags once the desitination has been reached
                bool yReached = false;
                bool zReached = false;
                bool xReached = false;

                // Calculate the y, z, and x position increase rates per frame.
                float yIncreaseRate = (transitionPosition.y - DefaultCameraPosition.y) / transitionTime * Time.deltaTime;
                float zIncreaseRate = (transitionPosition.z - DefaultCameraPosition.z) / transitionTime * Time.deltaTime;
                float xIncreaseRate = (transitionPosition.x - DefaultCameraPosition.x) / transitionTime * Time.deltaTime;

                Height += yIncreaseRate; // try to move the camera to the next y position step (frame).
                Drawback += zIncreaseRate; // try to move the camera to the next z position step (frame).
                Drawside += xIncreaseRate; // try to move the camera to the next x position step (frame).

                // The camera has reached its y destination (or passed it), flag that it has been reached.
                if (Height >= transitionPosition.y) {
                    Height -= (Height - transitionPosition.y);
                    yReached = true;
                }
                // The camera has reached its z destination (or passed it), flag that it has been reached.
                if (Drawback >= transitionPosition.z) {
                    Drawback -= (Drawback - transitionPosition.z);
                    zReached = true;
                }
                // The camera has reached its x destination (or passed it), flag that it has been reached.
                if (Drawside >= transitionPosition.x) {
                    Drawside -= (Drawback - transitionPosition.z);
                    xReached = true;
                }

                // Once all destinations have been flagged, stop the transition.
                if (yReached && zReached && xReached)
                    transitioning = false;
            }
        #endregion
        }

        /**
        * Stores the location the camera needs to 
        * travel to, how much time it should
        * take, and then signals that the transition
        * is ready.
        *
        * @param    position    The position the camera
        *                       needs to move to.
        * @param    time        The amount of time it 
        *                       should take for the 
        *                       camera moving from one
        *                       point to another.       
        */
        private void TransitionWithPlayer(Vector3 position, float time)
        {
            transitionPosition = position;
            transitionTime = time;
            transitioning = true;
        }

        /** 
        * ToDo: Disinct this method from TransitionWithPlayer
        * by removing the camera's subject and add functionality
        * of the camera spinning around to get good view of the map.
        */
        private void TransitionWithMap()
        {

        }
    }  
}