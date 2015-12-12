using UnityEngine;
using System.Collections;

namespace AdventureFVTC
{
    public class EngagedState : State
    {
        private Transform trans;
        private Transform playerTrans;
        //private Unit enemy;

        public float Speed { get; set; }    // patrol speed

        public EngagedState(StateController controller)
            : base(controller) // pass the controller to the base class
        {
            // get the trans from the controller
            trans = GameObject.GetComponent<Transform>();
            Enemy = GameObject.GetComponent<Enemy>();

            // player stuff
            playerTrans = Services.Run.Player.Character.gameObject.GetComponent<Transform>();
        }

        public override void Update(float deltaTime)
        {
            Vector3 target = playerTrans.position;
            target.y = trans.position.y;

            trans.LookAt(target);

            //float step = Speed * deltaTime; // calculate how far to move
            //trans.position += trans.forward * step; // move fwd

            float dist = (trans.position - target).magnitude;

            if (dist < 12)
            {
                Enemy.Attack();
            }
            else if (dist > 12)
            {
                Controller.ChangeState(AttackState.KEY);
            }
        }

        // use these for changing state
        public const string KEY = "EngagedState";
        public override string GetKey()
        {
            return KEY;
        }
    }
}

