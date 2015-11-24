using UnityEngine;

namespace AdventureFVTC
{
    public class CameraService:Service {
        public CameraService():base() {

        }

        protected override void Update() {
            if (Services.Run.Camera.transform.position == Services.Run.Camera.DefaultCameraPosition)
            {
                Services.Run.Camera.AtDefaultPosition = true;
                Services.Run.Camera.RotateWithSubject(Services.Run.Player.transform.rotation.eulerAngles);
            } 
            else
            {
                
            }               
        }
    }
}
