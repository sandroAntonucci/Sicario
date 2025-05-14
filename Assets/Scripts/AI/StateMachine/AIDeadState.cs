using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;


public class AIDeadState : AIBaseState
{
    public override void EnterState(AIHandler handler)
    {
        handler._animator.SetBool("isDead", true);
        // gotta remove that nasty AI from the region agents :>
        handler.currentNodeRegion.agentsInRegion.Remove(handler);
        //handler.DestroyObject(handler.gameObject);

        // Adds force to the rigidbody

        handler.GetComponent<NavMeshAgent>().enabled = false;

        handler.isDead = true;

        GameObject scoreEffect = MonoBehaviour.Instantiate(handler.scorePointsEffect, handler.transform.position, Quaternion.identity);

        scoreEffect.GetComponentInChildren<TextMeshPro>().text = handler.damageReceived.ToString();

        Rigidbody rb = handler.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.None;
            rb.useGravity = true;
            rb.isKinematic = true;
        }

        handler.weapon.DropWeapon();
        handler.weapon.SetUpPlayerWeapon();
        ScoreSystem.Instance.CheckDeadEnemies();
    }

    public override void UpdateState(AIHandler handler)
    {

    }

    public override void ExitState(AIHandler handler)
    {

    }
}
