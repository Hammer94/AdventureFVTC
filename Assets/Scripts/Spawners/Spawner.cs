using UnityEngine;

// @author  Ryan
// @date    12 Dec 2015
namespace AdventureFVTC {
    public class Spawner:MonoBehaviour {
        [SerializeField] protected bool spawnimmediately = true;       
        [SerializeField] protected float respawnTime = 5.0f;       
        [SerializeField] protected GameObject objectSpawned;
        protected bool startSpawning = false;

        protected float currentRespawnTime = 0.0f;
        protected GameObject clone;

        public bool StartSpawning {
            get {
                return startSpawning;
            }
            set {
                startSpawning = value;
            }
        }

        public virtual void Reset() {
            spawnimmediately = true;
            if (objectSpawned.gameObject != null) {
                Destroy(clone.gameObject);
            }               
        }
  
        protected virtual void Respawn() {
            if (spawnimmediately) {
                clone = GameObject.Instantiate(objectSpawned);
                clone.transform.parent = transform;
                clone.transform.position = transform.position;
                currentRespawnTime = 0.0f;
                spawnimmediately = false;
            }
            else {
                currentRespawnTime += Time.deltaTime;

                if (currentRespawnTime >= respawnTime) {                  
                    clone = GameObject.Instantiate(objectSpawned);                 
                    clone.transform.parent = transform;
                    clone.transform.position = transform.position;
                    currentRespawnTime = 0.0f;
                }
            }      
        }

        protected virtual void Start() {
            clone = objectSpawned;
        }

        protected virtual void Update() {
            if (clone.gameObject == null)
                clone = objectSpawned;
            if (!clone.gameObject.activeInHierarchy)           
                Respawn();                   
        }
    }
}
