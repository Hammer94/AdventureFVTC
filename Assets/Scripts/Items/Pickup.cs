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
    }
}