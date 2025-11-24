using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    

    private Rigidbody2D rb2D;
    private float moveInput;
    private bool isGrounded;

    public Transform groundCheck;
    public float checkRadius = 0.05f;
    public LayerMask groundLayer;

    public TMP_Text scoreText;
    public int score = 0;

    public Vector3 respawnPosition;


    private Animator animator;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        respawnPosition = transform.position;
        
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        rb2D.linearVelocity = new Vector2(moveInput * moveSpeed, rb2D.linearVelocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, jumpForce);
        }

        if (moveInput != 0)
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);

        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        animator.SetFloat("VerticalVelocity", rb2D.linearVelocity.y);
        animator.SetBool("IsGrounded", isGrounded);
    }
    public void Die()
    {
        // Teletransporte al punto de respawn
        transform.position = respawnPosition;
    }
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Item"))
        {
            score++;
            UpdateScore();
            Destroy(other.gameObject);
        }
    }



    public void Respawn()
    {
        Debug.Log("Intentando respawnear jugador...");

        if (CheckpointManager.Instance != null && CheckpointManager.Instance.HasCheckpoint())
        {
            Vector3 checkpointPos = CheckpointManager.Instance.GetCurrentCheckpoint();
            Debug.Log("Respawneando en checkpoint: " + checkpointPos);

            // Teletransportar al checkpoint
            transform.position = checkpointPos;

            // Restaurar propiedades físicas
            if (rb2D != null)
            {
                rb2D.linearVelocity = Vector2.zero;
                rb2D.angularVelocity = 0f;
            }

         

            Debug.Log("Jugador respawneado exitosamente en checkpoint");
        }
        else
        {
            Debug.LogWarning("No hay checkpoint disponible - respawneando en posición actual");
        }
    }
}