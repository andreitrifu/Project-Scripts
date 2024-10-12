using UnityEngine;

public class AIScript : MonoBehaviour
{
    private AIStateMachine stateMachine;

    void Start()
    {
        stateMachine = GetComponent<AIStateMachine>();
    }

    void FixedUpdate()
    {

    }

    void MoveToPoint(Vector3 point)
    {
        stateMachine.ChangeState(AIStateMachine.AIState.MoveToPoint);
        stateMachine.MoveToPoint(point);
    }

    void MoveToEnemy(Transform enemyTransform)
    {
        stateMachine.ChangeState(AIStateMachine.AIState.MoveToEnemy);
        stateMachine.MoveToEnemy(enemyTransform);
    }
}
