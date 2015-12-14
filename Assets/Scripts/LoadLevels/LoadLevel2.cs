using UnityEngine;
using System.Collections;

namespace AdventureFVTC
{
    public class LoadLevel2 : MonoBehaviour
    {

        void OnTriggerEnter(Collider collider)
        {
            if (collider.tag == "Player")
            {
                if (PersistentPlayerStats.HasLevel1GoalBeenMet) // If the player has gotten the items required from level one, left them enter level2.
                {
                    Application.LoadLevel("Level2");
                }              
            }
        }
    }
}
