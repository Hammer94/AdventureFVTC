using UnityEngine;

namespace AdventureFVTC {
    /**
     * An abstract class that describes what an item is and how it can be
     * used. Includes basic properties of items. Subclasses are responsible
     * for including more specific properties that may vary between items.
     * 
     * @author  Ryan
     * @date    06 Dec 2015
     */
    public abstract class Item {
        public enum ItemTypes
        {
            Level1,
            Level2
        }
          
        [SerializeField] protected string name;
        [SerializeField] protected string desc;
        [SerializeField] protected ItemTypes itemType;

        [SerializeField] private int quantity;

        /**
         * Allows setting the name if one is not already set.
         * Always allows getting the name.
         * 
         * @param   value   The value to set the name to.
         * @return          The name.
         */
        public string Name {
            get {
                return name;
            }
            set {
                if (name == null)
                    name = value;
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
         * Allows setting the itemType.
         * Always allows getting the itemType.
         * 
         * @param   value   The value to set the itemType to.
         * @return          The itemType.
         */
        public ItemTypes ItemType
        {
            get {
                return itemType;
            }
            set {
                itemType = value;
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

        /**
         * Defines any actions needed to consume one of this Item.
         * This method should be implemented by any subclass.
         * 
         * @return  True if the Item should be consumed, false otherwise.
         */
        protected abstract bool consumeProc();

        /**
         * Attempts to consume one of this Item. If the action succeeds
         * the quantity is reduced by one. If the action fails the quantity
         * remains the same. The action may succeed or fail based on subclass
         * implementation.
         * 
         * @return  True if the action succeeds, false otherwise.
         * @see     #consumeProc()
         */
        public bool consume() {
            if (quantity <= 0 || !consumeProc())
                return false;
            quantity--;
            return true;
        }
    }
}
