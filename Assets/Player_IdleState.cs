using UnityEngine;

public class Player_IdleState : EntityState
{
    public Player_IdleState(Player player, StateMachine machine) : base(player, machine, "idle")
    {
    }

    public override void Update()
    {
        base.Update();

        if (assignedPlayer.MovementInput.x != 0)
            assignedMachine.ChangeState(assignedPlayer.MoveState);
    }
}
