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
    }
}
