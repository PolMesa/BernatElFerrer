using UnityEngine;

public class DeathZone : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Aquí defines lo que pasa al "morir"
            // Por ejemplo, destruir al jugador:
            collision.GetComponent<PlayerController>().Die();

            // O reiniciar la escena:
            // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    

}
