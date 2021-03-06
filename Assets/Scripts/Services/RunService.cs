﻿using UnityEngine;

namespace AdventureFVTC {
    /**
     * A derivative service for AdventureFVTC. Recieves a gameRoot object, 
     * a player object, a character object, and a camera object on construction. 
     * Instantiates and sets the properties of the player and the camera at start.
     * Allows access to the player and camera publicly.
     *
     * @author  Ryan
     * @date    14 Dec 2015
     * @see     Service
     */
    public class RunService:Service {
        private GameObject gameRoot;
        private Player playerT;
        private Character characterT;
        private CameraBase cameraT;

        private Player player;
        private PlayerSpawner playerSpawn;
        
        public GameObject Game
        {
            get {
                return gameRoot;
            }
        }

        public Player Player {
            get {
                return player;
            }
        }

        public Character CharacterRoot {
            get {
                return characterT;
            }
        }

        public CameraBase Camera {
            get {
                return cameraT;
            }
        }

        public RunService(GameObject game, Player player, Character character, CameraBase camera):base() {
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
                player.transform.parent = gameRoot.transform.Find("Players").transform;

                playerSpawn = GameObject.FindObjectOfType<PlayerSpawner>();

                //player.Character = Object.Instantiate(characterT);
                //player.Character.name = "Player1Character";         
                //player.Character.transform.parent = gameRoot.transform.Find("Players").transform;
                //GameObject playerSpawner = gameRoot.transform.Find("Spawners").transform.Find("PlayerSpawner").transform.gameObject;
                //player.Character.transform.position = playerSpawner.transform.position;

                player.Camera = Object.Instantiate(cameraT);
                player.Camera.name = "Player1Camera";
                player.Camera.transform.parent = gameRoot.transform.Find("Players").transform;
                GameObject cameraSpawner = gameRoot.transform.Find("Spawners").transform.Find("CameraSpawner").transform.gameObject;
                player.Camera.SubjectBehindDirection = cameraSpawner;
                player.Camera.transform.position = cameraSpawner.transform.position;
                SubjectNode subjectNode = SubjectNode.FindObjectOfType<SubjectNode>();
                if (subjectNode != null)
                {
                    player.Camera.Subject = subjectNode.transform.gameObject;
                    subjectNode.InitialSetUp(player.Camera.transform.position);
                }
                else
                    playerSpawn.ViewingPanoramicView = false;
            }
        }
    }
}