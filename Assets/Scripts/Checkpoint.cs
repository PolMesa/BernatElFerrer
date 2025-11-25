using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Activar checkpoint
            CheckpointManager.Instance.ActivateCheckpoint(transform.position);
        }
    }
}
