using UnityEngine;

public class Player_IdleState : EntityState
{
    public Player_IdleState(Player player, StateMachine machine) : base(player, machine, "Idle State")
    {
    }

    public override void Update()
    {
        base.Update();

        if (player.MovementInput.x != 0)
            assignedMachine.ChangeState(player.MoveState);
    }
}
