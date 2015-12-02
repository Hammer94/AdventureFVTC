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
    * @date     30 Nov 2015
    * @see      CameraNode
    * @see      CameraService
    */
    public class SubjectNode:CameraNode {
        private float timeWaited = 0.0f;         
        private bool orbit = false;
        private bool destroySelf = false;
        private Vector3 playerCamera;
        private Vector3 desiredPosition;


        [SerializeField] private float radius = 10.0f; // How far from the center the camera node rotates.
        [SerializeField] private float radiusSpeed = 0.5f;
        [SerializeField] private float transitionToPlayerTime = 3.0f; // The time it takes the camera to get to the player.
        [SerializeField] private float orbitTime = 10.0f; // The time it takes the subject node to orbit the camera. 
        [SerializeField] private float destroyTime = 2.0f; // The time it takes the subject node to destroy itself.
                                                        
        /**
         * Receives a position to set as this nodes center of rotation.
         */
        public void InitialSetUp(Vector3 camera) {
            playerCamera = camera;
            transform.position.Set((transform.position.x - playerCamera.x) * radius + playerCamera.x, transform.position.y, 
                (transform.position.z - playerCamera.z) * radius + playerCamera.z);
            orbit = true;
        }
        
        protected override void Start() {
            base.Start();

        }

        /**
         * This override of update adds in the functionality
         * of the subject node having a orbit and then destroying 
         * itself. Once the orbitTime has been met, calls for a 
         * subject change to the player and then flags that the 
         * node is ready to move itself. Once destroyTime has 
         * been met, this node destroy itself.
         */
        protected override void Update() {
            base.Update();

            // If the node is orbiting the camera.
            if (orbit) {
                float rate = (360 / orbitTime) * Time.deltaTime;

                transform.RotateAround(playerCamera, Vector3.up, rate);
                desiredPosition = (transform.position - playerCamera).normalized * radius + playerCamera;
                transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed);
                           
                timeWaited += Time.deltaTime;
                if (timeWaited >= orbitTime) {                 
                    Services.Camera.SetSubjectToPlayer(transitionToPlayerTime);
                    timeWaited = 0.0f;
                    orbit = false;
                    destroySelf = true;
                }
            }
            // If the node is counting down to destroy itself.
            if (destroySelf) {
                timeWaited += Time.deltaTime;

                if (timeWaited >= destroyTime)
                {
                    Destroy(gameObject);   
                }

            }
        }
    }
}
