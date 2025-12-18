using UnityEngine;

public class PlayerMoveState : EntityState
{
    public PlayerMoveState(Player player, StateMachine machine) : base(player, machine, "Move State")
    {
    }
} 