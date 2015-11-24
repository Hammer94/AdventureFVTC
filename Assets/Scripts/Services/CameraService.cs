using UnityEngine;

namespace AdventureFVTC
{
    public class CameraService:Service {
        public CameraService():base() {

        }

        // ToDo: add a string parameter that signals what transition to call.
        public void StartTransition(Vector3 position, float time, bool returnToSubject) {
            Services.Run.Camera.TransitionWithSubject(position, time, returnToSubject);
        }
    }
}
