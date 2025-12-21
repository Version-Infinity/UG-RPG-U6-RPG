using UnityEngine;

public class Player_FallState : Player_AiredState
{
    public Player_FallState(Player player, StateMachine machine) : base(player, machine, "jumpFall")
    {
    }
}
