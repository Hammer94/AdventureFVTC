using UnityEngine;

// @author  Ryan
// @date    06 Dec 2015
namespace AdventureFVTC {
    public class MovingPlatform : MonoBehaviour {
        private Vector3 positionA;
        private Vector3 positionB;
        private bool waitingAtPosition = true;
        private bool moveToPointB = false;
        private float timeWaited = 0.0f;
        private float currentMoveTime = 0.0f;
        private bool playerIsOnPlatform = false;
        private Vector3 oldPosition = Vector3.zero;
        private Vector3 newPositon = Vector3.zero;

        [SerializeField] protected float timeToMove = 3.0f; // The seconds it takes the platform to move from A to B.
        [SerializeField] protected float waitingTime = 2.0f; // The time the platform waits before moving again.  
        [SerializeField] protected Vector3 moveDistance = Vector3.zero; // The distance the platform will move from its start position.

        public bool PlayerIsOnPlatform {
            get {
                return playerIsOnPlatform;
            }
            set {
                playerIsOnPlatform = value;
            }
        }

        void Start() {
            positionA = transform.position;
            positionB = transform.position + moveDistance;        
        }

        void Update() {
            if (waitingAtPosition)
            {
                timeWaited += Time.deltaTime;

                if (timeWaited >= waitingTime)
                {
                    timeWaited = 0.0f;
                    currentMoveTime = 0.0f;
                    moveToPointB = !moveToPointB;
                    waitingAtPosition = false;
                }
            }

            if (!waitingAtPosition) {
                // Get the old position before stepping.
                oldPosition = transform.position;

                if (moveToPointB) {
                    //increment timer once per frame
                    currentMoveTime += Time.deltaTime;
                    if (currentMoveTime > timeToMove)
                        currentMoveTime = timeToMove;

                    //lerp!
                    float perc = currentMoveTime / timeToMove;
                    transform.position = Vector3.Lerp(positionA, positionB, perc);
                    // Get the new position after stepping.
                    newPositon = transform.position;

                    if (transform.position == positionB)
                        waitingAtPosition = true;
                }

                if (!moveToPointB) {
                    //increment timer once per frame
                    currentMoveTime += Time.deltaTime;
                    if (currentMoveTime > timeToMove)
                        currentMoveTime = timeToMove;

                    //lerp!
                    float perc = currentMoveTime / timeToMove;
                    transform.position = Vector3.Lerp(positionB, positionA, perc);
                    // Get the new position after stepping.
                    newPositon = transform.position;

                    if (transform.position == positionA)
                        waitingAtPosition = true;
                }

                // If the player is standing on this platform.
                if (playerIsOnPlatform) {
                    // Get the amount the platform has just moved.
                    Vector3 difference = newPositon - oldPosition;
                    // Add the amount to the player's current position.
                    Vector3 newCharacterPosition = Services.Run.Player.Character.transform.position + difference;
                    // Move the character to their new position.
                    Services.Run.Player.Character.transform.position = newCharacterPosition;
                }
            }         
        }      
    }
}