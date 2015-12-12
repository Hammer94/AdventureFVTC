using UnityEngine;

// @author  Ryan
// @date    12 Dec 2015
namespace AdventureFVTC {
    public class Spawner:MonoBehaviour {
        [SerializeField] private bool spawnImediately = true;       
        [SerializeField] protected float respawnTime = 5.0f;       
        [SerializeField] protected GameObject objectSpawned;
        protected bool startSpawning = false;

        protected float currentRespawnTime = 0.0f;
        protected bool reset;
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
            reset = true;
            if (objectSpawned.gameObject != null) {
                Destroy(clone.gameObject);
            }               
        }
  
        protected virtual void Respawn() {
            if (reset) {
                clone = GameObject.Instantiate(objectSpawned);
                clone.transform.parent = transform;
                clone.transform.position = transform.position;
                currentRespawnTime = 0.0f;
                reset = false;
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
            reset = spawnImediately;
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
