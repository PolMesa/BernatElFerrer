using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("UI Elements")]
    public Slider healthSlider;
    public GameObject healthBarCanvas;
    public Vector3 healthBarOffset = new Vector3(0, 1.5f, 0);

    [Header("Death Settings")]
    public GameObject deathEffect;
    public float destroyDelay = 2f;

    private EnemyStateMachine enemyStateMachine;
    private Transform healthBarTransform;

    void Start()
    {
        currentHealth = maxHealth;
        enemyStateMachine = GetComponent<EnemyStateMachine>();

        // Configurar slider
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
            healthSlider.minValue = 0;
        }

        // BARRA SIEMPRE VISIBLE - Elimina esta línea que la ocultaba:
        // if (healthBarCanvas != null) { healthBarCanvas.SetActive(false); }

        // En su lugar, asegúrate de que esté activa:
        if (healthBarCanvas != null)
        {
            healthBarCanvas.SetActive(true); // SIEMPRE ACTIVA
            healthBarTransform = healthBarCanvas.transform;
        }
    }

    void Update()
    {
        // Hacer que la barra de vida siga al enemigo
        if (healthBarCanvas != null && healthBarCanvas.activeSelf)
        {
            // Posición: encima del enemigo + offset
            healthBarTransform.position = transform.position + healthBarOffset;

            // Rotación: siempre mirando a la cámara (2D simplificado)
            if (Camera.main != null)
            {
                // Para 2D, simplemente bloquea la rotación en Z
                healthBarTransform.rotation = Quaternion.Euler(0, 0, 0);

                // O si quieres que siempre mire a la cámara en 3D:
                // healthBarTransform.LookAt(Camera.main.transform);
                // healthBarTransform.Rotate(0, 180, 0); // Para que no esté al revés
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        // ELIMINADO: Ya no necesitamos activar la barra aquí
        // if (healthBarCanvas != null && !healthBarCanvas.activeSelf)
        // {
        //     healthBarCanvas.SetActive(true);
        // }

        // Actualizar slider
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        Debug.Log($"{gameObject.name} recibió {damage} de daño. Vida: {currentHealth}/{maxHealth}");

        // Feedback visual de daño
        StartCoroutine(DamageFlash());

        // Verificar muerte
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private System.Collections.IEnumerator DamageFlash()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            Color original = sprite.color;
            sprite.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sprite.color = original;
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} murió");

        // Cambiar al estado de muerte
        if (enemyStateMachine != null)
        {
            enemyStateMachine.ChangeState(enemyStateMachine.deathState);
        }

        // Efecto de muerte
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        // Ocultar barra de vida al morir
        if (healthBarCanvas != null)
        {
            healthBarCanvas.SetActive(false);
        }

        // Desactivar colisiones
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null) collider.enabled = false;

        // Desactivar este script
        enabled = false;

        // Destruir después de un tiempo
        Destroy(gameObject, destroyDelay);
    }

    // Para daño por porcentaje
    public void TakePercentageDamage(float percentage)
    {
        float damage = maxHealth * percentage;
        TakeDamage(damage);
    }

    public float GetCurrentHealth() => currentHealth;
    public float GetHealthPercentage() => currentHealth / maxHealth;
}
