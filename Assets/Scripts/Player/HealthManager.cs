
using UnityEngine;
using UnityEngine.Events;

public class HealthManager : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 3;

    private int currentHealth;
    private bool isDead;

    [Header("Events")]
    [SerializeField] private UnityEvent<int> onHealthChanged;
    [SerializeField] private UnityEvent onDeath;

    public int CurrentHealth => currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
        isDead = false;
    }

    private void Start()
    {
        onHealthChanged?.Invoke(currentHealth);
    }

    // Negative values cause damage, positive values heal.
    public void ChangeHealth(int amount)
    {
        if (isDead || amount == 0)
            return;

        int newHealth = Mathf.Clamp(
            currentHealth + amount,
            0,
            maxHealth
        );

        if (newHealth == currentHealth)
            return;

        currentHealth = newHealth;

        Debug.Log($"Health: {currentHealth}/{maxHealth}");

        onHealthChanged?.Invoke(currentHealth);

        if (currentHealth == 0)
            Die();
    }

    // Called automatically when health reaches zero.
    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        Debug.Log("Player died!");

        onDeath?.Invoke();
    }
}
