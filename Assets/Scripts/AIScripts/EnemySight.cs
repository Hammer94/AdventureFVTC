using UnityEngine;
using System.Collections;

public class EnemySight : MonoBehaviour
{
    private Transform trans;
    private EnemyController controller;

    void Start()
    {
        controller = GetComponentInParent<EnemyController>();
        trans = GetComponent<Transform>().parent.GetComponent<Transform>();
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Player")
        {
            Vector3 playerPos = coll.GetComponent<Transform>().position;
            Vector3 dir = playerPos - trans.position;
            dir.Normalize();
            Ray ray = new Ray(trans.position, dir);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, 24))
            {
                if (hitInfo.collider.tag == "Player")
                {
                    controller.ChangeState(AttackState.KEY);
                }
            }
        }
    }
}
