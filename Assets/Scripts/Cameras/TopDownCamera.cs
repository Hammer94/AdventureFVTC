using UnityEngine;

namespace AdventureFVTC {
    /**
     * A derivative camera for AdventureFVTC. Specifies
     * a height and drawback that specify where the camera will
     * be when it is locked.
     * 
     * @author  Ryan
     * @date 17 Nov 2015
     */
    public class TopDownCamera : Camera {
        [SerializeField] private float height = 15.0f;
        [SerializeField] private float drawback = 5.0f;

        /**
         * The height is how high above the subject the
         * camera is when the camera is locked.
         * When setting the height this property ensures
         * the drawback stays smaller than the height.
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
                Drawback = drawback;
                if (enabled)
                    relativePosition.y = height;
            }
        }

        /**
         * The drawback is how far behind the camera
         * positions itself when the camera is locked.
         * When setting the drawback this property ensures
         * the drawback stays smaller than the height.
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
                if (drawback > height)
                    drawback = height;
                if (enabled)
                    relativePosition.z = -drawback;
            }
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
         * Adds height and drawback to the start method.
         */
        protected override void Start() {
            base.Start();
            Height = height;
        }
    }
}