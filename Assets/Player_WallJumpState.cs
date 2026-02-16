using UnityEngine;

public class Player_WallJumpState : EntityState
{
    public Player_WallJumpState(Player player, StateMachine machine) : base(player, machine, "jumpFall")
    {
    }

    override public void Enter()
    {
        base.Enter();
        HandleWallJump();
    }

    private void HandleWallJump()
    {
        assignedPlayer.SetVelocity(assignedPlayer.WallJumpForce.x * -(assignedPlayer.CurrentDirectionX), assignedPlayer.WallJumpForce.y);
    }

    public override void Update()
    {
        base.Update();

        if (playerRigidbody2D.linearVelocityY < 0)
            assignedMachine.ChangeState(assignedPlayer.FallState);

        if(assignedPlayer.IsTouchingWall())
            assignedMachine.ChangeState(assignedPlayer.WallSlideState);
    }
}
