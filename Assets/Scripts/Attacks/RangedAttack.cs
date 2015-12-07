using UnityEngine;

// @author  Ryan
// @date    07 Dec 2015
namespace AdventureFVTC {
    public class RangedAttack:Attack {
        public RangedAttack(string type, Transform transform) :base() {
            originType = (OriginTypes)System.Enum.Parse(typeof(OriginTypes), type);
            unitTrans = transform;
        }

        protected override void Start() {
            base.Start();

        }

        protected override void FixedUpdate() {
            base.FixedUpdate();

            if (startAttack) {
                if (!positionSet)
                {
                    positionSet = true;
                    transform.position = unitTrans.position + offset + unitTrans.forward * 0.5f; // Move the attack to the front of its unit.
                    //clone.GetComponent<RemoveMe>().Setup();
                    
                    Rigidbody rbody = GetComponent<Rigidbody>();
                    rbody.useGravity = true;
                    Vector3 force = (new Vector3(0, 0, 0) + unitTrans.forward) * 1000; // And then move the attack foward.
                    rbody.AddForce(force);
                }           
            }

            // Once this attack has collided with any object.
            if (hasCollided)
                isActive = false; // Remove the attack.    
        }
    }
}
