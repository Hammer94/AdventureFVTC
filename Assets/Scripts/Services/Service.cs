namespace AdventureFVTC {
    /**
     * A base service class for AdventureFVTC. Allows the creation
     * of services that handle various tasks that not a part of
     * the other classes.
     * Includes base start, update, fixedUpdate, Start, Update,
     * and FixedUpdate methods that all services will inherit.
     * 
     * @author  Ryan
     * @date    26 Nov 2015
     */
    public abstract class Service {
        private bool enabled = true;

        public virtual bool Enabled {
            get {
                return enabled;
            }
            set {
                enabled = value;
                start();
            }
        }

        public void start() {
            if (enabled)
                Start();
        }

        public void update() {
            if (enabled)
                Update();
        }

        public void fixedUpdate() {
            if (enabled)
                FixedUpdate();
        }

        protected virtual void Start() {

        }

        protected virtual void Update() {

        }

        protected virtual void FixedUpdate() {

        }
    }
}
