using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAwareState : AIBaseState
{
    public override void EnterState(AIHandler handler)
    {
        foreach (AIHandler agent in handler.currentNodeRegion.agentsInRegion)
        {
            if (agent == handler) continue;
            agent.ChangeState(agent.chasingState);
        }
        handler.ChangeState(handler.chasingState);
    }

    public override void UpdateState(AIHandler handler)
    {

    }

    public override void ExitState(AIHandler handler)
    {

    }
}
