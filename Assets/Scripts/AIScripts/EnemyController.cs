using UnityEngine;
using System.Collections.Generic;

public class EnemyController : StateController
{
    // inspector variables
    public List<string> PatrolPointNames = new List<string>();
    public float patrolSpeed = 3f;
    public float attackSpeed = 5f;

    private PatrolState patrol; // so we can update the properties
    private AttackState attack;

    public virtual void Start()
    {
        patrol = new PatrolState(this); // create the state
        attack = new AttackState(this);

        // set the patrol points
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

        ChangeState(patrol.GetKey()); // set the first state to patrol
    }

    protected override void Update()
    {
        base.Update();

        patrol.Speed = patrolSpeed; // so we can dynamically update the speed
        attack.Speed = attackSpeed;
    }
}

