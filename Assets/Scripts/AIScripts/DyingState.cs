using UnityEngine;

// @author  Ryan
// @date    12 Dec 2015
namespace AdventureFVTC {
    public class DieState:State {
        public float Speed { get; set; }    // patrol speed

        public DieState(StateController controller)
            : base(controller) // pass the controller to the base class
        {
           
        }

        public override void Update(float deltaTime) {
             
        }

        // use these for changing state
        public const string KEY = "DyingState";
        public override string GetKey()
        {
            return KEY;
        }
    }
}
