using System;
using UnityEditor.AnimatedValues;
using UnityEngine;

public class Player_BasicAttackState : EntityState
{
    private const int FirstComboIndex = 1;
    private int MaxComboIndex = 3;
    private float attackVelocityTimer;
    private int comboIndex = 1;

    private float previousAttackTime;

    public Player_BasicAttackState(Player player, StateMachine machine) : base(player, machine, "basicAttack")
    {
        if (MaxComboIndex != player.AttackVelocity.Length)
        {
            MaxComboIndex = player.AttackVelocity.Length;
            Debug.LogWarning($"Max combo index was set to {MaxComboIndex} to match the length of the AttackVelocity array in the Player script.");
        }
    }

    override public void Enter()
    {
        base.Enter();
       
        ResetComboIndexIfNeeded();

        playerAnimator.SetInteger("basicAttackIndex", comboIndex);
        ApplyAttackVelocity();
    }

    public override void Update()
    {
        base.Update();
        HandleAttackVelocity();

        if (triggerCalled)
            assignedMachine.ChangeState(assignedPlayer.IdleState);
    }

    public override void Exit()
    {
        base.Exit();
        comboIndex++;
        previousAttackTime = Time.time;
    }

    private void HandleAttackVelocity()
    {
        attackVelocityTimer -= Time.deltaTime;
    
        if(attackVelocityTimer < 0f)
           assignedPlayer.SetVelocity(0f, playerRigidbody2D.linearVelocityY);
    }

    private void ApplyAttackVelocity()
    {
        attackVelocityTimer = assignedPlayer.AttackVelocityDuration;
        var currentAttackVelocity = assignedPlayer.AttackVelocity[comboIndex - 1];
        assignedPlayer.SetVelocity(assignedPlayer.CurrentDirectionX * currentAttackVelocity.x, currentAttackVelocity.y);
    }

    private void ResetComboIndexIfNeeded()
    {
        if (Time.time > previousAttackTime + assignedPlayer.ComboRestTime)
            comboIndex = FirstComboIndex;
        
        comboIndex = comboIndex > MaxComboIndex ? FirstComboIndex : comboIndex;
    }

}
