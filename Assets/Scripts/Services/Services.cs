using UnityEngine;

namespace AdventureFVTC {
    public class Services : MonoBehaviour {
        private static Services self;
        private static RunService run;
        private static InputService input;

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
	    }

	    void Update() {
            Run.update();
            Input.update();
	    }

        void FixedUpdate() {
            Run.fixedUpdate();
            Input.fixedUpdate();
        }
    }
}
