using UnityEngine;

// @author  Ryan
// @date    08 Dec 2015
namespace AdventureFVTC {
    public class DayCycleService : Service {
        private GameObject sun;
        private bool isNight = false;
        private float amountOfDayTime = 60.0f;
        private float amountOfNightTime = 20.0f;
        private float currentMoveTime = 0.0f;
        private bool startCycle = false;
        private float currentXEuler;

        public bool IsNight {
            get {
                return isNight;
            }
            set {
                isNight = value;
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

        public void StartCycle() {
            startCycle = true;
            currentXEuler = sun.transform.eulerAngles.x;
        }

        public DayCycleService(float dayTime, float nightTime):base() {
            if (dayTime != 0.0f)
                amountOfDayTime = dayTime;
            if (nightTime != 0.0f)
                amountOfNightTime = nightTime;
        }

        private void Cycle() {                
            if (isNight) {
                currentMoveTime += Time.deltaTime;
                if (currentMoveTime > amountOfNightTime)
                    currentMoveTime = amountOfNightTime;
                // lerp!
                float perc = currentMoveTime / amountOfNightTime;
                float change  = Mathf.Lerp(currentXEuler, 360, perc);

                sun.transform.eulerAngles = new Vector3(change, 90, 0);

                if (currentMoveTime >= amountOfNightTime) {
                    isNight = false;
                    currentMoveTime = 0.0f;
                    currentXEuler = 0;
                }
            }
            else {
                currentMoveTime += Time.deltaTime;
                if (currentMoveTime > amountOfDayTime)
                    currentMoveTime = amountOfDayTime;
                // lerp!
                float perc = currentMoveTime / amountOfDayTime;
                float change = Mathf.Lerp(currentXEuler, 180, perc);

                sun.transform.eulerAngles = new Vector3(change, 90, 0);

                if (currentMoveTime >= amountOfDayTime) {
                    isNight = true;
                    currentMoveTime = 0.0f;
                    currentXEuler = 180;
                }                
            }
        }

        protected override void Start() {
            sun = GameObject.FindGameObjectWithTag("Sun");
        }

        protected override void Update() {
            if (startCycle) {
                Cycle();
            }
        }
    }
}
