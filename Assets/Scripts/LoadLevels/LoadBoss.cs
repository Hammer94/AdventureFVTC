using UnityEngine;
using System.Collections;

public class LoadBoss : MonoBehaviour {

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            Application.LoadLevel("Boss Room");
        }
    }
}
