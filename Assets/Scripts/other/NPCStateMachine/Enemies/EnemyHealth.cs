using System;
using UnityEngine;

public class EnemyHealth : HealthBase, IDamageable, IKillable
{
    public event Action<int> OnEnemyDamageTaken;
    public event Action OnEnemyDeath;
    
    void Start()
    {
      
    }
    void Update()
    {
        
    }
    public void TakeDamage(int damage)
    {
        if (IsDead) return;

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        OnEnemyDamageTaken?.Invoke(damage);

        if (IsDead)
        {
            Debug.Log("dead");
            Kill();
        }
    }


    public void Kill()
    {
        OnEnemyDeath?.Invoke();

    }
}
