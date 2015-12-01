using UnityEngine;
using System.Collections;

public class RemoveMe : MonoBehaviour
{
    private Transform trans;

    public void Setup()
    {
        trans = GetComponent<Transform>();
    }

    void Update()
    {
        if (trans != null && trans.position.y < 0)
        {
            Destroy(gameObject);
        }
    }
}

