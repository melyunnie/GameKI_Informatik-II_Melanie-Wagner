using UnityEngine;

public class RockDMG : MonoBehaviour
{
    public int damageAmount;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IDamageable damageable) )
        {

            damageable.TakeDamage(damageAmount);

        }
    }
}
