using UnityEngine;

namespace AdventureFVTC {
    /**
     * A derivative service for AdventureFVTC. Recieves a gameRoot object, 
     * a player object, a character object, and a camera object on construction. 
     * Instantiates and sets the properties of the player and the camera at start.
     * Allows access to the player and camera publicly.
     *
     * @author  Ryan
     * @date    26 Nov 2015
     * @see     Service
     */
    public class RunService:Service {
        private GameObject gameRoot;
        private Player playerT;
        private Character characterT;
        private Camera cameraT;

        private Player player;
        
        public Player Player {
            get {
                return player;
            }
        }

        public Camera Camera {
            get {
                return cameraT;
            }
        }

        public RunService(GameObject game, Player player, Character character, Camera camera):base() {
            if (game == null)
                throw new System.Exception("Root game object cannot be null.");
            if (player == null)
                throw new System.Exception("Player reference cannot be null.");
            if (character == null)
                throw new System.Exception("Character reference cannot be null.");
            if (camera == null)
                throw new System.Exception("Camera reference cannot be null.");

            gameRoot = game;
            playerT = player;
            characterT = character;
            cameraT = camera;
        }

        protected override void Start() {
            if (player == null) {
                player = Object.Instantiate(playerT);
                player.name = "Player1";
                player.transform.parent = gameRoot.transform.FindChild("Players").transform;
                player.Character = Object.Instantiate(characterT);
                player.Character.name = "Player1Character";
                player.Character.transform.parent = gameRoot.transform.FindChild("Players").transform;
                player.Camera = Object.Instantiate(cameraT);
                player.Camera.name = "Player1Camera";
                player.Camera.transform.parent = gameRoot.transform.FindChild("Players").transform;
                player.Camera.Subject = player.Character.transform.gameObject;
                player.Camera.SubjectFacingDirection = player.Character.transform.FindChild("CharacterFacingDirection").transform.gameObject;
                player.Camera.SubjectBehindDirection = player.Character.transform.FindChild("CharacterBehindDirection").transform.gameObject;
            }
        }
    }
}