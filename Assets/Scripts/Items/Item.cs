using UnityEngine;

namespace AdventureFVTC {
    /**
     * An class that describes what an item is and how it can be
     * used. Includes basic properties of items. Subclasses are responsible
     * for including more specific properties that may vary between items.
     * 
     * @author  Ryan
     * @date    12 Dec 2015
     */
    public class Item:MonoBehaviour {
         
        [SerializeField] protected string itemName;
        [SerializeField] protected string desc;
        [SerializeField] private int quantity;

        /**
         * Allows setting the name if one is not already set.
         * Always allows getting the name.
         * 
         * @param   value   The value to set the name to.
         * @return          The name.
         */
        public string ItemName {
            get {
                return itemName;
            }
            set {
                if (itemName == null)
                    itemName = value;
            }
        }

        /**
         * Allows setting the description if one is not already set.
         * Always allows getting the description.
         * 
         * @param   value   The value to set the description to.
         * @return          The description.
         */
        public string Description {
            get {
                return desc;
            }
            set {
                if (desc == null)
                    desc = value;
            }
        }

        /**
         * Allows setting the quantity to any number greater than or equal to 0.
         * Sets the quantity to 0 if it were less than 0.
         * Always allows getting the quantity.
         * 
         * @param   value   The value to set the quantity to.
         * @return          The quantity.
         */
        public int Quantity {
            get {
                return quantity;
            }
            set {
                quantity = value;
                if (quantity <= 0)
                    quantity = 0;
            }
        }

        /**
         * Constructs a new Item.
         */
        public Item() {

        }

        /**
         * Returns a bool signifying if the item is usable.
         * 
         * @return  True if usable, false otherwise.
         */
        public virtual bool Usable {
            get {
                return (quantity > 0);
            }
        }

        void Update() {
            gameObject.transform.Rotate(0, 5, 0, Space.Self); // Spins the item.
        }
    }
}
