using UnityEngine;

namespace AdventureFVTC
{
    public class moveplatform : MonoBehaviour
    {
        private float tParam = 0.0f;
        private float xPositionA;
        private float yPositionA;
        private float zPositionA;
        private bool xMovementFinished = false;
        private bool yMovementFinished = false;
        private bool zMovementFinished = false;
        private bool waitingAtPosition = false;
        private float timeWaited = 0.0f;

        [SerializeField]
        protected float speed = 2.0f; // The seconds it takes the platform to move from A to B.
        [SerializeField]
        protected float waitingAtPositionTime = 3.0f; // The time the platform waits before moving again.
        [SerializeField]
        protected bool xPositiveMovement = true; // Will the platform moving at a positive x value?
        [SerializeField]
        protected bool yPositiveMovement = true; // Will the platform be moving at a positive y value?
        [SerializeField]
        protected bool zPositiveMovement = true; // Will the platform be moving at a positive z value?
        [SerializeField]
        protected float xRelativePosition = 20.0f; // The x distance the platform will move from its start position.
        [SerializeField]
        protected float yRelativePosition = 20.0f; // The y distance the platform will move from its start position.
        [SerializeField]
        protected float zRelativePosition = 20.0f; // The z distance the platform will move from its start position.

        void Start()
        {
            // Set position A as the position the platform starts at.
            xPositionA = transform.position.x;
            yPositionA = transform.position.y;
            zPositionA = transform.position.z;
        }

        void Update()
        {
            if (waitingAtPosition)
                timeWaited += Time.deltaTime * waitingAtPositionTime;

            if (timeWaited >= waitingAtPositionTime)
            {
                timeWaited = 0.0f;
                tParam = 0.0f;
                xMovementFinished = false;
                yMovementFinished = false;
                zMovementFinished = false;

                waitingAtPosition = false;
            }

            if (!waitingAtPosition)
            {
                if (tParam < 1)
                    tParam += Time.deltaTime * speed; //This will increment tParam by a speed multiplier.   

                // If x needs to be positive.
                if (xPositiveMovement && !xMovementFinished)
                {
                    float xLerp = Mathf.Lerp(xPositionA, xPositionA + xRelativePosition, tParam);
                    if (transform.position.x == xLerp)
                    {
                        xMovementFinished = true;
                        xPositiveMovement = false;
                    }
                }
                else if (!xPositiveMovement && !xMovementFinished)
                {
                    float xLerp = Mathf.Lerp(xPositionA + xRelativePosition, xPositionA, tParam);
                    if (transform.position.x == xLerp)
                    {
                        xMovementFinished = true;
                        xPositiveMovement = true;
                    }
                }

                // If y needs to be positive.
                if (yPositiveMovement && !yMovementFinished)
                {
                    float yLerp = Mathf.Lerp(yPositionA, yPositionA + yRelativePosition, tParam);
                    if (transform.position.y == yLerp)
                    {
                        yMovementFinished = true;
                        yPositiveMovement = false;
                    }
                }
                else if (!yPositiveMovement && !yMovementFinished)
                {
                    float yLerp = Mathf.Lerp(yPositionA + yRelativePosition, yPositionA, tParam);
                    if (transform.position.y == yLerp)
                    {
                        yMovementFinished = true;
                        yPositiveMovement = true;
                    }
                }

                // If z needs to be positive.
                if (zPositiveMovement && !zMovementFinished)
                {
                    float zLerp = Mathf.Lerp(zPositionA, zPositionA + zRelativePosition, tParam);
                    if (transform.position.z == zLerp)
                    {
                        zMovementFinished = true;
                        zPositiveMovement = false;
                    }
                }
                else if (!zPositiveMovement && !zMovementFinished)
                {
                    float zLerp = Mathf.Lerp(zPositionA + zRelativePosition, zPositionA, tParam);
                    if (transform.position.z == zLerp)
                    {
                        zMovementFinished = true;
                        zPositiveMovement = true;
                    }
                }
            }  
            
            // If the platform has finished all of its movement.
            if (xMovementFinished && yMovementFinished && zMovementFinished)
            {
                // Begin waiting before the platform moves again.
                waitingAtPosition = true;
            }
        }
    }
}