
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class ShrineDeathState : ShrineBaseState
{
    

    public override void ShrineEnterState(ShrineStateMachine shrine)
    {
        Debug.Log("dead");  shrine.gameObject.SetActive(false);
      
    }
    public override void ShrineExitState(ShrineStateMachine shrine)
    {
        
    }

    public override void ShrineOnCollisionEnter(ShrineStateMachine shrine, Collider collider)
    {
       
    }

    

    public override void ShrineOnCollisionStay(ShrineStateMachine shrine, Collider collider)
    {
        
    }

    public override void ShrineUpdateState(ShrineStateMachine shrine)
    {
        
    }
}
