using UnityEngine;
using System.Collections;

public class EngagedState : State
{
    private Transform trans;
    private Transform playerTrans;
    private GameObject player;

    public float Speed { get; set; }    // patrol speed

    public EngagedState(StateController controller)
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

        //float step = Speed * deltaTime; // calculate how far to move
        //trans.position += trans.forward * step; // move fwd

        float dist = (trans.position - target).magnitude;

        if (dist < 2)
        {
            Controller.Print("Engaged");
        }
        else if (dist > 2)
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
