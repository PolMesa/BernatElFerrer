using UnityEngine;

public class IdleState : PlayerState
{
    public IdleState(PlayerStateMachine player) : base(player) { }

    public override void Enter()
    {
        // Opcional: Debug.Log("Entró en Idle");
    }

    public override void Update()
    {
        float input = player.GetMoveInput();

        // Si está en el aire, pasar a Falling
        if (!player.isGrounded)
        {
            player.ChangeState(player.fallingState);
            return;
        }

        // Si se mueve, pasar a Walk
        if (input != 0)
        {
            player.ChangeState(player.walkState);
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
        // Opcional: Debug.Log("Salió de Idle");
    }
}


