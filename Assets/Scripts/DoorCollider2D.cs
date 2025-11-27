using UnityEngine;

public class DoorController2D : MonoBehaviour
{
    // Indica si la puerta ya está abierta.
    // Nos sirve para no ejecutar la lógica de apertura más de una vez.
    private bool isOpen = false;

    // Referencia al Collider2D de la puerta.
    // Mientras esté activado, la puerta bloquea el paso del jugador.
    private Collider2D col;

    // Referencia al SpriteRenderer de la puerta.
    // Lo usamos para cambiar el color cuando se abre.
    private SpriteRenderer spriteRenderer;

    // Awake se ejecuta al iniciar el objeto (antes de Start).
    // Aquí buscamos y guardamos las referencias a los componentes.
    private void Awake()
    {
        // Buscamos el Collider2D que está en el mismo GameObject.
        col = GetComponent<Collider2D>();

        // Buscamos el SpriteRenderer que está en el mismo GameObject.
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Este método se llama desde fuera (por ejemplo, desde el GameManager)
    // cuando se cumple la condición para abrir la puerta
    // (por ejemplo, cuando el jugador ha recogido y entregado la llave).
    public void OpenDoor()
    {
        // Si la puerta ya está abierta, salimos y no hacemos nada.
        // Evitamos repetir cambios o errores.
        if (isOpen) return;

        // Marcamos que la puerta ya está abierta.
        isOpen = true;

        // 1) La puerta deja de bloquear al jugador:
        //    desactivamos su collider para que el personaje pueda pasar.
        if (col != null)
            col.enabled = false;

        // 2) Feedback visual:
        //    cambiamos el color del sprite a verde semitransparente
        //    para indicar que la puerta está abierta.
        if (spriteRenderer != null)
            spriteRenderer.color = new Color(0f, 1f, 0f, 0.5f);

        // NOTA PARA MEJORAS:
        // - Aquí se podría reproducir un sonido (AudioSource.Play()).
        // - O lanzar una animación de abrir puerta con un Animator.
    }
}
