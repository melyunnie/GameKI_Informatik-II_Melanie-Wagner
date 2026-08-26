using UnityEngine;
public class ShrineStateMachine : MonoBehaviour
{
    ShrineBaseState currentState;
    public ShrineDeathState shrinedeath = new ShrineDeathState();
    public OyamatsumiAtk oyamatsumiAtk= new OyamatsumiAtk();
    private EnemyHealth enemyHealth;
    public ShrineHealth shrineHealth;
    //public int maxHP;
    public GameObject rockmanager;
    void Start() 
    {
       
        ChecShrineTag();
    }

    void Update()
    {
        currentState.ShrineUpdateState(this);

    }
    public void SwitchState(ShrineBaseState state)
    {
        currentState = state;
        state.ShrineEnterState(this);
    }
    private void OnTriggerEnter(Collider collision)
    {
        currentState.ShrineOnCollisionEnter(this, collision);


    }
    public void OnTriggerStay(Collider collision)
    {
        currentState.ShrineOnCollisionStay(this, collision);
    }
    public void CheckHP()
    {
        if (shrineHealth.currentHealth <= 0)
            SwitchState(shrinedeath);
    }
    void ChecShrineTag()
    {
        if (gameObject.CompareTag("Oyamatsumi"))
        {
            currentState = oyamatsumiAtk;
            currentState.ShrineEnterState(this);
        }
        if (gameObject.CompareTag("Hachiman"))
        {
           // currentState = HachimanAtk;
           // currentState.ShrineEnterState(this);
        }
        if (gameObject.CompareTag("Susanoo"))
        {
           // currentState = SusanooAtk;
           // currentState.ShrineEnterState(this);
        }

    }
}
