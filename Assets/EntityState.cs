using UnityEngine;

public abstract class EntityState 
{
    protected StateMachine assignedMachine;
    protected string animatorBoolKey;
    protected Player assignedPlayer;
    protected Animator playerAnimator;
    protected Rigidbody2D playerRigidbody2D;

    public EntityState(Player player, StateMachine machine, string name)
    {
        assignedPlayer = player;
        assignedMachine = machine;
        animatorBoolKey = name;
        playerAnimator = player.Animator;
        playerRigidbody2D = player.Rigidbody;
    }

    public virtual void Enter()
    {
        playerAnimator.SetBool(animatorBoolKey, true);
    }

    public virtual void Update()
    {
        Debug.Log($"{nameof(Update)} {animatorBoolKey}");
    }

    public virtual void Exit()
    {
        playerAnimator.SetBool(animatorBoolKey, false);
    }
}
