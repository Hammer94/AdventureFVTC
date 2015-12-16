using UnityEngine;

// @author  Ryan
// @date    15 Dec 2015
namespace AdventureFVTC
{
    public class DamageZone:MonoBehaviour
    {
        [SerializeField] private int damage = 1;
        
       private void OnTriggerStay(Collider collider)
        {
            Unit unit = collider.gameObject.GetComponent<Unit>();

            // If the object is a unit and is different type than the unit that started this attack.
            if (unit != null)
            {
                unit.Health -= damage;  // Hurt the unit this damage per trigger call. If the unit being hurt doesn't have an invulnerable time,
                                        // this trigger will kill the unit pretty much instantly.  
            }
        } 
    }
}
