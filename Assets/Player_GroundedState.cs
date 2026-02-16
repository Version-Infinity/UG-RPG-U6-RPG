using UnityEngine;

public class Player_GroundedState : EntityState
{
    public Player_GroundedState(Player player, StateMachine machine, string name) : base(player, machine, name)
    {
    }

    public override void Enter()
    {
        base.Enter();
        assignedPlayer.ResetCanAttack();
    }

    public override void Update()
    {
        base.Update();

        if (!assignedPlayer.IsGrounded())
            assignedMachine.ChangeState(assignedPlayer.FallState);

        if (playerInputSet.Player.Jump.WasPressedThisFrame())
            assignedMachine.ChangeState(assignedPlayer.JumpState);

        if (attackInputDetected())
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
