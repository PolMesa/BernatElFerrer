using UnityEngine;


public class JumpState : PlayerState
{
    bool jumped = false;

    public JumpState(PlayerStateMachine player) : base(player) { }

    public override void Enter()
    {
        jumped = false;
        // Opcional: Debug.Log("Entr� en Jump");
    }

    public override void Update()
    {
        float input = player.GetMoveInput();
        player.MoveHorizontal(input);

        // Saltar solo una vez al entrar
        if (!jumped)
        {
            player.Jump();
            jumped = true;
        }

        // Si empieza a caer, pasar a FallingState
        if (player.rb2D.linearVelocity.y < 0)
        {
            player.ChangeState(player.fallingState);
            return;
        }

        // Si se toca suelo inmediatamente (por ejemplo salto peque�o)
        if (player.isGrounded && player.rb2D.linearVelocity.y <= 0)
        {
            if (input == 0)
                player.ChangeState(player.idleState);
            else
                player.ChangeState(player.walkState);
        }
    }

    public override void Exit()
    {
        // Opcional: Debug.Log("Sali� de Jump");
    }
}


