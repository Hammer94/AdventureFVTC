using UnityEngine;

namespace AdventureFVTC {
    /**
     * A unit in the game. It has health and can move around within the map.
     * 
     * @author  Ryan
     * @date    13 Dec 2015
     */
    public class Unit:MonoBehaviour {
        public enum UnitTypes {
            Unit,
            Player,
            Snowman,
            WaterMonster,
            Demon
        }

        public enum UnitStates
        {
            Patroll,
            Attack,
            Die
        }

        private Vector2 desiredDirection;
        [SerializeField] private int maxHealth;     
        [SerializeField] private UnitTypes unitType = UnitTypes.Unit;
        [SerializeField] private GameObject rangedUnitAttack;
        [SerializeField] private GameObject meleeUnitAttack;
        [SerializeField] private float attackInterval = 1.0f;
        [SerializeField] private float deathTime = 3.0f;
        [SerializeField] protected float timeCanMoveAfterAttack = 0.2f;
        [SerializeField] protected float delayBeforeAttackStarts = 0.2f;
        [SerializeField] protected float invulnerabilityTime = 0.0f;

        private bool tookDamage;
        protected bool isInvulnerable = false;
        protected float currentInvulnTime = 0.0f;
        protected UnitStates unitState;
        protected bool startDelay = false;
        protected bool cantMove = false;
        protected bool attacked = false;
        private float currentAttackInterval = 0.0f;
        protected float timeDelayed = 0.0f;
        protected int health;
        protected bool dying = false;
        protected bool dead = false;
        
        private float currentDeathTime = 0.0f;

        public float patrolSpeed;
        public float attackSpeed;
        public float rotationSpeed;
        public float desiredRotation;

        public bool TookDamage {
            get {
                return tookDamage;
            }
            set {
                tookDamage = value;
            }
        }

        public bool Dying {
            get {
                return dying;
            }
        }

        public bool Dead {
            get {
                return dead;
            }
            set {
                dead = value;
            }
        }

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
                if (invulnerabilityTime != 0 && !isInvulnerable) { // If this unit just got hurt, can become invulnerable, and isn't already invulnerable.                  
                    if (value < health) {
                        isInvulnerable = true; // Make this unit invulnerable.
                        tookDamage = true;
                    }
                        
                    health = value; // Hurt this unit.                      
                }                  
                else if (invulnerabilityTime == 0) { // Else this unit can't be invulnerable.
                    if (value < health)
                        tookDamage = true;
                    health = value;          
                }                   
                if (health > maxHealth)
                    health = maxHealth;
                else if (health < 0)
                    health = 0;              
                if (health == 0) // If this unit has 0 health left.
                    dying = true; // Start this unit's death sequence.
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

        // Used to delay the unit after it attacks, not allowing it to attack and move at the same time.
        public bool CantMove {
            get {
                return cantMove;
            }
            set {
                cantMove = value;
            }
        }

        // This unit's time since the last attack. Once this time reaches its maximum, the unit can attack again.
        public float CurrentAttackInterval {
            get {
                return currentAttackInterval;
            }
            set {
                currentAttackInterval = value;
            }
        }

        // This unit's time between attacks.
        public float AttackInterval {
            get {
                return attackInterval;
            }
        }

        // This unit's rangedAttack.
        public GameObject RangedUnitAttack {
            get {
                return rangedUnitAttack;
            }
            set {
                rangedUnitAttack = value;
            }
        }

        // This unit's meleeAttack.
        public GameObject MeleeUnitAttack {
            get {
                return meleeUnitAttack;
            }
            set {
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

        public bool IsInvulnerable {
            get {
                return isInvulnerable;
            }
        }

        protected void RemoveInulverability() {
            currentInvulnTime += Time.deltaTime;

            if (currentInvulnTime >= invulnerabilityTime) {
                currentInvulnTime = 0.0f;
                isInvulnerable = false;
            }
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
            if (unitState == UnitStates.Patroll)
                desiredDirection = direction.normalized * patrolSpeed;
            else if (unitState == UnitStates.Attack)
                desiredDirection = direction.normalized * attackSpeed;
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
            if (!startDelay) // If the delay hasn't been started yet.
                startDelay = true; // Start the delay before the attack.
            else { // Attack;
                if (!attacked)
                { // If the unit hasn't just attacked. 
                    if (rangedUnitAttack != null)
                    {
                        attacked = true;
                        GameObject clone = GameObject.Instantiate(RangedUnitAttack);
                        clone.GetComponent<RangedAttack>().GetValues(UnitType.ToString(), transform);
                    }
                    else if (meleeUnitAttack != null)
                    {
                        attacked = true;
                        GameObject clone = GameObject.Instantiate(RangedUnitAttack);
                        clone.GetComponent<MeleeAttack>().GetValues(UnitType.ToString(), transform);
                    }
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
        }

        protected virtual void Update() {
            if (startDelay) { // If the unit wants to attack.
                cantMove = true; // Disallow the unit to be able to move.
                timeDelayed += Time.deltaTime; // Count down.

                if (timeDelayed >= timeCanMoveAfterAttack)
                    cantMove = false; // Allow the unit to move again.

                if (timeDelayed >= delayBeforeAttackStarts) { // Once the unit has waited enough time.
                    Attack(); // Attack.
                }
            }

            if (attacked) { // If the unit has just attacked.
                currentAttackInterval += Time.deltaTime; // Count since the unit last attacked.
             
                if (currentAttackInterval >= attackInterval) { // If the count has reached the set interval between this unit's attacks.
                    currentAttackInterval = 0.0f; // Reset the count.
                    timeDelayed = 0.0f; // Reset the delay.
                    attacked = false; // Allow the unit to attack again.
                    startDelay = false; // Renable the delay for the next attack.
                }
            }

            if (isInvulnerable) // If this unit has taken damage and has become invulnerable
                RemoveInulverability(); // After a specified amount of time, allow the unit to get damage again.

            if (dying) // If this unit has 0 health.
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

            if (unitState == UnitStates.Patroll) {
                if (moveVector.magnitude >= patrolSpeed * Time.deltaTime)
                    moveVector = moveVector.normalized * patrolSpeed;
                else
                    moveVector = moveVector / Time.deltaTime;
            }
            else if (unitState == UnitStates.Attack)
            {
                if (moveVector.magnitude >= attackSpeed * Time.deltaTime)
                    moveVector = moveVector.normalized * attackSpeed;
                else
                    moveVector = moveVector / Time.deltaTime;            
            }
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