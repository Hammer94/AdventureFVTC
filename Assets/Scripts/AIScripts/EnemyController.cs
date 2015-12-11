using UnityEngine;
using System.Collections.Generic;
namespace AdventureFVTC
{
    public class EnemyController : StateController
    {
        // inspector variables
        public List<string> PatrolPointNames = new List<string>();
        public float patrolSpeed = 3f;
        public float attackSpeed = 5f;

        private PatrolState patrol; // so we can update the properties
        private AttackState attack;
        private EngagedState engage;

        public PatrolState Patrol
        {
            get { return patrol; }
        }

        public virtual void Start()
        {
            Debug.Log("In EnemyController Start!");
            patrol = new PatrolState(this); // create the state
            Debug.Log("In EnemyController Start!");
            attack = new AttackState(this);
            Debug.Log("In EnemyController Start!");
            engage = new EngagedState(this);
            Debug.Log("In EnemyController Start!");

            //Debug.Log("Patrol is null? " + (patrol == null).ToString());
            //Debug.Log("Attack is null? " + (attack == null).ToString());
            //Debug.Log("Engage is null? " + (engage == null).ToString());

            // set the patrol points
            //foreach (string name in PatrolPointNames)
            //{
            //    GameObject point = GameObject.Find(name);
            //    if (point != null)
            //    {
            //        Transform t = point.GetComponent<Transform>();
            //        patrol.PatrolPoints.Add(t);
            //    }
            //}

            States.Add(patrol); // add to the list
            States.Add(attack);
            States.Add(engage);

            ChangeState(patrol.GetKey()); // set the first state to patrol
        }

        protected override void Update()
        {
            base.Update();

            patrol.Speed = patrolSpeed; // so we can dynamically update the speed
            attack.Speed = attackSpeed;
        }
    }

}