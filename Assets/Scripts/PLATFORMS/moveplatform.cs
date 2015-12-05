using UnityEngine;

// @author  Ryan
// @date    04 Dec 2015
namespace AdventureFVTC {
    public class moveplatform : MonoBehaviour {
        private Vector3 positionA;
        private Vector3 positionB;
        private bool waitingAtPosition = true;     
        private bool moveToPointB = true;
        private Vector3 spaceMoved = Vector3.zero;
        private Vector3 spaceAllowedToMove = Vector3.zero;
        private float timeWaited = 0.0f;
        private bool xReached = false;
        private bool yReached = false;
        private bool zReached = false;

        [SerializeField] protected float timeToMove = 3.0f; // The seconds it takes the platform to move from A to B.
        [SerializeField] protected float waitingAtPositionTime = 2.0f; // The time the platform waits before moving again.  
        [SerializeField] protected float xRelativePosition = 0.0f; // The x distance the platform will move from its start position.
        [SerializeField] protected float yRelativePosition = 5.0f; // The y distance the platform will move from its start position.
        [SerializeField] protected float zRelativePosition = 0.0f; // The z distance the platform will move from its start position.

        void Start()
        {
            // Set position A as the position the platform starts at.
            positionA = transform.position;
            positionB = new Vector3(positionA.x + xRelativePosition, positionA.y + yRelativePosition, positionA.z + zRelativePosition);

            // Get the whole values for the amount of space between the two positions on  the x axis.
            if ((positionB.x - positionA.x) < 0)
                spaceAllowedToMove.x = -1 * (positionB.x - positionA.x);
            else
                spaceAllowedToMove.x = positionB.x - positionA.x;
            // Get the whole values for the amount of space between the two positions on  the y axis.
            if ((positionB.y - positionA.y) < 0)
                spaceAllowedToMove.y = -1 * (positionB.y - positionA.y);
            else
                spaceAllowedToMove.y = positionB.y - positionA.y;
            // Get the whole values for the amount of space between the two positions on  the z axis.
            if ((positionB.z - positionA.z) < 0)
                spaceAllowedToMove.z = -1 * (positionB.z - positionA.z);
            else 
                spaceAllowedToMove.z = positionB.z - positionA.z;

            Debug.Log(positionA);
            Debug.Log(spaceAllowedToMove);
        }

