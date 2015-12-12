using UnityEngine;
using System.Collections;
namespace AdventureFVTC
{
    public class AttackState : State
    {
        private Transform trans;
        private Transform playerTrans;

        public float Speed { get; set; }    // patrol speed

        public AttackState(StateController controller)
            : base(controller) // pass the controller to the base class
        {
            // get the trans from the controller
            trans = GameObject.GetComponent<Transform>();
            Enemy = GameObject.GetComponent<Enemy>();

            // player stuff
            playerTrans = Services.Run.Player.Character.gameObject.transform;          
        }

        public override void Update(float deltaTime)
        {
            Vector3 target = playerTrans.position;
            target.y = trans.position.y;

            trans.LookAt(target);

            float step = Speed * deltaTime; // calculate how far to move
            trans.position += trans.forward * step; // move fwd

            float dist = (trans.position - target).magnitude;

            if (Enemy.Health == 0)
                Controller.ChangeState(DyingState.KEY);

            if (dist < 12)
            {             
                Enemy.Attack();
            }
            else if (dist < step)
            {
                Controller.Print("Hitting Player");
            }
            else if (dist > 24)
            {
                Controller.ChangeState(PatrolState.KEY);
            }
        }

        // use these for changing state
        public const string KEY = "AttackState";
        public override string GetKey()
        {
            return KEY;
        }
    }
}
