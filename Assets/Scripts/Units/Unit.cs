using UnityEngine;

namespace AdventureFVTC {
    /**
     * A unit in the game. It has health and can move around within the map.
     * 
     * @author  Ryan
     * @date    16 Nov 2015
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
        [SerializeField] private MeleeAttack meleeAttack;
        [SerializeField] private RangedAttack rangedAttack;
        [SerializeField] private float attackInterval = 1.0f;
        private bool attacked = false;
        private float currentAttackInterval = 0.0f;
        private int health;
        private bool dead = false;
        private float deathTime = 3.0f;
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

        public UnitTypes UnitType {
            get {
                return unitType;
            }
            set {
                unitType = value;
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
            if (!attacked) { // If the unit hasn't just attacked. 
                if (rangedAttack != null) {
                    attacked = true;
                    rangedAttack = new RangedAttack(unitType.ToString(), transform);
                    GameObject attack = Object.Instantiate(meleeAttack.GetComponent<GameObject>());
                }
                else if (meleeAttack != null) {
                    attacked = true;
                    meleeAttack = new MeleeAttack(unitType.ToString(), transform);
                    GameObject attack = Object.Instantiate(meleeAttack.GetComponent<GameObject>());
                }
            }
            
        }

        protected virtual void Update() {
            if (attacked) {
                currentAttackInterval += Time.deltaTime;

                if (currentAttackInterval >= attackInterval) {
                    currentAttackInterval = 0.0f;
                    attacked = false;
                }
            }           
                                   
            if (health == 0)          
                dead = true;
            if (dead) {
                //increment timer once per frame
                currentDeathTime += Time.deltaTime;
                if (currentDeathTime > deathTime)
                    Destroy(gameObject);
            }     
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