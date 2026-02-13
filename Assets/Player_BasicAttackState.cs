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
        comboAttackQueued = false;
        ResetComboIndexIfNeeded();
        DetermineAttackDirection();

        playerAnimator.SetInteger("basicAttackIndex", comboIndex);
        ApplyAttackVelocity();
    }

    private void DetermineAttackDirection()
    {
        attackDirecition = assignedPlayer.MovementInput.x != 0 ? (int)assignedPlayer.MovementInput.x : assignedPlayer.CurrentDirectionX;
    }

    public override void Update()
    {
        base.Update();
        HandleAttackVelocity();

        if(assignedPlayer.InputSet.Player.Attack.WasPressedThisFrame())
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
        assignedPlayer.SetVelocity(attackDirecition * currentAttackVelocity.x, currentAttackVelocity.y);
    }

    private void ResetComboIndexIfNeeded()
    {
        if (Time.time > previousAttackTime + assignedPlayer.ComboRestTime)
            comboIndex = FirstComboIndex;
        
        comboIndex = comboIndex > MaxComboIndex ? FirstComboIndex : comboIndex;
    }

}