        void Update() {
            if (waitingAtPosition)
                timeWaited += Time.deltaTime;
            
            if (timeWaited >= waitingAtPositionTime) {
                timeWaited = 0.0f;
                spaceMoved = new Vector3(0, 0, 0); // Reset the amount moved.
                xReached = false;
                yReached = false;
                zReached = false;
                waitingAtPosition = false; // Start the platform's movement.
            }

            if (!waitingAtPosition) {
                if (moveToPointB) {
                    // Get the rate of the platform moving per update equal to the difference between the two positions divided by the time
                    // it should take for the movement to complete, multiplied by the time since last update.
                    float xRate;
                    float yRate;
                    float zRate;

                    // Get the x, y, and z position increase rates per frame.
                    // If amount the y position needs to move is zero.
                    if (spaceAllowedToMove.x == 0) {
                        xRate = 0; // Set the rate of change to zero.
                        xReached = true; // Signal that the position has been reached.
                    }
                    else
                        xRate = Mathf.Min((positionB.x - positionA.x) / timeToMove * Time.deltaTime, spaceAllowedToMove.x);
                    // If amount the z position needs to move is zero.
                    if (spaceAllowedToMove.y == 0) {
                        yRate = 0; // Set the rate of change to zero.
                        yReached = true; // Signal that the position has been reached.
                    }
                    else
                        yRate = Mathf.Min((positionB.y - positionA.y) / timeToMove * Time.deltaTime, spaceAllowedToMove.y);
                    // If amount the x position needs to move is zero.
                    if (spaceAllowedToMove.z == 0) {
                        zRate = 0; // Set the rate of change to zero.
                        zReached = true; // Signal that the position has been reached.
                    }
                    else
                        zRate = Mathf.Min((positionB.z - positionA.z) / timeToMove * Time.deltaTime, spaceAllowedToMove.z);

                    // Get the whole values for the amount moved on each axis.
                    if (xRate < 0)
                        spaceMoved.x += -1 * xRate;
                    else
                        spaceMoved.x += xRate;
                    if (yRate < 0)
                       spaceMoved.y += -1 * yRate;
                    else           
                        spaceMoved.y += yRate;             
                    if (zRate < 0)                
                        spaceMoved.z += -1 * zRate;                  
                    else                  
                        spaceMoved.z += zRate;

                    // If the space moved has reached or surpassed the space allowed to move.
                    if (spaceMoved.x >= spaceAllowedToMove.x) {
                        xRate -= (spaceMoved.x - spaceAllowedToMove.x); // Shrink the axis rate.  
                        xReached = true;
                    }
                    if (spaceMoved.y >= spaceAllowedToMove.y) {
                        yRate -= (spaceMoved.y - spaceAllowedToMove.y); // Shrink the axis rate. 
                        yReached = true;
                    }
                    if (spaceMoved.z >= spaceAllowedToMove.z) {
                        zRate -= (spaceMoved.z - spaceAllowedToMove.z); // Shrink the axis rate. 
                        zReached = true;
                    }

                    // If the rates are zero, which will happen when the platform no longer needs to move on this axis.
                    if (xReached && yReached && zReached) {
                        moveToPointB = false; // Alternate the movement.
                        waitingAtPosition = true; // Start the waiting period.
                        Debug.Log(spaceMoved);
                    }
                  
                    float positionX = transform.position.x + xRate;
                    float positionY = transform.position.y + yRate;
                    float positionZ = transform.position.z + zRate;
                    Vector3 newPosition = new Vector3(positionX, positionY, positionZ);
                    // Move the platform to the next step.
                    transform.position = newPosition;
                }
                // Else move to pointA.
                else {
                    // Get the rate of the platform moving per update equal to the difference between the two positions divided by the time
                    // it should take for the movement to complete, multiplied by the time since last update.
                    float xRate;
                    float yRate;
                    float zRate;

                    // Get the x, y, and z position increase rates per frame.
                    // If amount the y position needs to move is zero.
                    if (spaceAllowedToMove.x == 0) {
                        xRate = 0; // Set the rate of change to zero.
                        xReached = true; // Signal that the position has been reached.
                    }
                    else
                        xRate = Mathf.Min((positionA.x - positionB.x) / timeToMove * Time.deltaTime, spaceAllowedToMove.x);
                    // If amount the z position needs to move is zero.
                    if (spaceAllowedToMove.y == 0) {
                        yRate = 0; // Set the rate of change to zero.
                        yReached = true; // Signal that the position has been reached.
                    }
                    else
                        yRate = Mathf.Min((positionA.y - positionB.y) / timeToMove * Time.deltaTime, spaceAllowedToMove.y);
                    // If amount the x position needs to move is zero.
                    if (spaceAllowedToMove.z == 0) {
                        zRate = 0; // Set the rate of change to zero.
                        zReached = true; // Signal that the position has been reached.
                    }
                    else
                        zRate = Mathf.Min((positionA.z - positionB.z) / timeToMove * Time.deltaTime, spaceAllowedToMove.z);

                    // Get the whole values for the amount moved on each axis.
                    if (xRate < 0)
                        spaceMoved.x += -1 * xRate;
                    else
                        spaceMoved.x += xRate;
                    if (yRate < 0)
                        spaceMoved.y += -1 * yRate;
                    else
                        spaceMoved.y += yRate;
                    if (zRate < 0)
                        spaceMoved.z += -1 * zRate;
                    else
                        spaceMoved.z += zRate;

                    // If the space moved has reached or surpassed the space allowed to move.
                    if (spaceMoved.x >= spaceAllowedToMove.x) { 
                        xRate -= (spaceMoved.x - spaceAllowedToMove.x); // Shrink the axis rate.  
                        xReached = true;
                    }
                    if (spaceMoved.y >= spaceAllowedToMove.y) { 
                        yRate -= (spaceMoved.y - spaceAllowedToMove.y); // Shrink the axis rate. 
                        yReached = true;
                    }
                    if (spaceMoved.z >= spaceAllowedToMove.z) {
                        zRate -= (spaceMoved.z - spaceAllowedToMove.z); // Shrink the axis rate. 
                        zReached = true;
                    }

                    // If the rates are zero, which will happen when the platform no longer needs to move on this axis.
                    if (xReached && yReached && zReached) {
                        moveToPointB = true; // Alternate the movement.
                        waitingAtPosition = true; // Start the waiting period.
                        Debug.Log(spaceMoved);
                    }

                    float positionX = transform.position.x + xRate;
                    float positionY = transform.position.y + yRate;
                    float positionZ = transform.position.z + zRate;
                    // Move the platform to the next step.
                    transform.position = new Vector3(positionX, positionY, positionZ);
                }                                   
            }           
        }
    }
}