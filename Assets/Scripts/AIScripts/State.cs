using UnityEngine;
using System.Collections;

namespace AdventureFVTC {
    public abstract class State
    {

        // ABSTRACT CLASS -- all states MUST have these things!

        protected StateController Controller { get; private set; }

        // easy access to the game object
        protected GameObject GameObject { get { return Controller.gameObject; } }
        protected Enemy Enemy { get; set; }

        // constructor (states must have a controller)
        public State(StateController controller)
        {
            Controller = controller;
        }

        public abstract string GetKey();
        public abstract void Update(float deltaTime);

    }
}
