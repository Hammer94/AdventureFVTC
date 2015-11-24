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
     * @date    24 Nov 2015
     */
    public class Camera : MonoBehaviour {
        protected Vector3 offsetPosition = new Vector3(0.0f, 0.0f, 0.0f);
        protected Vector3 offsetRotation = new Vector3(0.0f, 0.0f, 0.0f);
        protected Vector3 relativePosition = new Vector3(0.0f, 0.0f, 0.0f);

        [SerializeField] protected GameObject subject;
        [SerializeField] protected bool lockPosition = false;

        protected Vector3 defaultCameraRotation;
        protected GameObject subjectFacingDirection;
        protected Vector3 defaultCameraPosition;

        /**
         * The subject is an object which the camera can
         * focus on. When locked the camera maintains
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
            }
        }

        /**
         * The subject's Facing Direction is an object 
         * which the camera can focus on. When locked 
         * the camera maintains a relative position to the subject.
         * 
         * @param   value   The value to set as the subject's facing direction.
         * @return          The subject's facing direction.
         */
        public GameObject SubjectFacingDirection {
            get
            {
                return subjectFacingDirection;
            }
            set
            {
                subjectFacingDirection = value;
                if (enabled)
                    lookAtSubject();
            }
        }

        /**
         * Allows the player to lock the camera to
         * its subject. When locked, the camera will
         * maintain its position from its current subject.
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
         * The defaultCameraPosition is where the camera can return
         * to at any given time.
         * 
         * @param   value   The value to set as the defaultCameraPosition.
         * @return          The defaultCameraPosition.
         */
        public Vector3 DefaultCameraPosition {
            get {
                return defaultCameraPosition;
            }
            set {
                defaultCameraPosition = value;
            }
        }

        /**
          * Will store the returning boolean to allow inverting the rotation.
          * Will performs transition setup.
          * Will stores the location the camera needs to travel to, how much time it 
          * should take, and then signal that the transition is ready.
          *
          * @param   position    The position the camera needs to move to.
          * @param   time        The amount of time it should take for the 
          *                      camera moving from one point to another. 
          * @param   returnTo    The flag if the camera should return to
          *                      its default position.
          */
        public virtual void TransitionWithSubject(Vector3 position, float time, bool returnToSubject)
        {
           
        }

        /**
         * Will store the returning boolean to allow inverting the rotation.
         * Will performs transition setup.
         * Will unlocks the camera so it no longer moves relative to the player.
         * Will stores the location the camera needs to travel to, how much time it 
         * should take, and then signal that the transition is ready.
         *
         * @param   position    The position the camera needs to move to.
         * @param   time        The amount of time it should take for the 
         *                      camera moving from one point to another. 
         * @param   returnTo    The flag if the camera should return to
         *                      its default position.
         */
        public virtual void TransitionToPoint(Vector3 position, float time, bool returnToSubject)
        {

        }

        /**
         * Will change the RelativePosition of the camera to its current
         * position relative to the new subject.
         * Will call the transition with subject method.
         *
         * @see     RelativePosition
         * @see     TransitionWithSubject()
         */
        public virtual void ChangeSubject()
        {

        }

        /**
         * Rotates the camera to face the subject's facing direction or the subject.
         */
        private void lookAtSubject() {
            if (subjectFacingDirection != null)
                GetComponent<Transform>().LookAt(subjectFacingDirection.GetComponent<Transform>().position + offsetPosition, offsetRotation);
            else if (subject != null)
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
         * if locked. 
         * If the camera's isn't already rotating with
         * the subject, look at the subject.
         */
        protected virtual void Update() {
            if (lockPosition)
                followSubject();
            lookAtSubject();
        }
    }
/**
 * Closing pragma for earlier warning suppression.
 */
#pragma warning restore 1634
}