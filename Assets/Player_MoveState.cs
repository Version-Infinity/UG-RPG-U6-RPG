using UnityEngine;

public class Player_MoveState : EntityState
{
    public Player_MoveState(Player player, StateMachine machine) : base(player, machine, "move")
    {
    }

    public override void Update()
    {
        base.Update();

        if (assignedPlayer.MovementInput.x == 0)
            assignedMachine.ChangeState(assignedPlayer.IdleState);

        assignedPlayer.SetVelocity(assignedPlayer.MovementInput.x * assignedPlayer.MoveSpeed, playerRigidbody2D.linearVelocityY);
    }

} 