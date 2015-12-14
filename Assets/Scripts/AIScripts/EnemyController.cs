using UnityEngine;
using System.Collections.Generic;
namespace AdventureFVTC
{
    public class EnemyController : StateController
    {
        // inspector variables
        public List<string> PatrolPointNames = new List<string>();
        private Enemy enemy;

        private PatrolState patrol; // so we can update the properties
        private AttackState attack;
        private DieState die;

        public PatrolState Patrol
        {
            get { return patrol; }
        }

        public virtual void Start()
        {
            enemy = GetComponent<Enemy>();

            patrol = new PatrolState(this); // create the state
            attack = new AttackState(this);
            die = new DieState(this);

            //set the patrol points
            foreach (string name in PatrolPointNames)
            {
                GameObject point = GameObject.Find(name);
                if (point != null)
                {
                    Transform t = point.GetComponent<Transform>();
                    patrol.PatrolPoints.Add(t);
                }
            }
            States.Add(patrol); // add to the list
            States.Add(attack);
            States.Add(die);
            
            ChangeState(patrol.GetKey()); // set the first state to patrol
        }

        protected override void Update()
        {
            base.Update();

            if (enemy != null)
            {
                patrol.Speed = enemy.patrolSpeed; ; // so we can dynamically update the speed
                attack.Speed = enemy.attackSpeed;
                die.Speed = enemy.patrolSpeed;
            }          
        }
    }

}