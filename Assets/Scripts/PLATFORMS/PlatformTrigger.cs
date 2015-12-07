using UnityEngine;

// @author  Ryan
// @date    06 Dec 2015
namespace AdventureFVTC {
    public class PlatformTrigger:MonoBehaviour {
        private MovingPlatform platform;

        void Start() {
            platform = GetComponentInParent<MovingPlatform>();
        }

        void OnTriggerEnter(Collider obj) {
            if (obj.tag == "Player") {
                platform.PlayerIsOnPlatform = true;
            }
        }

        void OnTriggerExit(Collider obj) {
            if (obj.tag == "Player") {
                platform.PlayerIsOnPlatform = false;
            }
        }
    }
}
