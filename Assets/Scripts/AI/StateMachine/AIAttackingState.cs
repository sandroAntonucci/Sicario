using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAttackingState : AIBaseState
{

    private BaseGun gun;

    public override void EnterState(AIHandler handler)
    {

        handler.GetComponent<NavMeshAgent>().isStopped = true;

        gun = handler.GetComponentInChildren<BaseGun>();

        gun.StartShooting();
    }

    public override void UpdateState(AIHandler handler)
    {
        bool isInRange = handler.IsInAttackRange();
        bool isInVision = handler.IsPlayerInVision();
        GameObject playerObject = handler.GetPlayerObject();

        if (!isInRange && !isInVision)
        {
            gun.StopShooting();
            handler.ChangeState(handler.chasingState);
        }
        else
        {
            Vector3 directionTowardsPlayer = (playerObject.transform.position - handler.transform.position).normalized;
            handler.RotateTowardsPlayer(directionTowardsPlayer);
        }

    }

    public override void ExitState(AIHandler handler)
    {
        handler.GetComponent<NavMeshAgent>().isStopped = false;
        gun.StopShooting();
    }
}