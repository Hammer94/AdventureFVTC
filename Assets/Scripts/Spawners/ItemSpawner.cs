﻿using UnityEngine;

// @author  Ryan
// @date    12 Dec 2015
namespace AdventureFVTC {
    public class ItemSpawner:Spawner {
        [SerializeField] private bool spawnAtNight = false;
        [SerializeField] private bool onlySpawnAtNight = false;
        [SerializeField] private bool spawnOnlyOnce = false;

        private bool hasBeenSpawned = false;
        private bool readyToBeSpawned = false;

         private void Spawn() {
            GameObject clone = GameObject.Instantiate(objectSpawned);
            clone.transform.parent = Services.Run.Game.transform.FindChild("Items").transform;
            clone.transform.position = transform.position;
            hasBeenSpawned = true;
            readyToBeSpawned = false;
        }

        protected override void Respawn() {
            if (reset) {
                GameObject clone = GameObject.Instantiate(objectSpawned);
                clone.transform.parent = Services.Run.Game.transform.FindChild("Items").transform;
                clone.transform.position = transform.position;
                currentRespawnTime = 0.0f;
                reset = false;
                hasBeenSpawned = true;
                readyToBeSpawned = false;
            }
            else if (!readyToBeSpawned) {
                currentRespawnTime += Time.deltaTime;

                if (currentRespawnTime >= respawnTime) {
                    readyToBeSpawned = true;
                    currentRespawnTime = 0.0f;
                }
            }
            if (readyToBeSpawned) {
                if (spawnOnlyOnce && !hasBeenSpawned) { // If this spawner can only spawn its item once and hasn't spawned it yet.
                    // If this spawner only spawns its item at night and it is night time.
                    if (onlySpawnAtNight && Services.Cycle.IsNight) 
                        Spawn();            
                    else {
                        // If this spawner can spawn its item at night and at day, spawn whenever ready.
                        if (spawnAtNight) 
                            Spawn();
                        // If the spawner doesn't spawn its item at night and it's not night time.
                        else if (!spawnAtNight && !Services.Cycle.IsNight) 
                            Spawn();                  
                    }  
                }
                else if (!spawnOnlyOnce) { // If this spawner can spawn its item an infinite amount of times.
                    // If this spawner only spawns its item at night and it is night time.
                    if (onlySpawnAtNight && Services.Cycle.IsNight) 
                        Spawn();            
                    else {
                        // If this spawner can spawn its item at night and at day, spawn whenever ready.
                        if (spawnAtNight) 
                            Spawn();
                        // If the spawner doesn't spawn its item at night and it's not night time.
                        else if (!spawnAtNight && !Services.Cycle.IsNight) 
                            Spawn();                  
                    }  
                }                                        
            }
        }
    }
}
