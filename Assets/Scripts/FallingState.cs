using UnityEngine;

public class FallingState : PlayerState
{
    public FallingState(PlayerStateMachine player) : base(player) { }

    public override void Enter()
    {
        // Opcional: puedes poner un Debug
        // Debug.Log("Entró en Falling");
    }

    public override void Update()
    {
        float input = player.GetMoveInput();
        player.MoveHorizontal(input);

        // Si toca el suelo, decide a qué estado ir
        if (player.isGrounded)
        {
            if (input == 0)
                player.ChangeState(player.idleState);
            else
                player.ChangeState(player.walkState);
        }
    }

    public override void Exit()
    {
        // Opcional: Debug
        // Debug.Log("Salió de Falling");
    }
}
