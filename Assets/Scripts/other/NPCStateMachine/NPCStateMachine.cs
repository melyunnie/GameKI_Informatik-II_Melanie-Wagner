using UnityEngine;
using System.Collections;
public class NPCStateMachine : MonoBehaviour
{
    NPCBaseState currentState;
    #region Enemy

    public EnemyDeathState enemydeath = new EnemyDeathState();
    public float atkTimer;

    public float atkCooldown;
    public bool atkready = false;
    public EnemyHealth enemyHealth;
    private ShrineHealth shrineHealth;
    public int attackDamageAmount;
    public bool isMainShrine = false;
    #region tank
    public EnemyTankMovementState enemyTankMovement = new EnemyTankMovementState();
    public EnemyTankAttackState enemyTankAttack = new EnemyTankAttackState();
    #endregion

    #region CloseCombat
    public EnemyCloseCombatMovementState enemyCloseCombatMovement = new EnemyCloseCombatMovementState();
    public EnemyCloseCombatAttackState enemyCloseCombatAttack = new EnemyCloseCombatAttackState();
    #endregion

    #region RangedCombat
    public EnemyRangedCombatMovementState enemyRangedCombatMovement = new EnemyRangedCombatMovementState();
    public EnemyRangedCombatAttackState enemyRangedCombatAttack = new EnemyRangedCombatAttackState();
    #endregion

    #endregion



    public NpcMovement npcMovement;
    private void Awake()
    {
        //npcMovement.enabled=false;
    }
    void Start()
    {
        CheckNPCTag();
    }
    void Update()
    {
        currentState.UpdateState(this);

    }
    public void SwitchState(NPCBaseState state)
    {

        currentState = state;
        state.EnterState(this);
    }
    void CheckNPCTag()
    {
        if (gameObject.CompareTag("Tank"))
        {
            currentState = enemyTankMovement;
            currentState.EnterState(this);

        }
        else if (gameObject.CompareTag("CloseCombat"))
        {
            currentState = enemyCloseCombatMovement;
            currentState.EnterState(this);
        }
        else if (gameObject.CompareTag("RangedCombat"))
        {
            currentState = enemyRangedCombatMovement;
            currentState.EnterState(this);
        }

    }
    public void CheckHP()
    {
        if (enemyHealth.currentHealth == 0)
            SwitchState(enemydeath);
    }
    public void ShrineHeathCheck()
    {
        if (shrineHealth.currentHealth <= 0 && isMainShrine == false) { Debug.Log("hpcheck"); currentState.ExitState(this); }//?
    }

    public IEnumerator AtkTimer()
    {
        Debug.Log("timer");
        yield return new WaitForSeconds(atkTimer);
        currentState.ExitState(this);

    }
    public IEnumerator AtkCooldown()
    {
        Debug.Log("cooldown");
        yield return new WaitForSeconds(atkCooldown);
        atkready = true;

    }
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision);
        collision.gameObject.TryGetComponent(out shrineHealth);
        if (collision.gameObject.CompareTag("Mainshrine")) { isMainShrine = true; Debug.Log("main"); }
        currentState.OnCollisionEnter(this, collision);

    }
    public void OnTriggerStay(Collider collision)
    {
        currentState.OnCollisionStay(this, collision); if (collision == null) { currentState.ExitState(this); }
    }
    public void OnTriggerExit(Collider collision) { currentState.ExitState(this); }

}

