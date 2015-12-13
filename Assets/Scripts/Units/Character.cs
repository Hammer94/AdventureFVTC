using UnityEngine;
using System.Collections.Generic;

namespace AdventureFVTC {
    /**
     * A Unit that operates like a player, able to collect and use items.
     * No longer relies on a rigidbody for movement.
     * 
     * @author  Ryan
     * @date    12 Dec 2015
     * @see     Item
     */
    public class Character : Unit {
        private string attackType = "Punch"; // Will either be Punch or Snowball.
        private SkinnedMeshRenderer characterRenderer;
        private Color initialCharacterColor;       

        public Color InitialCharacterColor {
            get {
                return initialCharacterColor;
            }
        }

        /**
         * Constructs a new Character. Intitializes the list of Item
         * for storing item information.
         * 
         * @see Item
         */
        Character()
        {

        }

        /**
         * This override of Start disables the character relying on a rigidbody. 
         */
        protected override void Start() {
            Health = MaxHealth;
            characterRenderer = GameObject.FindGameObjectWithTag("RabbitRenderer").GetComponent<SkinnedMeshRenderer>();
            initialCharacterColor = characterRenderer.material.color;         
        }

        /**
         * This override of FixedUpdate removes the updating of the character's
         * rigidbody.
         */
        protected override void FixedUpdate() {

        }

        // Updates the attack type and then attacks.This will be used for the Demon, other enemy types will ignore attackType.
        public void AttackSetup(string attack) {
            startDelay = true;
            attackType = attack;
        }

        public override void Attack() {
            if (!attacked)
            { // If the unit hasn't just attacked. 
                if (RangedUnitAttack != null && attackType == "Snowball") {
                    attacked = true;
                    GameObject clone = GameObject.Instantiate(RangedUnitAttack);
                    clone.GetComponent<RangedAttack>().GetValues(UnitType.ToString(), transform);
                }
                else if (MeleeUnitAttack != null && attackType == "Punch") {
                    attacked = true;
                    GameObject clone = GameObject.Instantiate(MeleeUnitAttack);
                    clone.GetComponent<MeleeAttack>().GetValues(UnitType.ToString(), transform);
                }
            }  
        }

          /**
         * This override implements the player turning grey upon dying, letting the player know that they have died.
         * Called when the unit's health reaches zero.
         */
        public override void Die() {
            if (dying) {
                cantMove = true;

                CurrentDeathTime += Time.deltaTime; // Update the time since this unit has died.
                if (CurrentDeathTime > DeathTime) // Once the unit has been dead for the allowed time.
                    CurrentDeathTime = DeathTime;

                Debug.Log(dying);

                //lerp!
                float perc = CurrentDeathTime / DeathTime;
                Color darkGrey = new Color(0.2f, 0.2f, 0.2f, 1f);
                characterRenderer.material.color = Color.Lerp(initialCharacterColor, darkGrey, perc); // Make the rabbit fade to grey.

                if (characterRenderer.material.color == darkGrey)
                {
                    CurrentDeathTime = 0.0f;
                    dying = false;
                    dead = true;
                    //cantMove = false;
                }
            }           
        }
    }
}