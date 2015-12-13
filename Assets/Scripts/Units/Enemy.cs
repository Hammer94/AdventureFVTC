﻿using UnityEngine;

// @author  Ryan
// @date    10 Dec 2015
namespace AdventureFVTC {
    public class Enemy:Unit {
        private string attackType = "Punch"; // Will either be Punch or Fireball.
        private float rotationStepped = 0.0f; // Used to track how far the enemy has rotated upon dying.
        private bool finishedRotation = false;
        private Renderer[] enemyRenderers;
        private Vector3 startingScale;
        private int damping = 2;

        protected override void Start() {
            //base.Start();
            Health = MaxHealth;
            enemyRenderers = GetComponents<Renderer>();
            startingScale = transform.localScale;
        }

        public void RotateTowards(Vector3 targetPosition)
        {
            Vector3 lookPos = targetPosition - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
        }

        protected override void FixedUpdate()
        {

        }

        /**
         * This override implements the enemy rotating upon dying, letting the player know that the enemy is dead.
         * Called when the unit's health reaches zero.
         */
        public override void Die() {
            if (UnitType == UnitTypes.Snowman || UnitType == UnitTypes.WaterMonster) { // If the enemy is a Snowman or WaterMonster.          
                if (!finishedRotation) {

                    if (dying)
                    {
                        cantMove = true;

                        CurrentDeathTime += Time.deltaTime; // Update the time since this unit has died.
                        if (CurrentDeathTime > DeathTime) // Once the unit has been dead for the allowed time.
                            CurrentDeathTime = DeathTime;

                        float perc = CurrentDeathTime / DeathTime;
                        Vector3 newScale = new Vector3(0.1f, 0.1f, 0.1f);
                        transform.localScale = Vector3.Lerp(startingScale, newScale, perc);

                        
                        Color darkGrey = new Color(0.2f, 0.2f, 0.2f, 1f);
                        foreach (Renderer r in enemyRenderers)
                        {
                            r.material.color = Color.Lerp(Color.white, darkGrey, perc); // Make the rabbit fade to grey.

                            if (r.material.color == darkGrey) {
                                Destroy(gameObject);
                            }
                        }                   
                    }
                }             
            }

            CurrentDeathTime += Time.deltaTime; // Update the time since this unit has died.
            if (CurrentDeathTime > DeathTime) // Once the unit has been dead for the allowed time.
                Destroy(gameObject); // Remove this unit.
        }

        // Updates the attack type and then attacks.This will be used for the Demon, other enemy types will ignore attackType.
        public void DemonAttack(string attack) {
            attackType = attack;
            Attack();
        }

        /**
         * This override implements the enemy attacking based on its UnitType.
         */
        public override void Attack() {
            if (!attacked) {
                if (UnitType == UnitTypes.Demon) { // If the enemy is a Demon.
                    if (attackType == "Punch") { // And the attackType is Punch.
                        if (MeleeUnitAttack != null) {
                            attacked = true;
                            GameObject clone = GameObject.Instantiate(MeleeUnitAttack); // Perform a melee attack.
                            clone.GetComponent<MeleeAttack>().GetValues(UnitType.ToString(), transform);
                        }
                    }
                    else if (attackType == "Fireball") { // And the attackType is Fireball.
                        if (RangedUnitAttack != null) {
                            attacked = true;
                            GameObject clone = GameObject.Instantiate(RangedUnitAttack); // Perform a ranged attack.
                            clone.GetComponent<RangedAttack>().GetValues(UnitType.ToString(), transform);
                        }
                    }
                }
                if (UnitType == UnitTypes.Snowman || UnitType == UnitTypes.WaterMonster) { // If the enemy is either a Snowman or a WaterMonster.
                    if (RangedUnitAttack != null) { // Perform a ranged attack if this enemy has one.
                        attacked = true;
                        GameObject clone = GameObject.Instantiate(RangedUnitAttack);
                        clone.GetComponent<RangedAttack>().GetValues(UnitType.ToString(), transform);
                    }
                    else if (MeleeUnitAttack != null) { // Otherwise Perform a melee attack.
                        attacked = true;
                        GameObject clone = GameObject.Instantiate(MeleeUnitAttack);
                        clone.GetComponent<MeleeAttack>().GetValues(UnitType.ToString(), transform);
                    }
                }
            }                     
        }
    }
}
