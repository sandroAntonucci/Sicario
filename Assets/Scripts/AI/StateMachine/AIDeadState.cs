using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


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

        MonoBehaviour.Instantiate(handler.scorePointsEffect, handler.transform.position, Quaternion.identity);
            

        Rigidbody rb = handler.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.None;
            rb.useGravity = true;
            rb.isKinematic = true;
        }

        handler.weapon.DropWeapon();
        handler.weapon.SetUpPlayerWeapon();
        
    }

    public override void UpdateState(AIHandler handler)
    {

    }

    public override void ExitState(AIHandler handler)
    {

    }
}
