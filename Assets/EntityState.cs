using UnityEngine;

public abstract class EntityState 
{
    protected StateMachine assignedMachine;
    protected string animatorBoolKey;
    protected Player assignedPlayer;
    protected Animator playerAnimator;
    protected Rigidbody2D playerRigidbody2D;
    protected PlayerInputSet playerInputSet;

    public EntityState(Player player, StateMachine machine, string name)
    {
        assignedPlayer = player;
        assignedMachine = machine;
        animatorBoolKey = name;
        playerAnimator = player.Animator;
        playerRigidbody2D = player.Rigidbody;
        playerInputSet = player.InputSet;
    }

    public virtual void Enter()
    {
        playerAnimator.SetBool(animatorBoolKey, true);
    }

    public virtual void Update()
    {
        playerAnimator.SetFloat("yVelocity", playerRigidbody2D.linearVelocityY);
    }

    public virtual void Exit()
    {
        playerAnimator.SetBool(animatorBoolKey, false);
    }
}
