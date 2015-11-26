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
     * @date    26 Nov 2015
     * @see     Service
     * @see     Services
     * @see     RunService
     */
    public class CameraService:Service {
        public CameraService():base() {

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
                Services.Run.Camera.TransitionWithSubject(position, time, returnToSubject);
            else if (transitionType == "TransitionToPoint")
                Services.Run.Camera.TransitionToPoint(position, time, returnToSubject);
        }

        /** 
        * Tries to find the object with the specified tag, gets its facing direction 
        * children if it has them, and then sends out each of the objects to the camera
        * so it can perform a subject change.
        *
        * @param   objectTag        The object that the camera needs to change its subject to.        
        */
        public void StartSubjectChance(string objectTag) {
            GameObject subject = GameObject.FindGameObjectWithTag(objectTag);
            GameObject subjectFacingDirection = subject.transform.FindChild("CharacterFacingDirection").transform.gameObject;
            GameObject subjectBehindDirection = subject.transform.FindChild("CharacterBehindDirection").transform.gameObject;

            Services.Run.Camera.ChangeSubject(subject, subjectFacingDirection, subjectBehindDirection);
        }
    }
}
