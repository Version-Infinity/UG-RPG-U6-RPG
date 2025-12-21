using UnityEngine;

public class Player_JumpState : Player_AiredState
{
    public Player_JumpState(Player player, StateMachine machine) : base(player, machine, "jumpFall")
    {
    }

    override public void Enter()
    {
        base.Enter();
        assignedPlayer.SetVelocity(playerRigidbody2D.linearVelocityX, assignedPlayer.JumpForce);
    }

    override public void Update()
    {
        base.Update();
        if (playerRigidbody2D.linearVelocityY < 0)
            assignedMachine.ChangeState(assignedPlayer.FallState);
    }
}
