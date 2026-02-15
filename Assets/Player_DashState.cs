using UnityEngine;

public class Player_DashState : EntityState
{
    private float originalGravityScale;
    private int dashDirectionX;

    public Player_DashState(Player player, StateMachine machine) : base(player, machine, "dash")
    {
    }

    override public void Enter()
    {
        base.Enter();

        stateTimer = assignedPlayer.DashDuration;

        // Disable gravity during dash
        originalGravityScale = playerRigidbody2D.gravityScale;
        playerRigidbody2D.gravityScale = 0f;

        // Ensure dash direction remains in current facing direction
        dashDirectionX = assignedPlayer.MovementInput.x != 0 ? (int)assignedPlayer.MovementInput.x : assignedPlayer.CurrentDirectionX;
    }

    override public void Update()
    {
        base.Update();
        CheckForDashCancelation();

        assignedPlayer.SetVelocity(dashDirectionX * assignedPlayer.DashSpeed, 0f);

        if (stateTimer <= 0f)
            if (assignedPlayer.Grounded)
                assignedMachine.ChangeState(assignedPlayer.IdleState);
            else
                assignedMachine.ChangeState(assignedPlayer.FallState);
    }

    override public void Exit()
    {
        base.Exit();
        assignedPlayer.SetVelocity(assignedPlayer.CurrentDirectionX * (1 + (assignedPlayer.DashSpeed * .25f)), 0f);
        playerRigidbody2D.gravityScale = originalGravityScale;
    }

    private void CheckForDashCancelation()
    {
        if(playerInputSet.Player.Dash.WasReleasedThisFrame())
             stateTimer = 0f;

        if (assignedPlayer.WallDetected)
            if (assignedPlayer.Grounded)
                assignedMachine.ChangeState(assignedPlayer.IdleState);
            else
                assignedMachine.ChangeState(assignedPlayer.WallSlideState);
    }
}
