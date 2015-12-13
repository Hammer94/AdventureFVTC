using UnityEngine;

// @author  Ryan
// @date    13 Dec 2015
namespace AdventureFVTC {
    public class RangedAttack:Attack {
        public Transform unitTrans;

        protected override void Start() {
            base.Start();

            Rigidbody rbody = GetComponent<Rigidbody>();
            rbody.useGravity = true;
        }

        protected override void Update()
        {
            base.Update();

            // Once this attack has collided with any object.
            if (hasCollided)
                isActive = false; // Remove the attack.    
        }

        protected override void FixedUpdate() {
            base.FixedUpdate();

            if (!positionSet) {
                positionSet = true;
                transform.position = unitTrans.position + (offset.z * unitTrans.forward) * 0.5f; // Move the attack to the front of its unit.

                Rigidbody rbody = GetComponent<Rigidbody>();
                rbody.useGravity = true;
                Vector3 force = (new Vector3(0, 0, 0) + unitTrans.forward) * (projectileSpeed * 1000); // And then move the attack foward.
                rbody.AddForce(force);
                }                 
        }

         void OnCollisionEnter(Collision collisionInfo) {
            Unit unit = collisionInfo.gameObject.GetComponent<Unit>();
            // If the object is a unit and is different type than the unit that started this attack.
            if (unit != null && unit.UnitType.ToString() != originType.ToString()) {
                unit.Health -= damage; // Hurt the unit this attack hit.    
                Debug.Log("Has hurt " + unit.name + " " + damage + " damage!");
                Debug.Log(unit.name + " has " + unit.Health + " health remaining!");
            }

            hasCollided = true; // Signal that there has been a collision with another collider.                         
        }

        public void GetValues(string type, Transform transform)
        {
            originType = (OriginTypes)System.Enum.Parse(typeof(OriginTypes), type);
            unitTrans = transform;
        }
    }
}
