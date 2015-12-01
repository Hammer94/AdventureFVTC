using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class EnemySnowball : MonoBehaviour {

    private GameObject ball;
    private Transform trans;
    private Transform playerTrans;
    private Transform camTrans;
    
    private GameObject player;
    private float time = 3f;
    private float timeWaited = 0f;
    private bool attacked = false;
    
    
    
    
    private Vector3 offset = new Vector3(0, 0.8f, 0);

    void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Snowball");
        trans = GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerTrans = player.GetComponent<Transform>();

        camTrans = GetComponentInChildren<Camera>().GetComponent<Transform>();
        

        Cursor.visible = false;
    }

    void Update()
    {
        if(attacked == true)
        {
            timeWaited += Time.deltaTime;
        }
        
        Vector3 target = playerTrans.position;
        target.y = trans.position.y;
        float dist = (trans.position - target).magnitude;


        if (dist < 8)
        {
            if (timeWaited == 0) 
            {
                attacked = true;
                GameObject clone = GameObject.Instantiate(ball);
                clone.GetComponent<Transform>().position = trans.position + offset + trans.forward * 0.5f;
                //clone.GetComponent<RemoveMe>().Setup();

                Rigidbody rbody = clone.GetComponent<Rigidbody>();
                rbody.useGravity = true;
                Vector3 force = (new Vector3(0, camTrans.forward.y + 0.2f, 0) + trans.forward) * 1000;
                rbody.AddForce(force);
                
            }
                
            if (timeWaited >= time)
            {
                timeWaited = 0;
                attacked = false;
            }

            
        }
    }
}
