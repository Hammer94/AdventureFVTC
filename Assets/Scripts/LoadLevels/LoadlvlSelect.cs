using UnityEngine;
using System.Collections;

namespace AdventureFVTC {
    public class LoadlvlSelect : MonoBehaviour
    {

        void OnTriggerEnter(Collider collider)
        {
            if (collider.tag == "Player")
            {
                if (Application.loadedLevelName == "Level1" && PersistentPlayerStats.HasLevel1GoalBeenMet) // If the player is on level1 but hasn't gotten the items needed to leave.            
                {
                    PersistentPlayerStats.UpdateTotalsOnExit("Level1", Services.Run.Player.Character.Health);
                    PersistentPlayerStats.ResetCurrentsOnExit();
                    Application.LoadLevel("LevelSelect");
                }
                else if (Application.loadedLevelName == "Level2" && PersistentPlayerStats.HasLevel2GoalBeenMet) // If the player is on level2 but hasn't gotten the items needed to leave.
                {
                    PersistentPlayerStats.UpdateTotalsOnExit("Level2", Services.Run.Player.Character.Health);
                    PersistentPlayerStats.ResetCurrentsOnExit();
                    Application.LoadLevel("LevelSelect");
                }
                else if (Application.loadedLevelName == "Boss Room") // Else the player is on a different level than 1 or 2.
                    Application.LoadLevel("LevelSelect");
            }
        }
    }
}
