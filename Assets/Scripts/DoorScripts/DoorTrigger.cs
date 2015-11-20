using UnityEngine;
using System.Collections;

public class DoorTrigger : MonoBehaviour
{
    private DoorOpener door;

    public int Channel = 1;
    public bool ClickToOpen = true;

    void Start()
    {
        DoorOpener[] doors = GameObject.FindObjectsOfType<DoorOpener>();
        for (int i = 0; i < doors.Length; i++)
        {
            if (doors[i].Channel == Channel)
            {
                door = doors[i];
                break;
            }
        }
    }

    void OnTriggerEnter(Collider obj)
    {
        if (obj.tag == "Player" && door != null && !ClickToOpen)
        {
            door.Open();
        }
    }

    void OnTriggerExit(Collider obj)
    {
        if (obj.tag == "Player" && door != null && !ClickToOpen)
        {
            door.Close();
        }
    }

    void OnTriggerStay(Collider obj)
    {
        if (Input.GetMouseButtonDown(0) && obj.tag == "Player"
            && door != null && ClickToOpen)
        {
            door.Toggle();
        }
    }
}
