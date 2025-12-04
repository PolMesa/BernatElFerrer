using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Stats")]
    public int maxLives = 3;
    private int currentLives;
    private bool isDead = false;

    [Header("UI")]
    public TMP_Text livesText;
    public Slider healthSlider; // Opcional, si usas slider

    [Header("Game Over")]
    public string gameOverScene = "Game Over";
    public float gameOverDelay = 2f;

    void Start()
    {
        currentLives = maxLives;
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        // Restar vida
        currentLives -= damage;
        currentLives = Mathf.Max(0, currentLives);

        // Forzar actualización inmediata de UI
        UpdateUI();

        Debug.Log($"💔 Vida: {currentLives}/{maxLives}");

        // Si vida llega a 0, iniciar Game Over con delay
        if (currentLives <= 0)
        {
            StartCoroutine(GameOverSequence());
        }
    }

    private IEnumerator GameOverSequence()
    {
        isDead = true;

        // 1. Forzar mostrar "0 vidas" inmediatamente
        currentLives = 0;
        UpdateUI();

        // 2. Pausa para que se vea el "0" en pantalla
        yield return new WaitForSeconds(0.5f);

        // 3. Desactivar controles del jugador
        PlayerStateMachine player = GetComponent<PlayerStateMachine>();
        if (player != null)
        {
            player.enabled = false;
            if (player.rb2D != null)
            {
                player.rb2D.linearVelocity = Vector2.zero;
            }
        }

        // 4. Efecto visual de muerte (parpadeo)
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            for (int i = 0; i < 3; i++)
            {
                sprite.color = new Color(1, 0.5f, 0.5f, 0.5f);
                yield return new WaitForSeconds(0.2f);
                sprite.color = Color.white;
                yield return new WaitForSeconds(0.2f);
            }
        }

        // 5. Esperar tiempo restante antes de Game Over
        yield return new WaitForSeconds(gameOverDelay - 1f);

        // 6. Cargar escena Game Over
        LoadGameOverScene();
    }

    private void LoadGameOverScene()
    {
        Debug.Log("🎮 Cargando Game Over...");
        if (!string.IsNullOrEmpty(gameOverScene))
        {
            SceneManager.LoadScene(gameOverScene);
        }
        else
        {
            Debug.LogError("❌ Nombre de escena Game Over no asignado");
        }
    }

    private void UpdateUI()
    {
        // Actualizar texto
        if (livesText != null)
        {
            livesText.text = "Vidas: " + currentLives;

            // Cambiar color según vida
            if (currentLives <= 1)
                livesText.color = Color.red;
            else if (currentLives <= maxLives / 2)
                livesText.color = Color.yellow;
            else
                livesText.color = Color.white;
        }

        // Actualizar slider si existe
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxLives;
            healthSlider.value = currentLives;

            // Cambiar color de la barra
            Image fillImage = healthSlider.fillRect.GetComponent<Image>();
            if (fillImage != null)
            {
                if (currentLives <= 1)
                    fillImage.color = Color.red;
                else if (currentLives <= maxLives / 2)
                    fillImage.color = Color.yellow;
                else
                    fillImage.color = Color.cyan;
            }
        }
    }

    public void AddLives(int amount)
    {
        if (isDead) return;

        currentLives += amount;
        if (currentLives > maxLives)
            currentLives = maxLives;

        UpdateUI();
    }

    public int GetCurrentLives()
    {
        return currentLives;
    }

    public bool IsDead()
    {
        return isDead;
    }
}
