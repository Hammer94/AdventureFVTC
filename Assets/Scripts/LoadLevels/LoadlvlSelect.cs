using UnityEngine;
using System.Collections;

public class LoadlvlSelect : MonoBehaviour 
{

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            Application.LoadLevel("LevelSelect");
        }
    }
}
