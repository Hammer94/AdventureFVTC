using UnityEngine;

namespace AdventureFVTC {
    public class TriggerNode : CameraNode {              
        [SerializeField] private bool triggerOnEnter = false;
        [SerializeField] private bool triggerOnExit = false;
        [SerializeField] private bool isATransitionTrigger = false;
        [SerializeField] private bool isAChangeSubjectTrigger = false;  
        [SerializeField] private bool transitionOnEnter = false;
        [SerializeField] private bool subjectChangeOnEnter = false;   
        [SerializeField] private bool transitionOnExit = false;
        [SerializeField] private bool subjectChangeOnExit = false;
        [SerializeField] private Vector3 relativePosition;
        [SerializeField] private bool returnTo = false;
        
        /**
          * Runs when the player enters the trigger while the triggerOnEnter is true;
          * 
          * @param   obj         The object that triggers the method. 
          */
        void OnTriggerEnter(Collider obj) {
            if (obj.tag == "Player" && triggerOnEnter) { 
                if (isATransitionTrigger) {
                    if (transitionOnExit) {

                    }
                }
                if (isAChangeSubjectTrigger) {
                    if (subjectChangeOnExit) {

                    }
                }       
            }
        }

        /**
         * Runs when the player leaves the trigger while triggerOnExit is true.
         * 
         * @param   obj         The object that triggers the method. 
         */
        void OnTriggerExit(Collider obj) {
            if (obj.tag == "Player" && triggerOnExit) {
                if (isATransitionTrigger) {
                    if (transitionOnEnter) {

                    }
                }
                if (isAChangeSubjectTrigger) {
                    if (subjectChangeOnEnter) {

                    }
                }       
            }
        }
        
        protected override void Start() {
            base.Start();

            
        }

        protected override void Update() {
            base.Update();

            
        }
    }
}
