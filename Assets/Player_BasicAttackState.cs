using UnityEngine;

public class Player_BasicAttackState : EntityState
{
    private float attackVelocityTimer;

    public Player_BasicAttackState(Player player, StateMachine machine) : base(player, machine, "basicAttack")
    {
    }

    override public void Enter()
    {
        base.Enter();
        InitAttackMovement();
    }

    public override void Update()
    {
        base.Update();
        HandleAttackVelocity();

        if (triggerCalled)
            assignedMachine.ChangeState(assignedPlayer.IdleState);
    }

    private void HandleAttackVelocity()
    {
        attackVelocityTimer -= Time.deltaTime;
    
        if(attackVelocityTimer < 0f)
           assignedPlayer.SetVelocity(0f, playerRigidbody2D.linearVelocityY);
    }

    private void InitAttackMovement()
    {
        attackVelocityTimer = assignedPlayer.AttackVelocityDuration;
        assignedPlayer.SetVelocity(assignedPlayer.CurrentDirectionX * assignedPlayer.AttackVelocity.x, assignedPlayer.AttackVelocity.y);
    }
}
