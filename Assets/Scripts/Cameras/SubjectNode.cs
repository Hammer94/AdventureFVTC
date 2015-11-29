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
    * @date     28 Nov 2015
    * @see      CameraNode
    * @see      CameraService
    */
    public class SubjectNode : CameraNode {
        private float timeWaited = 0.0f;
        public bool beginLifeTime = false;
        public GameObject playerCamera;

        [SerializeField] private float transitionToPlayerTime = 3.0f; // The time it takes the camera to get to the player.
        [SerializeField] private float lifeTime = 3.0f; // The time the subject node is active and the time it takes 
                                                        // the node to perform a full rotation around the camera.

        protected override void Start() {
            base.Start();

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

            if (beginLifeTime)
            {
                float rate = 360 / lifeTime;

                transform.RotateAround(playerCamera.transform.position, Vector3.up, rate * Time.deltaTime);

                timeWaited += rate * Time.deltaTime;

                if (timeWaited >= lifeTime)
                {
                    Services.Camera.SetSubjectToPlayer(transitionToPlayerTime);
                    Destroy(this.gameObject);
                }
            }                   
        }
    }
}
