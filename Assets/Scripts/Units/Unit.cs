using UnityEngine;

namespace AdventureFVTC {
    /**
     * A unit in the game. It has health and can move around within the map.
     * 
     * @author  Ryan
     * @date    08 Dec 2015
     */
    public class Unit:MonoBehaviour {
        public enum UnitTypes {
            Unit,
            Player,
            Snowman,
            WaterMonster,
            Demon
        }

        private Vector2 desiredDirection;
        [SerializeField] private int maxHealth;     
        [SerializeField] private UnitTypes unitType = UnitTypes.Unit;
        [SerializeField] private GameObject rangedUnitAttack;
        [SerializeField] private GameObject meleeUnitAttack;
        [SerializeField] private float attackInterval = 1.0f;
        [SerializeField] private float deathTime = 3.0f;
        protected bool attacked = false;
        private float currentAttackInterval = 0.0f;
        private int health;
        private bool dead = false;
        
        private float currentDeathTime = 0.0f;

        public float speed;
        public float rotationSpeed;
        public float desiredRotation;

        /**
         * The maximum health of the Unit. Updates health if it becomes
         * out of bounds when this is changed.
         * 
         * @param   value   The value to set the max health to.
         * @return          The max health.
         * @see     #Health
         */
        public int MaxHealth {
            get {
                return maxHealth;
            }
            set {
                maxHealth = value;
                if (health > maxHealth)
                    health = maxHealth;
            }
        }

        /**
         * The health of the Unit. Ensures that health remains within
         * the bounds of 0 and the max health.
         * 
         * @param   value   The value to set the health to.
         * @return          The health.
         * @see     #MaxHealth
         */
        public int Health {
            get {
                return health;
            }
            set {
                health = value;
                if (health > maxHealth)
                    health = maxHealth;
                else if (health < 0)
                    health = 0;
            }
        }

        // The type of this unit.
        public UnitTypes UnitType {
            get {
                return unitType;
            }
            set {
                unitType = value;
            }
        }

        // This unit's time between attacks.
        public float CurrentAttackInterval {
            get {
                return currentAttackInterval;
            }
        }

        // This unit's rangedAttack.
        public GameObject RangedUnitAttack
        {
            get {
                return rangedUnitAttack;
            }
            set {
                rangedUnitAttack = value;
            }
        }

        // This unit's meleeAttack.
        public GameObject MeleeUnitAttack
        {
            get
            {
                return meleeUnitAttack;
            }
            set
            {
                meleeUnitAttack = value;
            }
        }

        // The time this unit is allowed to be in the gamespace after dying.
        protected float DeathTime
        {
            get { return deathTime; }
        }

        // The time since this unit has died. Will be compared to DeathTime.
        protected float CurrentDeathTime {
            get { return currentDeathTime; }
            set { currentDeathTime = value; }
        }

        /**
         * Moves the Unit to a specific position in map space.
         * 
         * @param   position    The position to move to.
         */
        public void moveTo(Vector2 position) {
            desiredDirection = position - toMapCoords(GetComponent<Transform>().position);
        }

        /**
         * Move the Unit to a specific position in world space.
         * 
         * @param   position    The position to move to.
         */
        public void moveTo(Vector3 position) {
            moveTo(toMapCoords(position));
        }

        /**
         * Moves the Unit in a specific map space direction.
         * 
         * @param   direction   The vector direction to move in.
         */
        public void moveDirection(Vector2 direction) {
            desiredDirection = direction.normalized * speed;
        }

        /**
         * Moves the Unit in a specific world space direction.
         * 
         * @param   direction   The vector direction to move in.
         */
        public void moveDirection(Vector3 direction) {
            moveDirection(toMapCoords(direction));
        }

        /**
         * Converts world coordinates to map coordinates.
         * 
         * @param   vec The world coordinate vector.
         * @return      The map coordinate vector.
         */
        private Vector2 toMapCoords(Vector3 vec) {
            return new Vector2(vec.x, vec.z);
        }

