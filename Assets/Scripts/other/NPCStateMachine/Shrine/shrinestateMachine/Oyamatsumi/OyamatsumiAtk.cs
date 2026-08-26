using UnityEngine;

public class OyamatsumiAtk : ShrineBaseState
{
    public override void ShrineEnterState(ShrineStateMachine shrine)
    {
        Debug.Log("oyamatsumi");shrine.rockmanager.SetActive(true);
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