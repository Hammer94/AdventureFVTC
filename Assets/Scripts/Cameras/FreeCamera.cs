using UnityEngine;

namespace AdventureFVTC
{
    /**
    * A derivative camera for AdventureFVTC. Specifies
     * a height and drawback that specify where the camera will
     * be when it is locked.
    * Allows free setting of offsets and relative position.
    * Defines how the camera moves to different locations while
    * still looking at the subject.
    *
    * @author Ryan
    * @date	  17 Nov 2015
    * @see    Camera
    */
    public class FreeCamera : Camera {
        [SerializeField] private float height = 1.0f;
        [SerializeField] private float drawback = 5.0f;
        private float DEFAULTHEIGHT;
        private float DEFAULTDRAWBACK;
        
        /**
        * The playerheight is how high above the subject the
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
                DEFAULTHEIGHT = height;
                Drawback = drawback;
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
                DEFAULTDRAWBACK = drawback;
                //if (playerdrawback > playerheight)
                   // playerdrawback = playerheight;
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
    
        /**
        * Public accessor/mutator to offsetPosition in Camera.
        *
        * @param value  The value to set as the offset.
        * @return       The offset position.
        */
        public Vector3 OffsetPosition
        {
            get
            {
                return offsetPosition;
            }
            set
            {
                offsetPosition = value;
            }
        }

        /**
        * Public accessor/mutator to offsetRotation in Camera.
        *
        * @param value  The value to set as the offset.
        * @return       The offset rotation.
        */
        public Vector3 OffsetRotation
        {
            get
            {
                return offsetRotation;
            }
            set
            {
                offsetRotation = value;
            }
        }

        /**
        * Public accessor/mutator to relativePosition in Camera.
        *
        * @param value  The value to set as the position
        * @return       The relative position.
        */
        public Vector3 RelativePosition
        {
            get
            {
                return relativePosition;
            }
            set
            {
                relativePosition = value;
            }
        }

        private void TransitionWithPlayer()
        {

        }

        private void TransitionWithMap()
        {

        }
    }  
}