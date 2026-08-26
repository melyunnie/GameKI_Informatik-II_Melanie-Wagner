using UnityEngine;

public class EnemyCloseCombatMovementState : NPCBaseState
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

        if (collider.CompareTag("shrine") || collider.CompareTag("Mainshrine"))
        {
            NPC.npcMovement.enabled = false; NPC.SwitchState(NPC.enemyCloseCombatAttack);
        }
    }
    public override void OnCollisionStay(NPCStateMachine NPC, Collider collider)
    {

    }
}
