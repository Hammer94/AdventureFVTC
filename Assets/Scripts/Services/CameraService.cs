using UnityEngine;

namespace AdventureFVTC
{
    public class CameraService:Service {
        public CameraService():base() {

        }

        // ToDo: add a parameter that signals what transition to call.
        public void StartTransition(Vector3 position, float time, bool returnToSubject, string transitionType) {
            if (transitionType == "TransitionWithSubject")
                Services.Run.Camera.TransitionWithSubject(position, time, returnToSubject);
            else if (transitionType == "TransitionToPoint")
                Services.Run.Camera.TransitionToPoint(position, time, returnToSubject);
        }

        public void StartSubjectChance(string objectTag)
        {
            GameObject subject = GameObject.FindGameObjectWithTag(objectTag);
            GameObject subjectFacingDirection = subject.transform.FindChild("CharacterFacingDirection").transform.gameObject;
        }
    }
}
