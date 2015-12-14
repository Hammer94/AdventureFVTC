using UnityEngine;
using System.Collections;

namespace AdventureFVTC
{
    public class LoadLevel : MonoBehaviour
    {


        void OnTriggerEnter(Collider collider)
        {
            if (collider.tag == "Player")
            {
                Application.LoadLevel("Level1");                            
            }
        }
    }
}

