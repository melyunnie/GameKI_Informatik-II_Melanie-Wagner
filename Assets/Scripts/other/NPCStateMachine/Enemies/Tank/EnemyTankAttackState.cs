
using UnityEngine;

public class EnemyTankAttackState : NPCBaseState
{
    public override void EnterState(NPCStateMachine NPC)
    {
        NPC.atkready = true; Debug.Log("range");

    }
    public override void UpdateState(NPCStateMachine NPC)
    {
        NPC.CheckHP(); NPC.ShrineHeathCheck();

    }
    public override void ExitState(NPCStateMachine NPC)
    {
        NPC.SwitchState(NPC.enemyRangedCombatMovement);
    }

    public override void OnCollisionEnter(NPCStateMachine NPC, Collider collider)
    {


    }
    public override void OnCollisionStay(NPCStateMachine NPC, Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out IDamageable damageable) && NPC.atkready == true)
        {

            damageable.TakeDamage(NPC.attackDamageAmount); NPC.atkready = false; NPC.StartCoroutine(NPC.AtkCooldown());

        }

    }

}
