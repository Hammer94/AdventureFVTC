using UnityEngine;

namespace AdventureFVTC {
    /**
    * A derivative camera node for AdventureFVTC. Receives a time the
    * the node is in the game space and a time it takes for the camera
    * to transition to the player.
    * Orbits the camera until the amount of time the node has been active
    * reaches or exceeds its specified lifeTime. Once its lifeTime has been
    * met, changes the camera's subject to the player and begins the camera's
    * transition.
    *
    * @author   Ryan
    * @date     29 Nov 2015
    * @see      CameraNode
    * @see      CameraService
    */
    public class SubjectNode:CameraNode {
        private float timeWaited = 0.0f;
        private Vector3 desiredPosition;

        public bool beginLifeTime = false;
        public Vector3 playerCamera;      
        public float radius = 10.0f; // How far from the center the camera node rotates.
        public float radiusSpeed = 0.5f;

        [SerializeField] private float transitionToPlayerTime = 3.0f; // The time it takes the camera to get to the player.
        [SerializeField] private float lifeTime = 3.0f; // The time the subject node is active and the time it takes 
                                                        // the node to perform a full rotation around the camera.
        /**
         * Receives a position to set as this nodes center of rotation.
         */
        public void InitialSetUp(Vector3 camera) {
            playerCamera = camera;
            transform.position.Set((transform.position.x - playerCamera.x) * radius + playerCamera.x, transform.position.y, 
                (transform.position.z - playerCamera.z) * radius + playerCamera.z);
            beginLifeTime = true;
        }
        
        protected override void Start() {
            base.Start();

            LifeTime = lifeTime;
        }

        public float LifeTime {
            get {
                return lifeTime;
            }
            set {
                lifeTime = value;
            }
        }

        /**
         * This override of update adds in the functionality
         * of the subject node having a lifetime. and orbiting
         * the camera. Once the lifeTime has been met, calls for 
         * the subject changing to the player and the removal of
         * the node from the gamespace.
         */
        protected override void Update() {
            base.Update();

            if (beginLifeTime) {
                float rate = (360 / lifeTime) * Time.deltaTime;

                transform.RotateAround(playerCamera, Vector3.up, rate);
                desiredPosition = (transform.position - playerCamera).normalized * radius + playerCamera;
                transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed);
                           
                timeWaited += Time.deltaTime;
                if (timeWaited >= lifeTime) {                 
                    Services.Camera.SetSubjectToPlayer(transitionToPlayerTime);
                    Destroy(gameObject);
                }
            }
        }
    }
}