        /**
         * Converts map coordinates to world coordinates.
         * 
         * @param   vec The map coordinate vector.
         * @return      The world coordiante vector.
         */
        private Vector3 toWorldCoords(Vector2 vec) {
            return new Vector3(vec.x, 0, vec.y);
        }

        /**
         * Calculates the difference between two angles.
         * 
         * @param   a   The first operand. (a-b)
         * @param   b   The second operand. (a-b)
         * @return      The difference.
         */
        private float DeltaAngle(float a, float b) {
            float diff = a - b;
            diff = diff % (Mathf.PI * 2);
            if (diff > Mathf.PI)
                diff = -Mathf.PI * 2 + diff;
            return diff;
        }

        // If the unit has a ranged attack, do that. Otherwise, do its melee attack.
        public virtual void Attack() {
            if (!attacked)
            { // If the unit hasn't just attacked. 
                if (rangedUnitAttack != null) {
                    attacked = true;
                    rangedUnitAttack.GetComponent<RangedAttack>().GetValues(UnitType.ToString(), transform);
                    Object.Instantiate(RangedUnitAttack);
                }
                else if (meleeUnitAttack != null) {
                    attacked = true;
                    meleeUnitAttack.GetComponent<MeleeAttack>().GetValues(UnitType.ToString(), transform);
                    Object.Instantiate(meleeUnitAttack);
                }
            }
        }

        /**
         * Called when the unit's health reaches zero.
         */
        public virtual void Die() {
            currentDeathTime += Time.deltaTime; // Update the time since this unit has died.
            if (currentDeathTime > deathTime) // Once the unit has been dead for the allowed time.
                Destroy(gameObject); // Remove this unit.
        }

        /**
         * Called when the script starts. Ensures this Unit
         * has a rigidbody. Disables the script if a rigidbody
         * doesn't exist.
         */
        protected virtual void Start() {
            if (GetComponent<Rigidbody>() == null)
                enabled = false;
            GetComponent<Rigidbody>().maxAngularVelocity = 100;
            health = maxHealth;

            if (rangedUnitAttack != null)
                rangedUnitAttack.GetComponent<RangedAttack>().GetValues(unitType.ToString(), transform);
            if (meleeUnitAttack != null)
                meleeUnitAttack.GetComponent<MeleeAttack>().GetValues(unitType.ToString(), transform);
        }

        protected virtual void Update() {
            if (attacked) { // If the unit has just attacked.
                currentAttackInterval += Time.deltaTime; // Count since the unit last attacked.

                if (currentAttackInterval >= attackInterval) { // If the count has reached the set interval between this unit's attacks.
                    currentAttackInterval = 0.0f; // Reset the count.
                    attacked = false; // Allow the unit to attack again.
                }
            }

            if (health == 0)
                Die(); // Start the unit's death sequence.        
        }

        /**
         * Called every physics step. Calculates the direction to move and rotate
         * and applies the related velocities and angular velocities to the
         * associated rigidbody.
         */
        protected virtual void FixedUpdate() {
            Vector2 moveVector = desiredDirection;
            float rot = DeltaAngle(desiredRotation, -(Mathf.Deg2Rad * GetComponent<Transform>().eulerAngles.y) - Mathf.PI / 2);

            if (moveVector.magnitude >= speed * Time.deltaTime)
                moveVector = moveVector.normalized * speed;
            else
                moveVector = moveVector / Time.deltaTime;
            desiredDirection -= moveVector;

            if (Mathf.Abs(rot) >= rotationSpeed * Time.deltaTime)
                rot = Mathf.Sign(rot) * rotationSpeed;
            else
                rot = rot / Time.deltaTime;

            GetComponent<Rigidbody>().velocity = toWorldCoords(moveVector);
            GetComponent<Rigidbody>().angularVelocity = new Vector3(0, -rot, 0);
        }
    }
}