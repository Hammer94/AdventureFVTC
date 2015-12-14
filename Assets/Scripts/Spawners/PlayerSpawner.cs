using UnityEngine;

// @author  Ryan
// @date    11 Dec 2015
namespace AdventureFVTC {
    public class PlayerSpawner:Spawner {
        [SerializeField] private GameObject deathMarker;

        private GameObject markerClone;
        private bool viewingPanoramicView = true;
        private bool performingRespawn = true;
        private bool hasSetUp = false;
        private bool destroyMarker = false;

        private float destroyMarkerTime = 1.0f;
        private float currentMakerTime = 0.0f;

        private void DyingSetup() {
            if (!hasSetUp) {
                hasSetUp = true;             
                markerClone = Object.Instantiate(deathMarker);
                markerClone.name = "DeathMarker";
                markerClone.transform.parent = Services.Run.Game.transform.Find("Players").transform;
                markerClone.transform.position = Services.Run.Player.Camera.SubjectBehindDirection.transform.position;
                Services.Run.Player.Camera.Subject = markerClone;
                Services.Run.Player.Camera.SubjectBehindDirection = markerClone;
            }
        }

        private void DestroyMarker() {
            currentMakerTime += Time.deltaTime;
            if (currentMakerTime >= destroyMarkerTime) {
                destroyMarker = false;
                hasSetUp = false;
                currentMakerTime = 0.0f;              
                Destroy(markerClone.gameObject);
            }
        }

        // This override ensures that the player spawner will never reset itself, as that functionality is already
        // present in the camera and PersistentPlayerStats.
        public override void Reset() {
            reset = true; // Set first spawn to true so their reset doesnt cost them a life.
        }

        protected override void Start() {
            reset = true;
            objectSpawned = null; // Player spawner is spawning from the run service, its doesn't need an object.
        }

        protected override void Update() {
            if (destroyMarker)
                DestroyMarker();
            
            if (Services.Run.Player.Character == null || Services.Run.Player.Character.Dead) {
                if (!viewingPanoramicView)
                    DyingSetup();
                Respawn();
            }
                            
        }

        protected override void Respawn() {
            if (PersistentPlayerStats.LivesLeft > 0) {
                if (reset) // If this is the first time this player has spawned this level attempt.           
                    reset = false; // Don't subtract any of their lives.             
                else // Else the player has died after being spawned in the beginning.             
                    PersistentPlayerStats.LivesLeft -= 1; // Take away from their remaining lives.

                Services.Run.Player.Camera.IsSubjectChangeStillTransitioning = true;
                
                if (Services.Run.Player.Character != null) {                   
                    Services.Run.Player.Character.Dead = false;
                    Services.Run.Player.Character.MaxHealth = 3;
                    Services.Run.Player.Character.Health = Services.Run.Player.Character.MaxHealth;
                    SkinnedMeshRenderer rabbitRenderer = GameObject.FindGameObjectWithTag("RabbitRenderer").GetComponent<SkinnedMeshRenderer>();
                    rabbitRenderer.material.color = Services.Run.Player.Character.InitialCharacterColor;                
                }
                else {
                    Services.Run.Player.Character = Object.Instantiate(Services.Run.CharacterRoot);
                    Services.Run.Player.Character.name = "Player1Character";
                    Services.Run.Player.Character.transform.parent = Services.Run.Game.transform.Find("Players").transform;
                }

                Services.Run.Player.Character.transform.position = transform.position; // Move the player to its spawner.            

                if (viewingPanoramicView)
                    viewingPanoramicView = false;
                else {
                    // Transition the subject using a time based on distance.
                    float distance = Vector3.Distance(Services.Run.Player.Character.transform.position, Services.Run.Player.Camera.transform.position);
                    float time = distance / 24.667f;
                    if (time < 4)
                        time = 4f;

                    Services.Camera.SetSubjectToPlayer(time); // Move the camera to the player.                  
                    destroyMarker = true; // Destroy the marker where the player died.                
                }                        
            }
            else if (PersistentPlayerStats.LivesLeft == 0) {
                Services.Respawn.ResetAllSpawners(); // Reset all the spawners, removing their object from the gamespace and then putting them back in.
                PersistentPlayerStats.GameOver(); // Reset this players stats and giving them back their lives, allowing them to respawn.
            }           
        }
    }
}
