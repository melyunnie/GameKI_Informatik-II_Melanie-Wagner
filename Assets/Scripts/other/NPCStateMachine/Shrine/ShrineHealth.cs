using UnityEngine;
using System;

public class ShrineHealth : HealthBase, IDamageable, IKillable
{
    public event Action<int> OnShrineDamageTaken;
    public event Action OnShrineDeath;

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
        OnShrineDamageTaken?.Invoke(damage);

        if (IsDead)
        {
            Debug.Log("Shrine dead");

            Kill();
        }
    }


    public void Kill()
    {
        gameObject.SetActive(false);
        OnShrineDeath?.Invoke();

    }
}