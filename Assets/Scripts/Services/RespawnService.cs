using UnityEngine;

// @author  Ryan
// @date    08 Dec 2015
namespace AdventureFVTC {
    public class RespawnService : Service {
        private Spawner[] spawners;

        public Spawner[] Spawners {
            get {
                return spawners;
            }
        }

        public RespawnService():base() {

        }

        protected override void Start() {
            // Get all the spawners in the level.
            spawners = GameObject.FindObjectsOfType<Spawner>();
        }

        public void ResetAllSpawners() {
            foreach (Spawner s in spawners)
            {
                s.Reset(); // Reset all the spawns to their starting positions and start the game anew.
            }
        }

        protected override void Update() {

        }
    }
}
