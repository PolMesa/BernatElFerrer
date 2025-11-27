using UnityEngine;
using TMPro; // si usas TextMeshPro para la UI

public class PlayerHealth : MonoBehaviour
{
    [Header("Stats")]
    public int maxLives = 3;
    private int currentLives;

    [Header("UI")]
    public TMP_Text livesText; // Asignar en el inspector

    [Header("Respawn")]
    public Vector3 respawnPosition;

    private PlayerStateMachine playerController;

    void Start()
    {
        currentLives = maxLives;
        UpdateUI();

        playerController = GetComponent<PlayerStateMachine>();
        respawnPosition = transform.position; // posición inicial
    }

    // Función para recibir daño
    public void TakeDamage(int damage)
    {
        currentLives -= damage;
        UpdateUI();
        Debug.Log("Has recibido daño");
        if (currentLives <= 0)
        {
            Die();
        }
        else
        {
            // opcional: retroceso, animación de daño, sonido...
        }
    }

    // Muerte del jugador
    private void Die()
    {
        // Puedes hacer respawn o reiniciar nivel
        if (playerController != null)
        {
            playerController.Respawn(); // tu función existente
        }

        currentLives = maxLives; // si quieres reiniciar las vidas al respawnear
        UpdateUI();
    }

    // Actualiza la UI
    private void UpdateUI()
    {
        if (livesText != null)
        {
            livesText.text = "Vidas: " + currentLives;
        }
    }

    // Función para sumar vidas (por powerups, por ejemplo)
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

