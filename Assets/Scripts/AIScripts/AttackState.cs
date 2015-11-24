using UnityEngine;
using System.Collections;

public class AttackState : State
{
    private Transform trans;
    private Transform playerTrans;
    private GameObject player;

    public float Speed { get; set; }    // patrol speed

    public AttackState(StateController controller)
        : base(controller) // pass the controller to the base class
    {
        // get the trans from the controller
        trans = GameObject.GetComponent<Transform>();

        // player stuff
        player = GameObject.FindGameObjectWithTag("Player");
        playerTrans = player.GetComponent<Transform>();
    }

    public override void Update(float deltaTime)
    {
        Vector3 target = playerTrans.position;
        target.y = trans.position.y;

        trans.LookAt(target);

        float step = Speed * deltaTime; // calculate how far to move
        trans.position += trans.forward * step; // move fwd

        float dist = (trans.position - target).magnitude;

        if(dist < 4)
        {
            Controller.ChangeState(EngagedState.KEY);
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

