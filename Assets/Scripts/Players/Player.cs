using UnityEngine;

namespace AdventureFVTC {
    /**
     * A player in the game that controls a character and camera.
     * Recieves input that is used to control the character.
     * 
     * @author  Ryan
     * @date    12 Dec 2015
     */
    public class Player:MonoBehaviour {
        [SerializeField] private Character character;
        [SerializeField] private CameraBase playerCamera;

        //private Vector2 inputMove;
        //private float inputAngle;

        /**
         * Allows a Character to be set once and retrieved
         * many times.
         * 
         * @param   value   The Character to set.
         * @return          The Character.
         */
        public Character Character {
            get {
                return character;
            }
            set {
                //if (character == null)
                //    character = value;
                character = value;
            }
        }

        /**
         * Allows a Camera to be set once and retrieved
         * many times.
         * 
         * @param   value   The Camera to set.
         * @return          The Camera.
         */
        public CameraBase Camera {
            get {
                return playerCamera;
            }
            set {
                if (playerCamera == null)
                    playerCamera = value;
            }
        }

        /**
         * Allows the player to recieve input to
         * move in the vertical direction.
         * 
         * @param   d   The vector distance to move.
         */
        //public void inputVertical(float d) {
        //    inputMove.y = d;
        //}

        /**
         * Allows the player to recieve input to
         * move in the horizontal direction.
         * 
         * @param   d   The vector distance to move.
         */
        //public void inputHorizontal(float d) {
        //    inputMove.x = d;
        //}

        /**
         * Allows the player to recieve input to
         * rotate. Accepts the input using right hand rule.
         * The direciton 0 is facing down the z axis.
         * 
         * @param   r   The angle to rotate in radians.
         */
        //public void inputRotation(float r) {
        //    inputAngle = r;
        //}

        /**
         * Called when the script begins running. Stops the script
         * if it does not have access to its character or camera.
         */
        void Start() {
            if (character == null || playerCamera == null) {
                enabled = false;
            }
            // TODO: set character properties.
        }

        /**
         * Called every update frame. Handles processing input information
         * and passing it to the character.
         * 
         * This behavior is temporary and will change later.
         */
        void Update() {
            //character.moveDirection(inputMove);
            //character.desiredRotation = inputAngle;
        }
    }
}