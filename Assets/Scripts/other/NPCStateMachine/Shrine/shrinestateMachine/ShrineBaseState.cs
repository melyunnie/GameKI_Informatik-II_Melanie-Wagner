using UnityEngine;

public abstract class ShrineBaseState 
{
    public abstract void ShrineEnterState(ShrineStateMachine shrine);
    public abstract void ShrineUpdateState(ShrineStateMachine shrine);
    public abstract void ShrineExitState(ShrineStateMachine shrine);
    public abstract void ShrineOnCollisionEnter(ShrineStateMachine shrine, Collider collider);
    public abstract void ShrineOnCollisionStay(ShrineStateMachine shrine, Collider collider);

}
