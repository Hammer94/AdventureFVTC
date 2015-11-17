using UnityEngine;
using System.Collections;

public class LoadLevel2 : MonoBehaviour {

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            Application.LoadLevel("Level2");
        }
    }
}
