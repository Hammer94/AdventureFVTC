using UnityEngine;

namespace AdventureFVTC {
    /**
     * An item in the world that can be collected. Contains
     * item Item that gives information about how this item
     * can be operated once collected.
     * 
     * @author  Ryan
     * @date    16 Nov 2015
     * @see     Item
     */
    public class Pickup : MonoBehaviour {
        private Item item;
        private float destroyDelay = 1.0f;
        private float delayedTime = 0.0f;
        private bool destroySelf = false;
        private bool playedAudio = false;

        /**
         * Allows setting the Item if one does not exist.
         * Always allows getting the Item.
         *
         * @param   value       The value to set the Item to.
         * @return              The Item.
         * @see     Item
         */
        public Item Item {
            get {
                return item;
            }
            set {
                if (item == null)
                    item = value;
            }
        }

        public bool DestroySelf {
            get {
                return destroySelf;
            }
            set {
                DestroySelf = value;
            }
        }
        
         /**
         * Called when the Character intersects a trigger. If the trigger
         * is a Pickup it collects its Item.
         * 
         * @param   collider    The collider that was entered.
         * @see     Pickup
         * @see     Item
         */
        void OnTriggerEnter(Collider collider) {
           
            if (collider.tag == "Player" && !destroySelf) {
                destroySelf = true;

                GameObject particles = transform.FindChild("particles").gameObject;              
                if (particles != null)
                    Destroy(particles);
                Destroy(item.gameObject);
             
                if (item.ItemName == "Level1") 
                    PersistentPlayerStats.AddToCurrentLevel1(item.Quantity); // Add to Level1 item amount.             
                else if (item.ItemName == "Level2") 
                    PersistentPlayerStats.AddToCurrentLevel2(item.Quantity); // Add to Level2 item amount.                  
                else if (item.ItemName == "Coin") 
                    PersistentPlayerStats.AddToCurrentScore(item.Quantity); // Add to coin item amount.                 
                else if (item.ItemName == "Snowball") 
                    PersistentPlayerStats.GotSnowBall(); // Let the player throw the snowball.           
            }
        }

        void Start() {
            item = GetComponentInChildren<Item>();
        }
        
        void Update() {
            if (destroySelf) {
                if (!playedAudio) {
                    playedAudio = true;
                    GetComponent<AudioSource>().Play();
                }

                delayedTime += Time.deltaTime;
                if (delayedTime >= destroyDelay)
                    Destroy(gameObject);
            }
        }       
    }
}