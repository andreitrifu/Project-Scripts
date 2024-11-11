using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class AIStateMachine : MonoBehaviour
{
    public enum AIState { Idle, MoveToPoint, MoveToEnemy }
    NavMeshAgent myAgent;
    private AIState currentState;

    public List<Transform> movePoints;
    private Transform currentMovePoint;
    private DetectClosest detectClosest;

    public float minEnemyDistance = 20f;
    public float maxEnemyDistance = 150f;
    public float minPointDistance = 10f;
    public float maxPointDistance = 400f;

    void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
        detectClosest = GetComponent<DetectClosest>();
        currentState = AIState.MoveToPoint;

        InitializeMovePoints();
    }

    void FixedUpdate()
    {
        HandleState();
        CheckStateTransitions();
    }

    private void InitializeMovePoints()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("VictoryPoints"))
        {
            movePoints.Add(obj.transform);
        }
    }

    private void HandleState()
    {
        switch (currentState)
        {
            case AIState.Idle:
                StopAgent();
                break;
            case AIState.MoveToPoint:
                HandleMoveToPoint();
                break;
            case AIState.MoveToEnemy:
                HandleMoveToEnemy();
                break;
        }
    }

    private void StopAgent()
    {
        myAgent.ResetPath();
    }

    private void HandleMoveToPoint()
    {
        if (currentMovePoint == null || IsAtPosition(currentMovePoint.position, minPointDistance))
        {
            currentMovePoint = FindRandomMovePoint();

            if (currentMovePoint != null)
            {
                myAgent.SetDestination(currentMovePoint.position);
            }
            else
            {
                ChangeState(AIState.Idle);
            }
        }
    }

    private void HandleMoveToEnemy()
    {
        Transform enemy = detectClosest.closestEnemy?.transform;
        if (enemy != null)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.position);
            if (distanceToEnemy <= maxEnemyDistance && distanceToEnemy > minEnemyDistance)
            {
                myAgent.SetDestination(enemy.position);
            }
            else
            {
                ChangeState(AIState.MoveToPoint);
            }
        }
        else
        {
            ChangeState(AIState.MoveToPoint);
        }
    }

    private void CheckStateTransitions()
    {
        if (currentState != AIState.MoveToEnemy && ShouldMoveToEnemy())
        {
            ChangeState(AIState.MoveToEnemy);
        }
        else if (currentState != AIState.MoveToPoint && ShouldMoveToPoint())
        {
            ChangeState(AIState.MoveToPoint);
        }
        else if (ShouldIdle())
        {
            ChangeState(AIState.Idle);
        }
    }

    private bool ShouldMoveToEnemy()
    {
        if (detectClosest.closestEnemy != null)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, detectClosest.closestEnemy.transform.position);
            return distanceToEnemy < maxEnemyDistance && distanceToEnemy > minEnemyDistance;
        }
        return false;
    }

    private bool ShouldMoveToPoint()
    {
        return currentMovePoint != null &&
               IsInRange(currentMovePoint.position, minPointDistance, maxPointDistance) &&
               (detectClosest.closestEnemy == null ||
                Vector3.Distance(transform.position, detectClosest.closestEnemy.transform.position) > maxEnemyDistance);
    }

    private bool ShouldIdle()
    {
        return (currentMovePoint != null && IsAtPosition(currentMovePoint.position, minPointDistance)) ||
               (detectClosest.closestEnemy != null &&
                Vector3.Distance(transform.position, detectClosest.closestEnemy.transform.position) <= minEnemyDistance);
    }

    public void ChangeState(AIState newState)
    {
        currentState = newState;
    }

    private bool IsAtPosition(Vector3 position, float minDistance)
    {
        return Vector3.Distance(transform.position, position) < minDistance;
    }

    private bool IsInRange(Vector3 position, float minDistance, float maxDistance)
    {
        float distance = Vector3.Distance(transform.position, position);
        return distance >= minDistance && distance <= maxDistance;
    }

    private Transform FindRandomMovePoint()
    {
        List<Transform> availablePoints = new List<Transform>();

        foreach (Transform point in movePoints)
        {
            ControlArea controlArea = point.GetComponent<ControlArea>();
            if (controlArea == null || (!controlArea.team1Taken && !controlArea.team2Taken))
            {
                availablePoints.Add(point);
            }
        }

        if (availablePoints.Count > 0)
        {
            int randomIndex = Random.Range(0, availablePoints.Count);
            return availablePoints[randomIndex];
        }

        return null;
    }

    // Public method to move to a specific point
    public void MoveToPoint(Vector3 point)
    {
        currentMovePoint = null;  // Clear current target to reset movement
        ChangeState(AIState.MoveToPoint);
        myAgent.SetDestination(point);
    }

    // Public method to move towards an enemy
    public void MoveToEnemy(Transform enemyTransform)
    {
        ChangeState(AIState.MoveToEnemy);
        myAgent.SetDestination(enemyTransform.position);
    }
}
