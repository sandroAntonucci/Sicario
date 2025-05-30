using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.Mathematics;
using UnityEngine;

public class AIHandler : MonoBehaviour, IDamageable
{
    public Animator _animator;
    public int health = 100;
    public BaseWeapon weapon;
    public float fieldOfView = 30.0f;
    public float distanceOfView = 5;
    public EnemyType enemyType;
    public float rangedEnemyDistance = 8;
    public float meleeEnemyDistance = 2;

    public bool isDead = false;

    public int damageReceived = 0;

    public AIBaseState currentAiState; 
    public AIIdleState idleState = new AIIdleState();
    public AIAwareState awareState = new AIAwareState();
    public AIChasingState chasingState = new AIChasingState();
    public AIAttackingState attackingState = new AIAttackingState();
    public AIStunnedState stunnedState = new AIStunnedState();
    public AIDeadState deadState = new AIDeadState();

    public NodeRegion currentNodeRegion;
    public Node currentNode;

    public GameObject scorePointsEffect;

    void Start()
    {
        _animator = transform.GetChild(0).GetComponent<Animator>();
        weapon = GetComponentInChildren<BaseWeapon>();

        // change that sweet sweet animator branch
        switch (enemyType)
        {
            case EnemyType.Melee:
                // nothing?
                break;
            case EnemyType.Pistol:
                _animator.SetBool("isPistol", true);
                break;
            case EnemyType.Rifle:
                _animator.SetBool("isRifle", true);
                break;
        }

        currentAiState = idleState;
        currentAiState.EnterState(this);
    }

    void Update()
    {
        currentAiState.UpdateState(this);
    }

    public void DealDamage(int amount, string gunName = "", bool silent = false)
    {

        if (health > 0)
            ScoreSystem.Instance.TriggerAwardPointsEvent(amount, gunName);
            damageReceived = amount;

        health -= amount;

        if (!silent && currentAiState == idleState)
        {
            ChangeState(awareState);
        }
        
        if (health <= 0 && currentAiState != deadState)
        {
            ChangeState(deadState);
        }
    }

    public void DestroyObject(GameObject obj)
    {
        Destroy(obj);
    }

    public void ChangeState(AIBaseState newState)
    {
        currentAiState.ExitState(this);
        currentAiState = newState;
        newState.EnterState(this);
    }

    public void RotateTowardsPlayer(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
    }

    public GameObject GetPlayerObject()
    {
        return GameObject.FindGameObjectWithTag("Player");
    }

    public bool IsPlayerInVision()
    {
        // Debug AI fov.
        Debug.DrawRay(transform.position, Quaternion.Euler(0, -fieldOfView / 2, 0) * transform.forward * distanceOfView, Color.blue);
        Debug.DrawRay(transform.position, Quaternion.Euler(0, fieldOfView / 2, 0) * transform.forward * distanceOfView, Color.blue);

        GameObject playerObject = GetPlayerObject();
        if (playerObject == null)
            return false;

        Vector3 directionTowardsPlayer = (playerObject.transform.position - transform.position).normalized;

        float angle = Vector3.Angle(transform.forward, directionTowardsPlayer);

        if (angle < fieldOfView / 2)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerObject.transform.position);
            if (distanceToPlayer < distanceOfView)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, directionTowardsPlayer, out hit, distanceOfView))
                {
                    Debug.DrawRay(transform.position, playerObject.transform.position, Color.red);
                    if (hit.collider.CompareTag("Player"))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public bool IsInAttackRange()
    {
        GameObject playerObject = GetPlayerObject();
        if (playerObject == null)
            return false;

        float distanceToPlayer = Vector3.Distance(transform.position, playerObject.transform.position);
        if (enemyType == EnemyType.Melee && distanceToPlayer < meleeEnemyDistance)
            return true;
        else if ((enemyType == EnemyType.Rifle || enemyType == EnemyType.Pistol) && distanceToPlayer < rangedEnemyDistance)
            return true;
        
        return false;
    }
}
