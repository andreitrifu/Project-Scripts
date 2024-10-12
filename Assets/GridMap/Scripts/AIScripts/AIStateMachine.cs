using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class AIStateMachine : MonoBehaviour
{
    public enum AIState { Idle, MoveToPoint, MoveToEnemy }
    NavMeshAgent myAgent;
    private AIState currentState;

    public List<Transform> movePoints; // List of points to move to
    private Transform currentMovePoint; // Current move point

    public float minEnemyDistance = 20f;
    public float maxEnemyDistance = 150f;
    public float minPointDistance = 10f;
    public float maxPointDistance = 400f;

    void Start()
    {
        // Set initial state
        myAgent = GetComponent<NavMeshAgent>();
        currentState = AIState.MoveToPoint;
        currentMovePoint = null;

        // Find and assign objects with the "VictoryPoints" tag to the movePoints list
        GameObject[] victoryPointObjects = GameObject.FindGameObjectsWithTag("VictoryPoints");
        foreach (GameObject obj in victoryPointObjects)
        {
            movePoints.Add(obj.transform);
        }
    }

    void FixedUpdate()
    {
        // Update current state
        switch (currentState)
        {
            case AIState.Idle:
                // Stop the agent from moving
                myAgent.ResetPath();
                break;

            case AIState.MoveToPoint:
                // Check if the current move point is null
                if (currentMovePoint == null || Vector3.Distance(transform.position, currentMovePoint.position) < minPointDistance)
                {
                    // Find a random move point not controlled by any team
                    currentMovePoint = FindRandomMovePoint();

                    // If a move point is found, move to it
                    if (currentMovePoint != null)
                    {
                        myAgent.SetDestination(currentMovePoint.position);
                    }
                    else
                    {
                        currentState = AIState.Idle; // No move points available, go back to idle
                    }
                }
                break;


            case AIState.MoveToEnemy:
                // Check if enemy is within range
                float distanceToEnemy = Vector3.Distance(transform.position, GetComponent<DetectClosest>().closestEnemy.transform.position);
                if (distanceToEnemy < maxEnemyDistance && distanceToEnemy > minEnemyDistance)
                {
                    // Move towards the enemy
                    myAgent.SetDestination(GetComponent<DetectClosest>().closestEnemy.transform.position);
                }
                else
                {
                    // Enemy out of range, switch to MoveToPoint
                    currentState = AIState.MoveToPoint;
                }
                break;
        }

        // Check conditions for moving to enemy
        if (currentState != AIState.MoveToEnemy && GetComponent<DetectClosest>().closestEnemy != null)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, GetComponent<DetectClosest>().closestEnemy.transform.position);
            if (distanceToEnemy < maxEnemyDistance && distanceToEnemy > minEnemyDistance)
            {
                // Change state to MoveToEnemy
                ChangeState(AIState.MoveToEnemy);
            }
        }

        // Check conditions for moving to point
        if (currentState != AIState.MoveToPoint && currentMovePoint != null)
        {
            float distanceToPoint = Vector3.Distance(transform.position, currentMovePoint.position);
            float distanceToEnemy = Vector3.Distance(transform.position, GetComponent<DetectClosest>().closestEnemy.transform.position);
            if (distanceToPoint < maxPointDistance && distanceToPoint > minPointDistance && distanceToEnemy > maxEnemyDistance)
            {
                // Change state to MoveToPoint
                ChangeState(AIState.MoveToPoint);
            }
        }
        if (currentState != AIState.MoveToPoint && currentState != AIState.MoveToEnemy)
        {
            float distanceToPoint = Vector3.Distance(transform.position, currentMovePoint.position);
            float distanceToEnemy = Vector3.Distance(transform.position, GetComponent<DetectClosest>().closestEnemy.transform.position);
            if (distanceToPoint <= minPointDistance ||distanceToEnemy <= minEnemyDistance)
            {
                ChangeState(AIState.Idle);
            }
        }
    }

    public void ChangeState(AIState newState)
    {
        currentState = newState;
    }

    // Method to find the closest move point that is not taken by any team
    private Transform FindClosestMovePoint()
    {
        Transform closestPoint = null;
        float closestDistance = Mathf.Infinity;
        int indexOfClosestPoint = -1;

        for (int i = 0; i < movePoints.Count; i++)
        {
            Transform point = movePoints[i];
            ControlArea controlArea = point.GetComponent<ControlArea>();

            if (controlArea == null || (!controlArea.team1Taken && !controlArea.team2Taken))
            {
                float distance = Vector3.Distance(transform.position, point.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPoint = point;
                    indexOfClosestPoint = i;
                }
            }
        }

        // Remove the closest point if it's found and taken
        if (closestPoint != null && (closestPoint.GetComponent<ControlArea>()?.team1Taken ?? false) || (closestPoint.GetComponent<ControlArea>()?.team2Taken ?? false))
        {
            movePoints.RemoveAt(indexOfClosestPoint);
        }

        return closestPoint;
    }

    // Public method to move to a specific point
    public void MoveToPoint(Vector3 point)
    {
        // Move towards the specified point
        myAgent.SetDestination(point);
    }

    // Public method to move towards an enemy
    public void MoveToEnemy(Transform enemyTransform)
    {
        // Move towards the enemy
        myAgent.SetDestination(enemyTransform.position);
    }
    // Method to find a random move point that is not taken by any team
    private Transform FindRandomMovePoint()
    {
        List<Transform> availablePoints = new List<Transform>();

        // Filter out points controlled by any team
        foreach (Transform point in movePoints)
        {
            ControlArea controlArea = point.GetComponent<ControlArea>();
            if (controlArea == null || (!controlArea.team1Taken && !controlArea.team2Taken))
            {
                availablePoints.Add(point);
            }
        }

        // If there are available points, select a random one
        if (availablePoints.Count > 0)
        {
            int randomIndex = Random.Range(0, availablePoints.Count);
            return availablePoints[randomIndex];
        }

        return null; // No available points
    }

}
