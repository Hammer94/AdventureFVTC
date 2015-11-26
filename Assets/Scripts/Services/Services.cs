using UnityEngine;

namespace AdventureFVTC {
    /**
     * Holds all of the active services in AdventureFVTC. Allows public
     * access to each of the services it holds.
     * Services can only be set as a new service once.
     * Recieves a playerObject, characterObject, and cameraObject to be
     * given to RunService.
     *
     * @author  Ryan
     * @date    26 Nov 2015
     * @see     RunService
     * @see     InputService
     * @see     CameraService
     */
    public class Services : MonoBehaviour {
        private static Services self;
        private static RunService run;
        private static InputService input;
        private static CameraService cameraS;

        [SerializeField] private Player playerObject;
        [SerializeField] private Character characterObject;
        [SerializeField] private Camera cameraObject;

        public static Services Instance {
            get {
                if (self == null)
                    new Services();
                return self;
            }
        }

        public static RunService Run {
            get {
                return run;
            }
        }

        public static InputService Input {
            get {
                if (input == null)
                    input = new InputService();
                return input;
            }
        }

        public static CameraService Camera {
            get {
                if (cameraS == null)
                    cameraS = new CameraService();
                return cameraS;
            }
        }

        public Services() {
            if (self == null)
                self = this;
            else
                throw new System.Exception("Cannot create mutliple Services.");
        }

	    void Start() {
            run = new RunService(gameObject, playerObject, characterObject, cameraObject);
            Run.start();
            Input.start();
            Camera.start();
	    }

	    void Update() {
            Run.update();
            Input.update();
            Camera.update();
	    }

        void FixedUpdate() {
            Run.fixedUpdate();
            Input.fixedUpdate();
            Camera.fixedUpdate();
        }
    }
}
