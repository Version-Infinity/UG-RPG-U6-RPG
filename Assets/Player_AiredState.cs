using UnityEngine;

public class Player_AiredState : EntityState
{
    public Player_AiredState(Player player, StateMachine machine, string name) : base(player, machine, name)
    {
    }

    public override void Update()
    {
        base.Update();

        if(assignedPlayer.MovementInput.x != 0)
        {
            assignedPlayer.SetVelocity(assignedPlayer.MovementInput.x * (assignedPlayer.MoveSpeed * assignedPlayer.InAirMoveMultiplier), playerRigidbody2D.linearVelocityY);
        }

        if(assignedPlayer.IsGrounded())
            assignedMachine.ChangeState(assignedPlayer.IdleState);
        else if (assignedPlayer.IsTouchingWall())
            assignedMachine.ChangeState(assignedPlayer.WallSlideState);
        else if (attackInputDetected())
            assignedMachine.ChangeState(assignedPlayer.BasicAttackState);
    }

    private bool attackInputDetected()
    {
        bool attackAttampted = playerInputSet.Player.Attack.WasPressedThisFrame() || playerInputSet.Player.LightAttack.WasPressedThisFrame() || playerInputSet.Player.MediumAttack.WasPressedThisFrame() || playerInputSet.Player.HeavyAttack.WasPressedThisFrame();
        if (attackAttampted)
            return assignedPlayer.CanAttack();
        
        return false;
    }
}
