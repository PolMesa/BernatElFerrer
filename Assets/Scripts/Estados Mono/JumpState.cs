public class JumpState : PlayerState
{
    public JumpState(PlayerStateMachine player) : base(player) { }

    public override void Enter()
    {
        player.Jump();
    }

    public override void Update()
    {
        float input = player.GetMoveInput();
        player.MoveHorizontal(input);

        if (player.rb2D.linearVelocity.y <= 0)
        {
            player.ChangeState(player.fallingState);
        }
    }

    public override void Exit() { }
}



