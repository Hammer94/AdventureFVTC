using UnityEngine;
using System.Collections;
namespace AdventureFVTC
{
    public class AttackState : State
    {
        private Transform trans;
        private Transform playerTrans;
        private bool inAttackRange = false;

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

        public override void Update(float deltaTime) {
            Vector3 target = playerTrans.position;
            target.y = trans.position.y;

            Enemy.RotateTowards(target);

            float step = Speed * deltaTime; // calculate how far to move

            float dist = (trans.position - target).magnitude;
            

            if (Enemy.Health == 0)
                Controller.ChangeState(DieState.KEY);

            if (dist > 12)
                trans.position += trans.forward * step; // move fwd

            if (Services.Run.Player.Character.Dying || dist > 24)
            {
                Controller.ChangeState(PatrolState.KEY);
            }
            else if (dist < 12)
            {
                Vector3 targetDir = target - trans.position;
                float angleBetween = Vector3.Angle(trans.forward, targetDir);
                if (angleBetween < 5.0f)                 
                    Enemy.Attack();
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
