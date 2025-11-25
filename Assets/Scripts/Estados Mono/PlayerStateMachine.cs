using UnityEngine;
using TMPro;

public class PlayerStateMachine : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius = 0.05f;
    public LayerMask groundLayer;
    public bool isGrounded;

    [Header("Score")]
    public TMP_Text scoreText;
    public int score = 0;

    [Header("Respawn")]
    public Vector3 respawnPosition;

    [HideInInspector] public Rigidbody2D rb2D;
    private Animator animator;

    // -----------------------------
    // Máquina de Estados
    // -----------------------------
    public PlayerState currentState;
    public IdleState idleState;
    public WalkState walkState;
    public JumpState jumpState;
    public FallingState fallingState;

    float moveInput;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        respawnPosition = transform.position;

        // Inicializar estados
        idleState = new IdleState(this);
        walkState = new WalkState(this);
        jumpState = new JumpState(this);
        fallingState = new FallingState(this);

        ChangeState(idleState);
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        // Actualizar estado actual
        currentState?.Update();

        // Animaciones
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        animator.SetFloat("VerticalVelocity", rb2D.linearVelocity.y);
        animator.SetBool("IsGrounded", isGrounded);

        // Flip del sprite según dirección
        if (moveInput != 0)
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
    }

    // -----------------------------
    // Funciones para los estados
    // -----------------------------
    public void MoveHorizontal(float input)
    {
        rb2D.linearVelocity = new Vector2(input * moveSpeed, rb2D.linearVelocity.y);
    }

    public void Jump()
    {
        rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, jumpForce);
    }

    public void ChangeState(PlayerState newState)
    {
        if (currentState == newState) return;

        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public float GetMoveInput() => moveInput;

    // -----------------------------
    // Funciones de Score / Items
    // -----------------------------
    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Recolectables
        if (other.CompareTag("Item"))
        {
            score++;
            UpdateScore();
            Destroy(other.gameObject);
        }

        // Checkpoints
        if (other.CompareTag("Checkpoint"))
        {
            if (CheckpointManager.Instance != null)
                CheckpointManager.Instance.ActivateCheckpoint(other.transform.position);
        }
    }

    // -----------------------------
    // Respawn / Muerte
    // -----------------------------
    public void Die()
    {
        Respawn();
    }

    public void Respawn()
    {
        Vector3 targetPos = respawnPosition;

        if (CheckpointManager.Instance != null && CheckpointManager.Instance.HasCheckpoint())
        {
            targetPos = CheckpointManager.Instance.GetCurrentCheckpoint();
            Debug.Log("Jugador respawneado en checkpoint: " + targetPos);
        }
        else
        {
            Debug.Log("Jugador respawneado en inicio");
        }

        // Teletransportar
        transform.position = targetPos;

        // Reiniciar físicas
        if (rb2D != null)
        {
            rb2D.linearVelocity = Vector2.zero;
            rb2D.angularVelocity = 0f;
        }
    }
}

