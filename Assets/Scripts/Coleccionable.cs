using UnityEngine;

public class CollectItem : MonoBehaviour
{
    public int score = 0; // contador de objetos recogidos

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Item"))
        {
            score++;
            Debug.Log("Objeto recogido! Puntos: " + score);
            Destroy(other.gameObject);
        }
    }
}