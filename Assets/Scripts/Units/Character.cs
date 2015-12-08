using UnityEngine;
using System.Collections.Generic;

namespace AdventureFVTC
{
    /**
     * A Unit that operates like a player, able to collect and use items.
     * No longer relies on a rigidbody for movement.
     * 
     * @author  Ryan
     * @date    08 Dec 2015
     * @see     Item
     */
    public class Character : Unit
    {
        private List<Item> list;

        /**
         * Constructs a new Character. Intitializes the list of Item
         * for storing item information.
         * 
         * @see Item
         */
        Character()
        {
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
        void OnTriggerEnter(Collider collider)
        {
            Pickup item = collider.GetComponentInParent<Pickup>();
            if (item != null)
            {
                list.Add(item.Item);
                Destroy(item.gameObject);
            }
        }

        /**
         * This override of Start disables the character relying on a rigidbody. 
         */
        protected override void Start() {
            Health = MaxHealth;

            if (RangedUnitAttack != null)
                RangedUnitAttack.GetComponent<RangedAttack>().GetValues(UnitType.ToString(), transform);
            if (MeleeUnitAttack != null)
                MeleeUnitAttack.GetComponent<MeleeAttack>().GetValues(UnitType.ToString(), transform);
        }

        /**
         * This override of FixedUpdate removes the updating of the character's
         * rigidbody.
         */
        protected override void FixedUpdate() {

        }

        public override void Attack() {
            if (!attacked) { // If the unit hasn't just attacked. 
                if (RangedUnitAttack != null) {
                    attacked = true;
                    Object.Instantiate(RangedUnitAttack);
                }
                else if (MeleeUnitAttack != null) {
                    attacked = true;
                    Object.Instantiate(MeleeUnitAttack);
                }
            }
        }
    }
}