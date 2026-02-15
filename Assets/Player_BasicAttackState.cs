using System;
using Unity.VisualScripting;
using UnityEngine;

public class Player_BasicAttackState : EntityState
{
    // Number of Attack Types
    private const int FirstComboIndex = 1;
    private int MaxComboIndex = 3;

    //Combo Attack State Variables
    private bool comboAttackQueued;
    private int attackDirecition;
    private int comboIndex = 1;
    private float attackVelocityTimer;
    private float previousAttackTime;
    private float preAttackVelocityX;

    public Player_BasicAttackState(Player player, StateMachine machine) : base(player, machine, "basicAttack")
    {
        if (MaxComboIndex != player.AttackVelocity.Length)
            MaxComboIndex = player.AttackVelocity.Length;
    }

    override public void Enter()
    {
        base.Enter();
        comboAttackQueued = false;
        preAttackVelocityX = playerRigidbody2D.angularVelocity;


        ResetComboIndexIfNeeded();
        DetermineAttackDirection();
        DetectAttackType();

        playerAnimator.SetInteger("basicAttackIndex", comboIndex);
        ApplyAttackVelocity();
    }

    private void DetectAttackType()
    {
        var input = playerInputSet.Player;
        if (input.LightAttack.WasPressedThisFrame())
            comboIndex = 1;
        else if (input.MediumAttack.WasPressedThisFrame())
            comboIndex = 2;
        else if (input.HeavyAttack.WasPressedThisFrame())
            comboIndex = 3;
    }

    private void DetermineAttackDirection()
    {
        attackDirecition = assignedPlayer.MovementInput.x != 0 ? (int)assignedPlayer.MovementInput.x : assignedPlayer.CurrentDirectionX;
    }

    public override void Update()
    {
        base.Update();
        HandleAttackVelocity();

        if(assignedPlayer.InputSet.Player.Attack.WasPressedThisFrame() && assignedPlayer.CanAttack())
            QueueNextAttack();

        if (triggerCalled)
            HandleStateExit();
    }

    private void HandleStateExit()
    {
        if (comboAttackQueued)
        {
            playerAnimator.SetBool(animatorBoolKey, false);
            assignedPlayer.QueueAttackState();
        }
        else
            assignedMachine.ChangeState(assignedPlayer.IdleState);
    }

    public override void Exit()
    {
        base.Exit();
        comboIndex++;
        previousAttackTime = Time.time;
    }


    private void QueueNextAttack()
    {
        if (comboIndex < MaxComboIndex)
            comboAttackQueued = true;
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
        assignedPlayer.SetVelocity((attackDirecition * currentAttackVelocity.x) + preAttackVelocityX, currentAttackVelocity.y);
    }

    private void ResetComboIndexIfNeeded()
    {
        if (Time.time > previousAttackTime + assignedPlayer.ComboRestTime)
            comboIndex = FirstComboIndex;
        
        comboIndex = comboIndex > MaxComboIndex ? FirstComboIndex : comboIndex;
    }

}
