using UnityEngine;

// @author  Ryan
// @date    10 Dec 2015
namespace AdventureFVTC {
    public class Spawner:MonoBehaviour {
        [SerializeField] private bool spawnImediately = true;       
        [SerializeField] protected float respawnTime = 5.0f;
        [SerializeField] protected float currentRespawnTime = 0.0f;
        [SerializeField] protected GameObject objectSpawned;

        protected bool reset = true;

        protected virtual void Reset() {
            reset = true;
            Destroy(objectSpawned.gameObject);          
        }
  
        protected virtual void Respawn() {
            if (reset) {
                GameObject clone = GameObject.Instantiate(objectSpawned);
                clone.transform.parent = transform;
                clone.transform.position = transform.position;
                currentRespawnTime = 0.0f;
                reset = false;
            }
            else {
                currentRespawnTime += Time.deltaTime;

                if (currentRespawnTime >= respawnTime) {
                    //clone.transform.parent = Services.Run.Game.transform.FindChild("Players").transform;
                    GameObject clone = GameObject.Instantiate(objectSpawned);                 
                    clone.transform.parent = transform;
                    clone.transform.position = transform.position;
                    currentRespawnTime = 0.0f;
                }
            }      
        }

        protected virtual void Start() {
            reset = spawnImediately;
        }

        protected virtual void Update() {
            if (objectSpawned.gameObject == null)           
                Respawn();           
        }
    }
}
