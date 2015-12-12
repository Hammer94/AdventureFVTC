using UnityEngine;

// @author  Ryan
// @date    12 Dec 2015
namespace AdventureFVTC {
    public class DyingState:State {
        private Transform trans;
        private Transform playerTrans;
        private bool lookedAt = false;

        public float Speed { get; set; }    // patrol speed

        public DyingState(StateController controller)
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
            target.y = trans.position.y;

            Debug.Log("Dying!");

            if (!lookedAt)
            {
                lookedAt = true;
                trans.LookAt(target);
            }         
        }

        // use these for changing state
        public const string KEY = "DyingState";
        public override string GetKey()
        {
            return KEY;
        }
    }
}
