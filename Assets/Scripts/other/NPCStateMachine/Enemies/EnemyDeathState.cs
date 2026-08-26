using UnityEngine;

public class EnemyDeathState : NPCBaseState
{
    public override void EnterState(NPCStateMachine NPC)
    {
        Debug.Log("dead"); NPC.npcMovement.enabled = false;
    }

    public override void ExitState(NPCStateMachine NPC)
    {

    }

    public override void UpdateState(NPCStateMachine NPC)
    {

    }

    public override void OnCollisionEnter(NPCStateMachine NPC, Collider collider)
    {

    }
    public override void OnCollisionStay(NPCStateMachine NPC, Collider collider)
    {

    }

}
