using UnityEngine;

namespace AdventureFVTC {
    /**
     * A derivative camera node for AdventureFVTC. Specifies conditions 
     * on how the trigger node should act.
     * Has triggers to check when the player enters or leaves the node.
     * Allows setting whether the trigger node should perform transitions 
     * and subject changes, as well as whether to do those on entering or 
     * leaving the trigger.
     * Allows the node to specifiy which one of the two transition types it 
     * can perform.
     *
     * @author  Ryan
     * @date    28 Nov 2015
     * @see     CameraNode
     */
    public class TriggerNode:CameraNode {
        private enum TransitionType
        {
            TransitionWithSubject,
            TransitionToPoint
        }
             
        [SerializeField] private Vector3 newPosition; // The position the camera should move to.
        [SerializeField] private float time; // The amount of time it should take for the camera to move there.
        [SerializeField] private bool returningOnEnter = false; // Should the camera return to its default position on enter?
        [SerializeField] private bool returningOnExit = false; // Should the camera return to its default position on exit?

        [SerializeField] private string objectTagToLookFor; // What objectTag the services will look for when doing a subject change.
        [SerializeField] private TransitionType transitionEnterType; // What type of transition does this trigger use OnEnter?
        [SerializeField] private TransitionType transitionExitType; // What type of transition does this trigger use OnExit?

        // The conditions that define how the trigger should be triggered.
        [SerializeField]
        private bool triggerOnEnter = false;
        [SerializeField]
        private bool triggerOnExit = false;
        [SerializeField]
        private bool isATransitionTrigger = false;
        [SerializeField]
        private bool isAChangeSubjectTrigger = false;
        [SerializeField]
        private bool transitionOnEnter = false;
        [SerializeField]
        private bool subjectChangeOnEnter = false;
        [SerializeField]
        private bool transitionOnExit = false;
        [SerializeField]
        private bool subjectChangeOnExit = false;

        /**
         * Runs when the player enters the trigger and the trigger is a OnEnterTrigger.
         * If this trigger is a subjectchange and subjectChangeOnEnter trigger, perform a subject change.
         * If this trigger is a transition and transitionOnEnter trigger, go to the next step.
         * If this trigger is a returningOnEnter trigger, transition to the default position. Else, transition
         * to a new position.
         * 
         * @param   obj         The object that triggers the method. 
         */
        void OnTriggerEnter(Collider obj) {
            if (obj.tag == "Player" && triggerOnEnter) {
                if (isAChangeSubjectTrigger) {
                    if (subjectChangeOnEnter) {
                        Services.Camera.StartSubjectChange(objectTagToLookFor); // Change the subject.
                    }
                }
                if (isATransitionTrigger) {
                    if (transitionOnEnter) {
                        // Is the camera returning to its default position on enter?
                        if (returningOnEnter)
                            // Set the position as the camera's default position and send returning as true.
                            Services.Camera.StartTransition(Services.Run.Camera.DefaultCameraPosition, time, true, transitionEnterType.ToString());
                        // Else the camera is moving to a new position.
                        else
                            // Set the position as the new position and send returning as false.
                            Services.Camera.StartTransition(newPosition, time, false, transitionEnterType.ToString());
                    }
                }              
            }
        }

        /**
         * Runs when the player enters the trigger and the trigger is a OnExitTrigger.
         * If this trigger is a subjectchange and subjectChangeOnExit trigger, perform a subject change.
         * If this trigger is a transition and transitionOnExit trigger, go to the next step.
         * If this trigger is a returningOnExit trigger, transition to the default position. Else, transition
         * to a new position.
         * 
         * @param   obj         The object that triggers the method. 
         */
        void OnTriggerExit(Collider obj) {
            if (obj.tag == "Player" && triggerOnExit) {
                if (isAChangeSubjectTrigger) {
                    if (subjectChangeOnExit) {
                        Services.Camera.StartSubjectChange(objectTagToLookFor); // Change the subject.
                    }
                }
                if (isATransitionTrigger) {
                    if (transitionOnExit) {
                        // Is the camera returning to its default position on exit?
                        if (returningOnExit)
                            // Set the position as the camera's default position and send returning as true.
                            Services.Camera.StartTransition(Services.Run.Camera.DefaultCameraPosition, time, true, transitionExitType.ToString());
                        // Else the camera is moving to a new position.
                        else
                            // Set the position as the new position and send returning as false.
                            Services.Camera.StartTransition(newPosition, time, false, transitionExitType.ToString());
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
