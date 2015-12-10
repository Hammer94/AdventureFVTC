using UnityEngine;

// @author  Ryan
// @date    10 Dec 2015
namespace AdventureFVTC {
    public class Attack:MonoBehaviour {
        public enum OriginTypes {
            Unit,
            Player,
            Snowman,
            WaterMonster,
            Demon
        }

        [SerializeField] protected int damage = 1;
        [SerializeField] protected Vector3 offset = new Vector3(0, 0, 1);
        [SerializeField] protected float projectileSpeed = 1.0f;
        [SerializeField] protected float activeTime = 1.0f;

        protected OriginTypes originType;

        protected bool hasCollided = false;
        protected bool isActive = true;
        protected bool positionSet = false;
        private float timeHasBeenActive = 0.0f;

        public OriginTypes OriginType {
            get {
                return originType; }
            }

        protected void OnCollisionStay(Collision collisionInfo) {
            Unit unit = collisionInfo.gameObject.GetComponent<Unit>();
            hasCollided = true; // Signal that there has been a collision with another collider. 

            // If the object is a unit and is different type than the unit that started this attack.
            if (unit != null && unit.UnitType.ToString() != originType.ToString()) {
                unit.Health -= damage; // Hurt the unit this attack hit.    
                Debug.Log("Has hurt " + unit.name + " " + damage + " damage!");
                Debug.Log(unit.name + " has " + unit.Health + " health remaining!");
            }                        
        }

        protected void OnTriggerEnter(Collider collider) {
            Unit unit = collider.gameObject.GetComponent<Unit>();

            // If the object is a unit and is different type than the unit that started this attack.
            if (unit != null && unit.UnitType.ToString() != originType.ToString()) {
                unit.Health -= damage; // Hurt the unit this attack hit.    
                Debug.Log("Has hurt " + unit.name + " " + damage + " damage!");
                Debug.Log(unit.name + " has " + unit.Health + " health remaining!");
            }
        }

        protected virtual void Start() {     
            if (originType == OriginTypes.Player || originType == OriginTypes.Unit)          
                transform.parent = Services.Run.Game.transform.Find("Players").transform;           
            else           
                transform.parent = Services.Run.Game.transform.Find("Enemies").transform;        
        }

        protected virtual void Update() {
            if (!isActive)
                Destroy(gameObject);
        }

        protected virtual void FixedUpdate() {
            // If the attack has started.
            timeHasBeenActive += Time.deltaTime; // Keep track of how long the attack has been active.

            if (timeHasBeenActive >= activeTime) // Once the allowed active time has been reached.
                isActive = false; // Remove this attack.         
        }      
    }
}
