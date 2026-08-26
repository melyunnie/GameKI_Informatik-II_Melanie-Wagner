using UnityEngine;

    public class HealthBase : MonoBehaviour
    {
        public int maxHealth;
        public int currentHealth;
        public bool IsFullLife => currentHealth >= maxHealth;
        public bool IsDead => currentHealth <= 0;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        private void OnEnable()
        {
            currentHealth = maxHealth;
        }
    }
public interface IKillable
{
    void Kill();
}
public interface IDamageable
{
    void TakeDamage(int damage);
}

