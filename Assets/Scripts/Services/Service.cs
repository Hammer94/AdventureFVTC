namespace AdventureFVTC {
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
