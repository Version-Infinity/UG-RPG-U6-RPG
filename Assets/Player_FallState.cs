using UnityEngine;

public class Player_FallState : EntityState
{
    public Player_FallState(Player player, StateMachine machine) : base(player, machine, "jumpFall")
    {
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();
        //if (playerRigidbody2D.IsGrounded())
        //{
        //    assignedMachine.ChangeState(assignedPlayer.IdleState);
        //}
    }
}
