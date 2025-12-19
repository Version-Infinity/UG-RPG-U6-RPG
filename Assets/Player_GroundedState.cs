using UnityEngine;

public class Player_GroundedState : EntityState
{
    public Player_GroundedState(Player player, StateMachine machine, string name) : base(player, machine, name)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if(playerInputSet.Player.Jump.WasPressedThisFrame())
        {
            assignedMachine.ChangeState(assignedPlayer.JumpState);
        }

    }
}
