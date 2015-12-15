using UnityEngine;
using System.Collections;
namespace AdventureFVTC
{
    public class AttackState : State
    {
        private Transform trans;
        private Transform playerTrans;
        private bool inAttackRange = false;

        public float Speed { get; set; }    // patrol speed

        public AttackState(StateController controller)
            : base(controller) // pass the controller to the base class
        {
            // get the trans from the controller
            trans = GameObject.GetComponent<Transform>();
            Enemy = GameObject.GetComponent<Enemy>();

            // player stuff
            playerTrans = Services.Run.Player.Character.gameObject.transform;          
        }

        public override void Update(float deltaTime) {
            Vector3 target = playerTrans.position;
            target.y = trans.position.y; // Ignore the y position of the target.
          
            Enemy.RotateTowards(target); // Look at the target.
            Enemy.UseStuckPrevention = false;

            float step = Speed * deltaTime; // Calculate how far to move.
            float dist = (trans.position - target).magnitude; // Get the distance between the enemy and its target.

            if (Enemy.Health == 0)
                Controller.ChangeState(DieState.KEY);

            if (Enemy.UnitType == Unit.UnitTypes.Snowman) // If this enemy is a snowman.
            {
                if (Enemy.RangedUnitAttack != null) // Use a ranged attack.
                {
                    if (dist > 15)
                        Enemy.MoveForward(step); // move fwd
                    if (Services.Run.Player.Character.Dying || dist > 20)
                        Controller.ChangeState(PatrolState.KEY);
                    else if (dist < 15)
                    {
                        Vector3 targetDir = target - trans.position;
                        float angleBetween = Vector3.Angle(trans.forward, targetDir);
                        if (Enemy.MaxHealth > 1) { // If this enemy is the Snow King.
                            if (angleBetween < 20.0f)
                                Enemy.AttackSetup("");
                        }
                        else { // Else this is a standard snowman.
                            if (angleBetween < 10.0f)
                                Enemy.AttackSetup("");
                        }                      
                    }
                }
                else if (Enemy.MeleeUnitAttack != null) // Use a melee attack.
                {
                    if (dist > 3)
                        Enemy.MoveForward(step); // move fwd
                    if (Services.Run.Player.Character.Dying || dist > 20)
                        Controller.ChangeState(PatrolState.KEY);
                    else if (dist < 3)
                    {
                        Vector3 targetDir = target - trans.position;
                        float angleBetween = Vector3.Angle(trans.forward, targetDir);
                        if (angleBetween < 45.0f)
                            Enemy.AttackSetup("");
                    }
                }                      
            }
            else if (Enemy.UnitType == Unit.UnitTypes.WaterMonster) // If this enemy is a water monster.
            {
                if (Enemy.RangedUnitAttack != null) // Use a ranged attack.
                {
                    if (dist > 8)
                        Enemy.MoveForward(step); // move fwd
                    if (Services.Run.Player.Character.Dying || dist > 20)
                        Controller.ChangeState(PatrolState.KEY);
                    else if (dist < 8)
                    {
                        Vector3 targetDir = target - trans.position;
                        float angleBetween = Vector3.Angle(trans.forward, targetDir);
                        if (angleBetween < 30.0f)
                            Enemy.AttackSetup("");                
                    }
                }
                else if (Enemy.MeleeUnitAttack != null) // Use a melee attack.
                {
                    if (dist > 3)
                        Enemy.MoveForward(step); // move fwd
                    if (Services.Run.Player.Character.Dying || dist > 20)
                        Controller.ChangeState(PatrolState.KEY);
                    else if (dist < 3)
                    {
                        Vector3 targetDir = target - trans.position;
                        float angleBetween = Vector3.Angle(trans.forward, targetDir);
                        if (angleBetween < 60.0f)
                            Enemy.AttackSetup("");
                    }
                }
            }
            else if (Enemy.UnitType == Unit.UnitTypes.Demon) // If this enemy is a demon.
            {
                if (dist > 50)
                    Enemy.MoveForward(step); // move fwd
                if (Services.Run.Player.Character.Dying || dist > 60)
                    Controller.ChangeState(PatrolState.KEY);
                else if (dist < 50)
                {
                    if (dist < 25) // Use a melee attack.
                    {
                        Vector3 targetDir = target - trans.position;
                        float angleBetween = Vector3.Angle(trans.forward, targetDir);
                        if (angleBetween < 20.0f)
                            Enemy.AttackSetup("Punch");
                    }
                    else // Use a ranged attack.
                    {
                        Vector3 targetDir = target - trans.position;
                        float angleBetween = Vector3.Angle(trans.forward, targetDir);
                        if (angleBetween < 15.0f)
                            Enemy.AttackSetup("Fireball");
                    }
                }
            }
        }

        // use these for changing state
        public const string KEY = "AttackState";
        public override string GetKey()
        {
            return KEY;
        }
    }
}
