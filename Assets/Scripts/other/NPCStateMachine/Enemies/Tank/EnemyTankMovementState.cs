using UnityEngine;

public class EnemyTankMovementState : NPCBaseState
{
    public override void EnterState(NPCStateMachine NPC)
    {

        NPC.npcMovement.enabled = true;
    }
    public override void UpdateState(NPCStateMachine NPC)
    {
        NPC.CheckHP();
    }
    public override void ExitState(NPCStateMachine NPC)
    {

    }

    public override void OnCollisionEnter(NPCStateMachine NPC, Collider collider)
    {

        if (collider.CompareTag("Mainshrine"))
        {
            NPC.npcMovement.enabled = false; NPC.SwitchState(NPC.enemyTankAttack);
        }
    }
    public override void OnCollisionStay(NPCStateMachine NPC, Collider collider)
    {

    }
}

