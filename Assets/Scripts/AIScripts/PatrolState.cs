using UnityEngine;
using System.Collections.Generic;
namespace AdventureFVTC
{
    public class PatrolState : State
    {
        private Transform trans;    // trans of the enemy so we can move it
        private int index = 0;      // index of point we're going toward
        private int damping = 2;

        public float Speed { get; set; }    // patrol speed

        public List<Transform> PatrolPoints { get; private set; } // setup in the controller

        public PatrolState(StateController controller)
            : base(controller) // pass the controller to the base class
        {
            // get the trans from the controller
            trans = GameObject.GetComponent<Transform>();
            Enemy = GameObject.GetComponent<Enemy>();
            PatrolPoints = new List<Transform>(); // instanciate the list
        }

        public override void Update(float deltaTime)
        {
            if (Enemy.WasSetToUseStuckPrevention)
                Enemy.UseStuckPrevention = true;

            if (PatrolPoints.Count > 1)
            {
                Vector3 target = PatrolPoints[index].position;
                target.y = trans.position.y; // ignore vertical position

                float dist = (trans.position - target).magnitude;

                if (dist <= 1) // have we arrived at the target
                {
                    index++; // next point
                    if (index >= PatrolPoints.Count)
                    {
                        index = 0; // cycle through the points
                    }
                }
                else
                {
                    Enemy.RotateTowards(target);

                    float step = Speed * deltaTime; // calculate how far to move
                    trans.position += trans.forward * step; // move fwd

                    // are we close enough?
                    if ((trans.position - target).magnitude < step)
                    {
                        trans.position = target;
                    }
                }
            }
        }

        // use these for changing state
        public const string KEY = "PatrolState";
        public override string GetKey()
        {
            return KEY;
        }
    }
}
