using UnityEngine;
using System.Collections.Generic;

// @author  Ryan
// @date    10 Dec 2015
namespace AdventureFVTC {
    public class EnemySpawner:Spawner {
        [SerializeField] private bool spawnAtNight = false;
        [SerializeField] private bool onlySpawnAtNight = false;
        [SerializeField] private bool spawnOnlyOnce = false;     
        public List<string> PatrolPoints = new List<string>();

        private EnemyController enemyController;
        private bool hasBeenSpawned = false;
        private bool readyToBeSpawned = false;

        private void Spawn() {
            GameObject clone = GameObject.Instantiate(objectSpawned);
            clone.GetComponent<EnemyController>().PatrolPointNames = PatrolPoints;
            clone.transform.parent = Services.Run.Game.transform.FindChild("Enemies").transform;
            clone.transform.position = transform.position;
            hasBeenSpawned = true;
            readyToBeSpawned = false;
        }

        protected override void Start() {
            base.Start();

            // Get this spawners enemyController.
            enemyController = objectSpawned.GetComponent<EnemyController>();
            // Set the patrol points.
            foreach (string name in PatrolPoints)
            {
                GameObject point = GameObject.Find(name);
                if (point != null)
                {
                    Transform t = point.GetComponent<Transform>();
                    enemyController.Patrol.PatrolPoints.Add(t);
                }
            }
        }

        protected override void Respawn() {
            if (reset) {
                GameObject clone = GameObject.Instantiate(objectSpawned);
                clone.GetComponent<EnemyController>().PatrolPointNames = PatrolPoints;
                clone.transform.parent = Services.Run.Game.transform.FindChild("Enemies").transform;
                clone.transform.position = transform.position;
                currentRespawnTime = 0.0f;
                hasBeenSpawned = true;
                reset = false;
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
                    else {
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
