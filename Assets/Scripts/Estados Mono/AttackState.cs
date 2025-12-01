using Unity.VisualScripting;
using UnityEngine;

public class AttackState : PlayerState
{
    public AttackState(PlayerStateMachine player) : base(player) { }

    public override void Enter()
    {
        Debug.Log("🗡️ Entrando en estado de ataque");

        // Detener movimiento durante el ataque
        player.rb2D.linearVelocity = new Vector2(0, player.rb2D.linearVelocity.y);

        // Reproducir animación de ataque
        // EL DAÑO SE APLICARÁ CON ANIMATION EVENT, NO AQUÍ
        player.animator.SetTrigger("Attack");

        // Registrar tiempo del ataque para cooldown
        player.lastAttackTime = Time.time;
        AttackAnimationState.AttackFinished += OnAttackAnimationFinished;
    }
    

    public override void Update()
    {
        // Ya no necesitamos timer aquí, la animación maneja el tiempo

        // Podemos verificar si la animación ha terminado
        // (esto es opcional si usas Animation Event para terminar)

        // Si no usas Animation Event para terminar, puedes usar esto:
        /*
        AnimatorStateInfo stateInfo = player.animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("PlayerAttack") && stateInfo.normalizedTime >= 1f)
        {
            ReturnToPreviousState();
        }
        */
    }

    // Método llamado por Animation Event cuando termina la animación
    public void OnAttackAnimationFinished()
    {
        ReturnToPreviousState();
    }

    private void ReturnToPreviousState()
    {
        if (!player.isGrounded)
        {
            player.ChangeState(player.fallingState);
            Debug.Log("CAMBIANDO A FALL");
        }
        else if (player.GetMoveInput() != 0)
        {
            player.ChangeState(player.walkState);
            Debug.Log("CAMBIANDO A WaLK");

        }
        else
        {
            player.ChangeState(player.idleState);
            Debug.Log("CAMBIANDO A IDLE");

        }
    }

    public override void Exit()
    {
        Debug.Log("Saliendo del estado de ataque");
        AttackAnimationState.AttackFinished -= OnAttackAnimationFinished;

    }
}
