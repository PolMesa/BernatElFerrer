using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f; // AJUSTA ESTO PARA CADA ENEMIGO
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

        // Barra siempre visible
        if (healthBarCanvas != null)
        {
            healthBarCanvas.SetActive(true);
            healthBarTransform = healthBarCanvas.transform;
        }

        Debug.Log($"{gameObject.name} - Vida inicial: {currentHealth}/{maxHealth}");
    }

    void Update()
    {
        // Hacer que la barra de vida siga al enemigo
        if (healthBarCanvas != null && healthBarCanvas.activeSelf)
        {
            healthBarTransform.position = transform.position + healthBarOffset;
            healthBarTransform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth); // No menos de 0

        Debug.Log($"{gameObject.name} recibió {damage} de daño. Vida: {currentHealth}/{maxHealth}");

        // Actualizar slider
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        // Feedback visual
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
            // Método profesional que no interfiere con otros sistemas
            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            sprite.GetPropertyBlock(mpb);

            // Cambiar a rojo
            mpb.SetColor("_Color", Color.red);
            sprite.SetPropertyBlock(mpb);

            yield return new WaitForSeconds(0.1f);

            // Restaurar
            mpb.SetColor("_Color", Color.white);
            sprite.SetPropertyBlock(mpb);
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

    // NUEVO: Para saber cuántos golpes necesita
    public int GetHitsToDie(float damagePerHit)
    {
        return Mathf.CeilToInt(maxHealth / damagePerHit);
    }

    public float GetCurrentHealth() => currentHealth;
    public float GetHealthPercentage() => currentHealth / maxHealth;
}
