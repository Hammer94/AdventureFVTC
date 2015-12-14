using UnityEngine;

// @author  Ryan
// @date    13 Dec 2015
namespace AdventureFVTC {
    public class ItemSpawner:Spawner {
        [SerializeField] private bool spawnAtNight = false;
        [SerializeField] private bool onlySpawnAtNight = false;
        [SerializeField] private bool spawnOnlyOnce = false;

        private bool hasBeenSpawned = false;
        private bool readyToBeSpawned = false;

         private void Spawn() {
            clone = GameObject.Instantiate(objectSpawned);
            clone.transform.parent = Services.Run.Game.transform.FindChild("Items").transform;
            clone.transform.position = transform.position;
            hasBeenSpawned = true;
            readyToBeSpawned = false;
        }

        private void CheckSpawnConditions() {
            Debug.Log(Services.Cycle.IsNight);
            if (spawnOnlyOnce && !hasBeenSpawned) { // If this spawner can only spawn its item once and hasn't spawned it yet.
                // If this spawner only spawns its item at night and it is night time.
                if (onlySpawnAtNight && Services.Cycle.IsNight)
                    Spawn();
                // If this spawner can spawn its item at night and at day, spawn whenever ready.    
                else if (spawnAtNight) 
                    Spawn();
                // If the spawner can't spawn its item at night and it's not night time.
                else if (!spawnAtNight && !Services.Cycle.IsNight)
                    Spawn();
            }
            else if (!spawnOnlyOnce) { // If this spawner can spawn its item an infinite amount of times.
                // If this spawner only spawns its item at night and it is night time.
                if (onlySpawnAtNight && Services.Cycle.IsNight)
                    Spawn();
                // If this spawner can spawn its item at night and at day, spawn whenever ready.
                else if (spawnAtNight)                  
                    Spawn();
                // If the spawner cant't spawn its item at night and it's not night time.
                else if (!spawnAtNight && !Services.Cycle.IsNight)
                    Spawn();
            }  
        }

        public override void Reset() {
            spawnimmediately = true;
            hasBeenSpawned = false;
            if (clone.gameObject.activeInHierarchy) {
                Destroy(clone.gameObject);
            }
        }

        protected override void Respawn() {
            if (spawnimmediately) {

                CheckSpawnConditions(); // Check when this enemy can spawn.

                currentRespawnTime = 0.0f; // Reset current respawn time.
                spawnimmediately = false; // Disallow immediately respawning for next death.
            }
            else if (!spawnimmediately) {
                if (!readyToBeSpawned) {
                    currentRespawnTime += Time.deltaTime;

                    if (currentRespawnTime >= respawnTime) {
                        readyToBeSpawned = true;
                        currentRespawnTime = 0.0f;
                    }
                }
                else if (readyToBeSpawned) {
                    CheckSpawnConditions(); // Check when this enemy can spawn.                                        
                }
            } 
        }
    }
}
