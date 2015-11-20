
using UnityEngine;
using System.Collections;

public class DoorOpener : MonoBehaviour
{
    // States for opening & closing a door
    public enum DoorState
    {
        Closed,
        Opening,
        Open,
        Closing
    }


    private Transform trans;            // the door transform
    private Transform hingeTrans;       // the hinge transform so we can rotate the door around it
    private float rotationAngle = 0;    // how far have we rotated?

    // Inspector Variables
    public DoorState State = DoorState.Closed;  // initial state of the door
    public bool SwitchOpenDirection = false;    // used to open the door the other way

    [Range(1.0f, 170.0f)]               // creates a slider in the inspector (Min = 1 degree, Max = 170 degrees)
    public float OpenRange = 90.0f;     // how far can the door open? (in degrees)

    public float Speed = 10;            // how fast the door opens/closes
    public int Channel = 1;             // for use with trigger

    void Start()
    {
        // grab the transforms from the prefab
        trans = GetComponent<Transform>();
        hingeTrans = trans.GetChild(0).GetComponent<Transform>();
    }

    void Update()
    {
        /* FOR TESTING *
        if (Input.GetMouseButtonDown(0))
        {
            Toggle();
        }
        /**/

        // Are we currently opening or closing?
        if (State == DoorState.Opening || State == DoorState.Closing)
        {
            // we might need to adjust the speed (but we dont want to change the variable the user set)
            // so we'll use a temporary one called angle and use that
            float angle = Speed;

            // Make sure the speed is not greater than the door range
            angle = Mathf.Min(angle * Time.deltaTime * 60, OpenRange);

            rotationAngle += angle; // try to move the door to the next step (frame)

            if (rotationAngle >= OpenRange) // if the door is all the way open (OR PASSED THE MAX!)
            {
                angle -= (rotationAngle - OpenRange); // shrink the angle a bit if the door has gone too far!

                // update the current state
                if (State == DoorState.Opening)
                {
                    State = DoorState.Open;
                }
                else if (State == DoorState.Closing)
                {
                    State = DoorState.Closed;
                }
            }

            // change the direction the door is moving if the door is/was
            // closing XOR if the door opens the other way
            bool close = (State == DoorState.Closing || State == DoorState.Closed);
            if (close ^ SwitchOpenDirection) { angle *= -1; }

            // rotate the door around the hinge
            trans.RotateAround(hingeTrans.position, Vector3.up, angle);
        }
    }

    public void Toggle()
    {
        if (State == DoorState.Closed)
        {
            State = DoorState.Opening; // Open the door!
            rotationAngle = 0; // We just started to open/close, so reset the rotation angle
        }
        else if (State == DoorState.Open)
        {
            State = DoorState.Closing; // Close the door!
            rotationAngle = 0; // We just started to open/close, so reset the rotation angle
        }
    }

    public void Open()
    {
        // only works if door is closed
        if (State == DoorState.Closed) Toggle();
    }

    public void Close()
    {
        // only works if door is open
        if (State == DoorState.Open) Toggle();
    }
}

