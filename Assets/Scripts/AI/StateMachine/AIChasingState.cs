using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AIChasingState : AIBaseState
{
    NavMeshAgent agent;

    private bool firstTimeDetected = false;
    private bool canAttack = false;
    private bool isRecognizing = false;

    public float recognitionTime = 0.8f;

    public override void EnterState(AIHandler handler)
    {
        agent = handler.GetComponent<NavMeshAgent>();

        if (handler.enemyType == EnemyType.Melee)
            agent.stoppingDistance = handler.meleeEnemyDistance;
        else if (handler.enemyType == EnemyType.Ranged)
            agent.stoppingDistance = handler.rangedEnemyDistance;

        if (!firstTimeDetected)
        {
            GameObject.FindGameObjectWithTag("PlayerInterface")
                .GetComponent<EnemyIndicatorManager>()
                .SpawnIndicator(handler.gameObject);

            firstTimeDetected = true;
        }

        if (!isRecognizing)
        {
            handler.StartCoroutine(StartAttackTimer(handler));
        }
    }

    private IEnumerator StartAttackTimer(AIHandler handler)
    {
        isRecognizing = true;
        canAttack = false;

        // Stop movement during recognition
        agent.isStopped = true;

        yield return new WaitForSeconds(recognitionTime);

        // Resume movement/attack after recognition
        agent.isStopped = false;
        canAttack = true;
        isRecognizing = false;
    }

    public override void UpdateState(AIHandler handler)
    {
        GameObject playerObject = handler.GetPlayerObject();
        bool isInRange = handler.IsInAttackRange();
        bool isInVision = handler.IsPlayerInVision();

        // Only update destination if not recognizing
        if (!isRecognizing && !isInRange)
        {
            agent.destination = playerObject.transform.position;
        }

        Vector3 directionTowardsPlayer = (playerObject.transform.position - handler.transform.position).normalized;
        handler.RotateTowardsPlayer(directionTowardsPlayer);

        if (isInRange && !isInVision)
        {
            agent.stoppingDistance--;
        }

        if (isInRange && isInVision && canAttack)
        {
            handler.ChangeState(handler.attackingState);
        }
    }

    public override void ExitState(AIHandler handler)
    {
        canAttack = false;
        isRecognizing = false;
        if (agent != null)
        {
            agent.isStopped = false; // Ensure movement resumes when leaving state
        }
    }
}
