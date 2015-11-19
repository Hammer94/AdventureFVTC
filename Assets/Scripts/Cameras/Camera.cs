using UnityEngine;

namespace AdventureFVTC {
    /**
     * Suppresses the warning 1634 for a naming collision with
     * UnityEngine.Camera. This warning is not relevant because
     * PrometheusSyndicate.Camera is in its own namespace and
     * will not collide.
     */
#pragma warning disable 1634
    /**
     * A base camera class for AdventureFVTC. Allows a subject to
     * be specified that the camera will focus on and follow if its
     * position is locked. Allows an offset in position to be added so it
     * faces a postion not directly at the subject. Allows an offset in rotation
     * to allow the camera to be rotated while still facing the subject.
     * 
     * @author  Ryan
     * @date 19 Nov 2015
     */
    public class Camera : MonoBehaviour {
        protected Vector3 offsetPosition = new Vector3(0.0f, 0.0f, 0.0f);
        protected Vector3 offsetRotation = new Vector3(0.0f, 1.0f, 0.0f);
        protected Vector3 relativePosition = new Vector3(0.0f, 0.0f, 0.0f);

        [SerializeField] protected GameObject subject;
        [SerializeField] protected bool lockPosition = false;
        [SerializeField] protected bool lookWithPlayer = false;

        public Vector3 DefaultCameraPosition;

        /**
         * The subject is the object which the camera
         * focuses on. When locked the camera maintains
         * a relative position to the subject.
         * 
         * @param   value   The value to set as the subject.
         * @return          The subject.
         */
        public GameObject Subject {
            get {
                return subject;
            }
            set {
                subject = value;
                if (enabled)
                    lookAtSubject();
            }
        }

        /**
         * Allows the player to lock the camera to
         * its subject. When locking, the camera will
         * maintain its position from its current location.
         * 
         * @param   value   Whether the position is locked.
         * @return          True if the position is locked, false otherwise.
         */
        public virtual bool LockPosition {
            get {
                return lockPosition;
            }
            set {
                lockPosition = value;
                if (subject != null && enabled)
                    relativePosition = GetComponent<Transform>().position - subject.GetComponent<Transform>().position;
            }
        }

        /**
        * Allows the camera to turn with the player.
        * When turning, the camera will maintain its 
        * position from its current location.
        * 
        * @param   value   Whether the position is locked.
        * @return          True if the position is locked, false otherwise.
        */
        public virtual bool LookWithPlayer
        {
            get
            {
                return lookWithPlayer;
            }
            set
            {
                lookWithPlayer = value;
                if (enabled)
                    offsetRotation = GetComponent<Transform>().position - subject.GetComponent<Transform>().position;
            }
        }

        /**
         * Allows the camera to recieve input to
         * rotate. Accepts the input using right hand rule.
         * The direciton 0 is facing down the z axis.
         * 
         * @param   r   The angle to rotate in radians.
         */
        public void inputRotation(float r)
        {
            offsetRotation.z = r;
        }

        /**
         * Rotates the camera to face the subject.
         */
        private void lookAtSubject() {
            if (subject != null)
                GetComponent<Transform>().LookAt(subject.GetComponent<Transform>().position + offsetPosition, offsetRotation);
        }

        /**
         * Moves the camera with the subject.
         */
        private void followSubject() {
            if (subject != null)
                GetComponent<Transform>().position = subject.GetComponent<Transform>().position + relativePosition;
        }

        /**
        * Saves position,y, position.z, and position.x 
        * as this camera's default position.
        */
        protected virtual void setCameraDefault()
        {
            DefaultCameraPosition.y = gameObject.transform.position.y;
            DefaultCameraPosition.z = gameObject.transform.position.z;
            DefaultCameraPosition.x = gameObject.transform.position.x;
        }

        /**
         * Disables this behavior if the object does
         * not have a UnityEngine.Camera associated with it.
         * Sets all values based on their restrictions.
         */
        protected virtual void Start() {
            if (GetComponent<UnityEngine.Camera>() == null)
                enabled = false;
            Subject = subject;
            LockPosition = lockPosition;
        }

        /**
         * Moves the camera every frame in order to
         * keep the camera in its relative position
         * if locked and to rotate the camera.
         */
        protected virtual void Update() {
            if (lockPosition)
                followSubject();
            lookAtSubject();
        }

        /**
         * Rotates the camera as the subject rotates. 
         */
        public void RotateWithSubject(Vector3 anglechange) {
            if (lookWithPlayer)
            {
                //gameObject.transform.rotation.SetEulerAngles(anglechange);
                gameObject.transform.eulerAngles.Set(anglechange.x, anglechange.y, anglechange.z);
            }
        }
    }
/**
 * Closing pragma for earlier warning suppression.
 */
#pragma warning restore 1634
}