using UnityEngine;
using System.Collections;

namespace AdventureFVTC
{
    public class LoadBoss : MonoBehaviour
    {

        void OnTriggerEnter(Collider collider)
        {
            if (collider.tag == "Player")
            {
                if (PersistentPlayerStats.HasLevel2GoalBeenMet) // If the player has gotten all the items they need from level2.
                    Application.LoadLevel("Boss Room");
            }
        }
    }
}
