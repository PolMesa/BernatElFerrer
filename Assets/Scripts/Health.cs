using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public event Action OnDeath;

    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log("Health inicializado - Salud: " + currentHealth + "/" + maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Daño recibido: " + damage + " - Salud restante: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Jugador murió - Invocando evento OnDeath");
        OnDeath?.Invoke();
    }

    public void RestoreHealth()
    {
        currentHealth = maxHealth;
        Debug.Log("Salud restaurada: " + currentHealth + "/" + maxHealth);
    }

    // Para testing: mata al jugador instantáneamente
    public void InstantDeath()
    {
        Debug.Log("Muerte instantánea activada");
        Die();
    }
}
