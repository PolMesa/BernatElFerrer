using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;

    private Vector3 currentCheckpoint;
    private bool hasCheckpoint = false;

    private void Awake()
    {
        // Singleton simple
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Guardar un checkpoint
    public void ActivateCheckpoint(Vector3 position)
    {
        currentCheckpoint = position;
        hasCheckpoint = true;
        Debug.Log("Checkpoint activado en: " + position);
    }

    public Vector3 GetCurrentCheckpoint()
    {
        return currentCheckpoint;
    }

    public bool HasCheckpoint()
    {
        return hasCheckpoint;
    }
}

