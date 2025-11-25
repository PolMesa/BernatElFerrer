using UnityEngine;

public class WalkState : PlayerState
{
    public WalkState(PlayerStateMachine player) : base(player) { }

    public override void Enter()
    {
        // Opcional: Debug.Log("Entró en Walk");
    }

    public override void Update()
    {
        float input = player.GetMoveInput();

        // Mover horizontalmente
        player.MoveHorizontal(input);

        // Si se cae de un borde, pasar a Falling
        if (!player.isGrounded)
        {
            player.ChangeState(player.fallingState);
            return;
        }

        // Si deja de moverse, volver a Idle
        if (input == 0)
        {
            player.ChangeState(player.idleState);
            return;
        }

        // Si presiona salto, pasar a Jump
        if (Input.GetButtonDown("Jump") && player.isGrounded)
        {
            player.ChangeState(player.jumpState);
            return;
        }
    }

    public override void Exit()
    {
        // Opcional: Debug.Log("Salió de Walk");
    }
}

