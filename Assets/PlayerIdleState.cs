using UnityEngine;

public class PlayerIdleState : EntityState
{
    public PlayerIdleState(Player player, StateMachine machine) : base(player, machine, "Idle State")
    {
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            assignedMachine.ChangeState(player.MoveState);
        }
    }
}
