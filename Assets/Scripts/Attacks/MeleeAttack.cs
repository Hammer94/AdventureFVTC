using UnityEngine;

// @author  Ryan
// @date    10 Dec 2015
namespace AdventureFVTC {
    public class MeleeAttack:Attack {
        public Transform unitTrans;
        private Vector3 atAttackForward;

        protected override void Start() {
            base.Start();

        }

        protected override void FixedUpdate() {
            base.FixedUpdate();

            if (!positionSet) {
                positionSet = true;
                transform.position = unitTrans.position + (offset.z * unitTrans.forward) * 0.5f; // Move the attack to the front of its unit.
                atAttackForward = unitTrans.forward;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, unitTrans.eulerAngles.y, unitTrans.eulerAngles.z);
            }

            float speed = projectileSpeed * Time.deltaTime; // Get the rate at which the attack moves forward.               
            Vector3 newPosition = transform.position + (atAttackForward * speed); // And then move the attack forward.
            transform.position = newPosition;         
        }

        public void GetValues(string type, Transform transform)
        {
            originType = (OriginTypes)System.Enum.Parse(typeof(OriginTypes), type);
            unitTrans = transform;
        }
    }
}
