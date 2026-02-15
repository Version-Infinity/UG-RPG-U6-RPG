using UnityEngine;

public class Player_EdgeGrabState : EntityState
{
    float originalGravityScale;

    public Player_EdgeGrabState(Player player, StateMachine machine) : base(player, machine, "edgeGrab")
    {
    }

    override public void Enter()
    {
        base.Enter();
        assignedPlayer.SetVelocity(0, 0);
        originalGravityScale = assignedPlayer.Rigidbody.gravityScale;
        assignedPlayer.Rigidbody.gravityScale = 0f;
    }

    override public void Update()
    {
        base.Update();
        if (playerInputSet.Player.Jump.WasPressedThisFrame())
            assignedMachine.ChangeState(assignedPlayer.WallJumpState);
        else if (assignedPlayer.Grounded)
            assignedMachine.ChangeState(assignedPlayer.IdleState);
        else if (!assignedPlayer.EdgeDetected)
            assignedMachine.ChangeState(assignedPlayer.FallState);
    }

    override public void Exit()
    {
        base.Exit();
        assignedPlayer.Rigidbody.gravityScale = originalGravityScale;
    }
}
