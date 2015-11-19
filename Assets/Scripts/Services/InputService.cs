using UnityEngine;

namespace AdventureFVTC {
    public class InputService:Service {

        public InputService():base() {

        }

        protected override void Update() {
            //float hInput = Input.GetAxis("Horizontal");
            //float vInput = Input.GetAxis("Vertical");

            //Services.Run.Player.inputHorizontal(hInput);
            //Services.Run.Player.inputVertical(vInput);

            //float ypos = (Input.mousePosition.y - Screen.height / 2);
            //float xpos = (Input.mousePosition.x - Screen.width / 2);
            //float rot = Mathf.Atan(ypos / xpos);
            //if (xpos < 0)
            //    rot = rot + Mathf.PI;

            //Services.Run.Player.inputRotation(rot);
        }
    }
}
