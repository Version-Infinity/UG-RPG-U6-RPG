using UnityEngine;

public class Player_MoveState : EntityState
{
    public Player_MoveState(Player player, StateMachine machine) : base(player, machine, "Move State")
    {
    }

    public override void Update()
    {
        base.Update();

        if (player.MovementInput.x == 0)
            assignedMachine.ChangeState(player.IdleState);
    }
} 