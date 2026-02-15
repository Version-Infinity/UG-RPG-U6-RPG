using System;
using UnityEngine;

public class Player_JumpState : Player_AiredState
{
    public Player_JumpState(Player player, StateMachine machine) : base(player, machine, "jumpFall")
    {
        playerInputSet.Player.Jump.canceled += ctx => CheckForJumpCancelation();
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

    private void CheckForJumpCancelation()
    {
       if(playerInputSet.Player.Jump.IsPressed())
           Debug.Log("Still Jumping");
        else { 
            Debug.Log("Jump Canceled");
            if (playerRigidbody2D.linearVelocityY > 0)
                assignedPlayer.SetVelocity(playerRigidbody2D.linearVelocityX, -1);
        }
    }
}
