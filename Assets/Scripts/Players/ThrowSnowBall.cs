using UnityEngine;
using System.Collections;

public class ThrowSnowBall : MonoBehaviour {

    private GameObject ball;
    private Transform trans;
    
    private Transform camTrans;
    private Vector3 offset = new Vector3(0, 0.8f, 0);

    void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Snowball");
        trans = GetComponent<Transform>();
        camTrans = GetComponentInChildren<Camera>().GetComponent<Transform>();
        


        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject clone = GameObject.Instantiate(ball);
            clone.GetComponent<Transform>().position = trans.position + offset + trans.forward * 0.5f;
            //clone.GetComponent<RemoveMe>().Setup();

            Rigidbody rbody = clone.GetComponent<Rigidbody>();
            rbody.useGravity = true;
            Vector3 force = (new Vector3(0, camTrans.forward.y + 0.2f, 0) + trans.forward) * 1000;
            rbody.AddForce(force);
        }
    }
}
