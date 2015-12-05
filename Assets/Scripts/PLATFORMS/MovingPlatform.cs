using UnityEngine;
using System.Collections;

// @author  Ryan
// @date    04 Dec 2015
namespace AdventureFVTC {
    public class MovingPlatform:MonoBehaviour {
        private Vector3 positionA;
        private Vector3 positionB;
        private bool waitingAtPosition = true;
        private bool moveToPointB = false;
        private float timeWaited = 0.0f;
        private float currentMoveTime = 0.0f;

        [SerializeField] protected float timeToMove = 3.0f; // The seconds it takes the platform to move from A to B.
        [SerializeField] protected float waitingTime = 2.0f; // The time the platform waits before moving again.  
        [SerializeField] protected Vector3 moveDistance = Vector3.zero; // The distance the platform will move from its start position.

        protected void Start()
        {
            positionA = transform.position;
            positionB = transform.position + moveDistance;
        }

        protected void Update()
        {
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

            if (moveToPointB && !waitingAtPosition)
            {
                //increment timer once per frame
                currentMoveTime += Time.deltaTime;
                if (currentMoveTime > timeToMove)
                {
                    currentMoveTime = timeToMove;
                }

                //lerp!
                float perc = currentMoveTime / timeToMove;
                transform.position = Vector3.Lerp(positionA, positionB, perc);

                if (transform.position == positionB)
                {
                    waitingAtPosition = true;
                }
            }

            if (!moveToPointB && !waitingAtPosition)
            {
                //increment timer once per frame
                currentMoveTime += Time.deltaTime;
                if (currentMoveTime > timeToMove)
                {
                    currentMoveTime = timeToMove;
                }

                //lerp!
                float perc = currentMoveTime / timeToMove;
                transform.position = Vector3.Lerp(positionB, positionA, perc);

                if (transform.position == positionA)
                {
                    waitingAtPosition = true;
                }
            }
        }         
    }
}
