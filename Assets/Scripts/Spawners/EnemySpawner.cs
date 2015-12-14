using UnityEngine;
using System.Collections.Generic;

// @author  Ryan
// @date    12 Dec 2015
namespace AdventureFVTC {
    public class EnemySpawner:Spawner {
        [SerializeField] private bool spawnAtNight = false;
        [SerializeField] private bool onlySpawnAtNight = false;
        [SerializeField] private bool spawnOnlyOnce = false;     
        [SerializeField] private List<string> PatrolPoints = new List<string>();
      
        private EnemyController enemyController;
        private bool hasBeenSpawned = false;
        private bool readyToBeSpawned = false;

        private void Spawn() {
            clone = GameObject.Instantiate(objectSpawned);
            clone.GetComponent<EnemyController>().PatrolPointNames = PatrolPoints;
            clone.transform.parent = Services.Run.Game.transform.FindChild("Enemies").transform;
            clone.transform.position = transform.position;
            clone.transform.rotation = transform.rotation;
            hasBeenSpawned = true;
            readyToBeSpawned = false;
        }

        private void SetEnemyController()
        {
            
        }

         public override void Reset() {
            spawnimmediately = true;
            if (clone.gameObject.activeInHierarchy) {
                Destroy(clone.gameObject);
            }
        }

        protected override void Start() {
            base.Start();

        }

        protected override void Respawn() {
            if (spawnimmediately) {
                if (spawnOnlyOnce && !hasBeenSpawned) { // If this spawner can only spawn its enemy once and hasn't spawned it yet.
                    // If this spawner only spawns its enemy at night and it is night time.
                    if (onlySpawnAtNight && Services.Cycle.IsNight) 
                        Spawn();            
                    else {
                        // If this spawner can spawn its enemy at night and at day, spawn whenever ready.
                        if (spawnAtNight) 
                            Spawn();                   
                        // If the spawner doesn't spawn its enemy at night and it's not night time.
                        else if (!spawnAtNight && !Services.Cycle.IsNight) 
                            Spawn();                  
                    }  
                }
                else if (!spawnOnlyOnce) { // If this spawner can spawn its enemy an infinite amount of times.
                    // If this spawner only spawns its enemy at night and it is night time.
                    if (onlySpawnAtNight && Services.Cycle.IsNight) 
                        Spawn();            
                    else if (!onlySpawnAtNight) {
                        // If this spawner can spawn its enemy at night and at day, spawn whenever ready.
                        if (spawnAtNight) 
                            Spawn();                   
                        // If the spawner doesn't spawn its enemy at night and it's not night time.
                        else if (!spawnAtNight && !Services.Cycle.IsNight) 
                            Spawn();                  
                    }  
                }  
                currentRespawnTime = 0.0f; // Reset current respawn time.
                spawnimmediately = false; // Disallow immediately respawning for next death.
            }
            else if (!readyToBeSpawned) {
                currentRespawnTime += Time.deltaTime;

                if (currentRespawnTime >= respawnTime) {
                    readyToBeSpawned = true;
                    currentRespawnTime = 0.0f;
                }
            }
            if (readyToBeSpawned) {
                if (spawnOnlyOnce && !hasBeenSpawned) { // If this spawner can only spawn its enemy once and hasn't spawned it yet.
                    // If this spawner only spawns its enemy at night and it is night time.
                    if (onlySpawnAtNight && Services.Cycle.IsNight) 
                        Spawn();            
                    else {
                        // If this spawner can spawn its enemy at night and at day, spawn whenever ready.
                        if (spawnAtNight) 
                            Spawn();                   
                        // If the spawner doesn't spawn its enemy at night and it's not night time.
                        else if (!spawnAtNight && !Services.Cycle.IsNight) 
                            Spawn();                  
                    }  
                }
                else if (!spawnOnlyOnce) { // If this spawner can spawn its enemy an infinite amount of times.
                    // If this spawner only spawns its enemy at night and it is night time.
                    if (onlySpawnAtNight && Services.Cycle.IsNight) 
                        Spawn();            
                    else if (!onlySpawnAtNight) {
                        // If this spawner can spawn its enemy at night and at day, spawn whenever ready.
                        if (spawnAtNight) 
                            Spawn();                   
                        // If the spawner doesn't spawn its enemy at night and it's not night time.
                        else if (!spawnAtNight && !Services.Cycle.IsNight) 
                            Spawn();                  
                    }  
                }                            
            }
        }
    }
}
