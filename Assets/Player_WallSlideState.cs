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

        if (playerInputSet.Player.Jump.WasPressedThisFrame())
            assignedMachine.ChangeState(assignedPlayer.WallJumpState);

        if (assignedPlayer.CanGrabEdge() && !(playerInputSet.Player.Movement.ReadValue<Vector2>().y < 0))
            assignedMachine.ChangeState(assignedPlayer.EdgeGrabState);
        else if (!assignedPlayer.CanWallSlide())
            assignedMachine.ChangeState(assignedPlayer.FallState);
        else if (assignedPlayer.IsGrounded())
        {
            assignedMachine.ChangeState(assignedPlayer.IdleState);
            assignedPlayer.FlipDirection();
        }

        //if (playerInputSet.Player.Dash.WasPerformedThisFrame())
        //{
        //    assignedMachine.ChangeState(assignedPlayer.DashState);
        //    assignedPlayer.FlipDirection();
        //}
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
