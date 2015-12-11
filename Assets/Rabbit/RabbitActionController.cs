using UnityEngine;
using System.Collections;

namespace AdventureFVTC {
    [RequireComponent(typeof(Animator))]
    public class RabbitActionController : MonoBehaviour {
        private Animator animator;
        private CharacterController controller;

        private int hashHit = Animator.StringToHash("Base Layer.Hit");
        private int hashDead = Animator.StringToHash("Base Layer.Dead");
        private int hashWalk = Animator.StringToHash("Base Layer.Walk");
        private int hashJump = Animator.StringToHash("Base Layer.Jump");
        private int hashPick = Animator.StringToHash("Base Layer.Pick");
        private int hashPunch = Animator.StringToHash("Base Layer.Punch");

        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
            controller = GetComponent<CharacterController>();
        }

        void OnGUI()
        {
            // Is the player is allowed to move / attack?
            if (Services.Input.AllowPlayerInput)
            {
                if (Input.GetKeyDown(KeyCode.P))
                {
                    if (Services.Run.Player.Character.CurrentAttackInterval == 0)
                    {
                        animator.Play(hashPunch);
                        Services.Run.Player.Character.AttackSetup("Punch");
                    }
                }

                if (Input.GetKeyDown(KeyCode.O))
                {
                    if (Services.Run.Player.Character.CurrentAttackInterval == 0 && PersistentPlayerStats.HasSnowBall)
                    {
                        animator.Play(hashPunch);
                        Services.Run.Player.Character.AttackSetup("Snowball");
                    }
                }

                if (Services.Run.Player.Character.Health == 0)
                {
                    Services.Input.AllowPlayerInput = false;
                    animator.Play(hashDead);
                    if (Services.Run.Player.Character.Dead)
                        animator.Play(hashWalk);                                   
                }

                //if (Input.GetKeyDown(KeyCode.U))
                //{
                //    animator.Play(hashHit);
                //}

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    animator.Play(hashJump);
                }
            }      
        }

        // Update is called once per frame
        void Update()
        {



            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxis("Horizontal");
            bool move = (v != 0.0f || h != 0.0f);

            animator.speed = move ? 2.0f : 1.0f;


            animator.SetFloat("Speed", move ? 1.0f : 0.0f);


        }

        void OnAnimatorMove()
        {


        }
    }
}