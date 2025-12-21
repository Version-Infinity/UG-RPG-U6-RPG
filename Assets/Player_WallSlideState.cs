using UnityEngine;
using UnityEngine.Rendering;

public class Player_WallSlideState : EntityState
{
    public Player_WallSlideState(Player player, StateMachine machine) : base(player, machine, "wallSlide")
    {
    }

    override public void Update()
    {
        base.Update();

        HandleWallSlide();

        if (assignedPlayer.Grounded)
        {
            assignedMachine.ChangeState(assignedPlayer.IdleState);
            assignedPlayer.FlipDirection();
        }

        if (!assignedPlayer.WallDetected)
            assignedMachine.ChangeState(assignedPlayer.FallState);
    }

    private void HandleWallSlide()
    {
        if (assignedPlayer.MovementInput.y < 0)
        {
            assignedPlayer.SetVelocity(assignedPlayer.MovementInput.x, playerRigidbody2D.linearVelocityY);
        }
        else
        {
           assignedPlayer.SetVelocity(assignedPlayer.MovementInput.x, playerRigidbody2D.linearVelocityY * assignedPlayer.WallSlideSpeed);
        }
    }
}
