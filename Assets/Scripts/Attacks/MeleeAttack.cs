using UnityEngine;

// @author  Ryan
// @date    07 Dec 2015
namespace AdventureFVTC {
    public class MeleeAttack:Attack {

        public MeleeAttack(string type, Transform transform) :base() {
            originType = (OriginTypes)System.Enum.Parse(typeof(OriginTypes), type);
            unitTrans = transform;
        }

        protected override void Start() {
            base.Start();

        }

        protected override void FixedUpdate() {
            base.FixedUpdate();

            if (startAttack) {
                float speed = projectileSpeed * Time.deltaTime; // Get the rate at which the attack moves forward.
                if (!positionSet)
                    transform.position = unitTrans.position + offset + unitTrans.forward * 0.5f; // Move the attack to the front of its unit.
                Vector3 newPosition = transform.forward + (transform.forward * speed); // And then move the attack forward.
            }
        }
    }
}
