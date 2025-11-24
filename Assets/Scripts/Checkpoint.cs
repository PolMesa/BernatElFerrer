using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Animator animator;
    private bool isActive = false;

    public Color inactiveColor = Color.gray;
    public Color activeColor = Color.green;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (spriteRenderer != null)
        {
            spriteRenderer.color = inactiveColor;
        }

        // Asegurarse de que el CheckpointManager existe
        EnsureCheckpointManagerExists();
    }

    private void EnsureCheckpointManagerExists()
    {
        if (CheckpointManager.Instance == null)
        {
            Debug.Log("CheckpointManager no encontrado - creando uno automáticamente");

            // Buscar en la escena
            CheckpointManager existingManager = FindObjectOfType<CheckpointManager>();
            if (existingManager != null)
            {
                Debug.Log("CheckpointManager encontrado en escena pero Instance no estaba asignado");
            }
            else
            {
                // Crear nuevo GameObject con CheckpointManager
                GameObject managerObject = new GameObject("CheckpointManager");
                managerObject.AddComponent<CheckpointManager>();
                Debug.Log("CheckpointManager creado automáticamente");
            }
        }
        else
        {
            Debug.Log("CheckpointManager encontrado correctamente");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isActive)
        {
            ActivateCheckpoint();
        }
    }

    private void ActivateCheckpoint()
    {
        // Verificar nuevamente antes de activar
        if (CheckpointManager.Instance == null)
        {
            Debug.LogError("CheckpointManager.Instance sigue siendo null después de la verificación");
            return;
        }

        CheckpointManager.Instance.SetCheckpoint(transform.position);
        Debug.Log("Checkpoint activado exitosamente en: " + transform.position);
    }
}