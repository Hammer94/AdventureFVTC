using UnityEngine;
using System.Collections.Generic;

namespace AdventureFVTC {
    /**
     * A Unit that operates like a player, able to collect and use items.
     * 
     * @author  Ryan
     * @date    16 Nov 2015
     * @see     Item
     */
    public class Character:Unit {
        private List<Item> list;

        /**
         * Constructs a new Character. Intitializes the list of Item
         * for storing item information.
         * 
         * @see Item
         */
        Character() {
            list = new List<Item>();
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
            Pickup item = collider.GetComponentInParent<Pickup>();
            if (item != null) {
                list.Add(item.Item);
                Destroy(item.gameObject);
            }
        }
    }
}