using UnityEngine;

namespace AdventureFVTC {
    /**
     * A derivative service for AdventureFVTC. Allows the transitioning
     * of the camera from location to location, and rotation to rotation.
     * Allows a transition type to be defined, telling this service which
     * transition type to perform.
     * Allows the camera's subject to change and the subject's
     * facing and behind directions to be reassigned.
     * Makes use of Services to access RunService.
     * Uses RunService to access the camera.
     *
     * @author  Ryan
     * @date    30 Nov 2015
     * @see     Service
     * @see     Services
     * @see     RunService
     */
    public class CameraService:Service {
        public CameraService():base() {

        }

        /**
         * Changes the camera's subject to the player and then transitions to the player over time seconds.
         */
        public void SetSubjectToPlayer(float time)
        {
            StartSubjectChange("Player");
            GameObject PlayerSpawnerCameraPosition = GameObject.FindGameObjectWithTag("PlayerSpawnerCameraPosition").gameObject;

            StartTransition(PlayerSpawnerCameraPosition.transform.position, time, true, "TransitionToPoint");
        }

        /** 
        * Recieves a position, a time, a boolean, and a transitiontype to perform a transition.
        * Performs one of two transitions based on what string it has been given.
        *
        * @param   position         The position the camera needs to move to.
        * @param   time             The amount of time it should take for the camera to transition. 
        * @param   returnTo         The flag if the camera is returning to its default position.
        * @param   transitionType   The type of transition the camera needs to perform.          
        */
        public void StartTransition(Vector3 position, float time, bool returnToSubject, string transitionType) {
            if (transitionType == "TransitionWithSubject")
                Services.Run.Player.Camera.TransitionWithSubject(position, time, returnToSubject);
            else if (transitionType == "TransitionToPoint")
                Services.Run.Player.Camera.TransitionToPoint(position, time, returnToSubject);
        }

        /** 
        * Tries to find the object with the specified tag, gets its facing direction 
        * children if it has them, and then sends out each of the objects to the camera
        * so it can perform a subject change.
        *
        * @param   objectTag        The object that the camera needs to change its subject to.        
        */
        public void StartSubjectChange(string objectTag) {
            GameObject subject = GameObject.FindWithTag(objectTag);
            //GameObject subjectBehindDirection = subject.transform.Find("CharacterBehindDirection").gameObject;
            GameObject subjectBehindDirection = GameObject.FindGameObjectWithTag("CharacterBehindDirection").gameObject;

            Services.Run.Player.Camera.ChangeSubject(subject, subjectBehindDirection);
        }
    }
}
