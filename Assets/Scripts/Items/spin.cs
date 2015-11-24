using UnityEngine;
using System.Collections;

public class spin : MonoBehaviour {

	// Update is called once per frame
	void Update () 
    {
        gameObject.transform.Rotate(0, 10, 0, Space.Self);
	}
}
