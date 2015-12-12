using UnityEngine;

// @author  Ryan
// @date    10 Dec 2015
namespace AdventureFVTC {
    public class Enemy:Unit {
        private string attackType = "Punch"; // Will either be Punch or Fireball.
        private float rotationStepped = 0.0f; // Used to track how far the enemy has rotated upon dying.
        private bool finishedRotation = false;
        private Renderer enemyRenderer;

        protected override void Start() {
            //base.Start();
            Health = MaxHealth;
            enemyRenderer = GetComponent<Renderer>();
        }

        /**
         * This override implements the enemy rotating upon dying, letting the player know that the enemy is dead.
         * Called when the unit's health reaches zero.
         */
        public override void Die() {
            if (UnitType == UnitTypes.Snowman || UnitType == UnitTypes.WaterMonster) { // If the enemy is a Snowman or WaterMonster.          
                if (!finishedRotation) {
                    float deathRotationRate = Time.deltaTime / (90.0f / (-0.5f * DeathTime));   // Get the rotation rate needed to rotate the enemy unit 
                                                                                                // -90 degrees in half the DeathTime.
                    rotationSpeed += -1 * deathRotationRate; // Keep track of how far the enemy has rotated.

                    if (rotationSpeed >= 90.0f) { // If the enemy has rotated 90 or more degrees.
                        finishedRotation = true; // Stop the rotation.
                        deathRotationRate -= (rotationSpeed - 90.0f); // Pull the rotation back a bit.
                    }

                    float xRotation = transform.eulerAngles.x + deathRotationRate; // Get the new x rotation.
                    GameObject body = transform.Find("body").gameObject;
                    if (body != null)
                        body.transform.eulerAngles = new Vector3(xRotation, body.transform.eulerAngles.y, body.transform.eulerAngles.z); // Update the enemy's current rotation.
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
