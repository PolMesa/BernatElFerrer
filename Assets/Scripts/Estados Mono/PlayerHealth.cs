using UnityEngine;
using TMPro;
using UnityEngine.UI; // Añade esta línea

public class PlayerHealth : MonoBehaviour
{
    [Header("Stats")]
    public int maxLives = 3;
    private int currentLives;

    [Header("UI")]
    public TMP_Text livesText;
    public Slider healthSlider; // Añade esta referencia al Slider

    [Header("Respawn")]
    public Vector3 respawnPosition;

    private PlayerStateMachine playerController;

    void Start()
    {
        currentLives = maxLives;
        UpdateUI();

        playerController = GetComponent<PlayerStateMachine>();
        respawnPosition = transform.position;

        // Configurar el Slider si está asignado
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxLives;
            healthSlider.value = currentLives;
        }
    }

    // Función para recibir daño (con porcentaje)
    public void TakeDamage(int damage)
    {
        currentLives -= damage;
        UpdateUI();
        Debug.Log($"Has recibido {damage} de daño. Vidas restantes: {currentLives}");

        if (currentLives <= 0)
        {
            Die();
        }
        else
        {
            // Efectos de daño (opcional)
        }
    }

    // Nueva función para recibir daño por porcentaje
    public void TakePercentageDamage(float percentage)
    {
        int damage = Mathf.CeilToInt(maxLives * percentage);
        TakeDamage(damage);
        Debug.Log($"Daño por {percentage * 100}%: {damage} vidas");
    }

    // Muerte del jugador
    private void Die()
    {
        Debug.Log("¡Jugador muerto!");

        if (playerController != null)
        {
            playerController.Respawn();
        }

        currentLives = maxLives;
        UpdateUI();
    }

    // Actualiza la UI
    private void UpdateUI()
    {
        if (livesText != null)
        {
            livesText.text = "Vidas: " + currentLives;
        }

        if (healthSlider != null)
        {
            healthSlider.value = currentLives;
        }
    }

    // Función para sumar vidas
    public void AddLives(int amount)
    {
        currentLives += amount;
        if (currentLives > maxLives)
            currentLives = maxLives;

        UpdateUI();
    }

    public int GetCurrentLives()
    {
        return currentLives;
    }
}
