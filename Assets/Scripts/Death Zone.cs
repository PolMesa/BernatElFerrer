using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("☠️ Jugador cayó en Death Zone");

            PlayerStateMachine player = other.GetComponent<PlayerStateMachine>();
            if (player != null)
            {
                player.Die(); // Esto ahora funciona

            }
            else
            {
                Debug.LogError("❌ No se encontró PlayerStateMachine en el jugador");
            }
        }
    }
}

