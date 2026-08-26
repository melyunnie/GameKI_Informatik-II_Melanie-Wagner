using UnityEngine;

public abstract class NPCBaseState
{

    public abstract void EnterState(NPCStateMachine NPC);
    public abstract void UpdateState(NPCStateMachine NPC);
    public abstract void ExitState(NPCStateMachine NPC);
    public abstract void OnCollisionEnter(NPCStateMachine NPC, Collider collider);
    public abstract void OnCollisionStay(NPCStateMachine NPC, Collider collider);

}
