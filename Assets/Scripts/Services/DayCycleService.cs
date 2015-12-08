using UnityEngine;

// @author  Ryan
// @date    08 Dec 2015
namespace AdventureFVTC {
    public class DayCycleService : Service {
        private bool isDay = true;
        private GameObject sun;

        public bool IsDay {
            get {
                return isDay;
            }
            set {
                isDay = value;
            }
        }

        public GameObject Sun {
            get {
                return sun;
            }
            set {
                sun = value;
            }
        }

        public DayCycleService():base() {

        }

        protected override void Start()
        {
            //ToDo: make a prefab of a directional light that has the "Sun" tag.
            //sun = GameObject.FindGameObjectWithTag("Sun");
        }

        protected override void Update() {

        }
    }
}
