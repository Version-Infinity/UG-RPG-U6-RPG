using System;
using System.Collections;
using UnityEditor;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Player : MonoBehaviour
{
    private StateMachine stateMachine;
    public PlayerInputSet InputSet { get; private set; }

    public Rigidbody2D Rigidbody { get; private set; }
    public Animator Animator { get; private set; }
    public Player_IdleState IdleState { get; private set; }
    public Player_MoveState MoveState { get; private set; }
    public Player_JumpState JumpState { get; private set; }
    public Player_FallState FallState { get; private set; }
    public Player_WallSlideState WallSlideState { get; private set; }
    public Player_WallJumpState WallJumpState { get; private set; }
    public Player_DashState DashState { get; private set; }
    public Player_BasicAttackState BasicAttackState { get; private set; }
    public Player_EdgeGrabState EdgeGrabState { get; private set; }

    public Vector2 MovementInput { get; private set; } = Vector2.zero;

    [Header("Attack Settings")]
    public Vector2[] AttackVelocity;
    public float AttackVelocityDuration = .1f;
    public float ComboRestTime = 1f;
    private Coroutine queuedAttackState;
    private bool canAttack = true;

    [Header("Movement Settings")]
    public EntityDirection CurrentDirection { get; private set; } = EntityDirection.Right;
    public int CurrentDirectionX { get { return CurrentDirection == EntityDirection.Right ? 1 : -1; } }
    public float MoveSpeed = 8f;
    public float JumpForce = 12f;
    public Vector2 WallJumpForce;

    [Range(0, 1)]
    public float WallSlideSpeed = .3f;
    [Range(0, 1)]
    public float InAirMoveMultiplier = 0.5f;
    public float edgeCheckHeight = 1f;
    public float edgeAirDetectionOffset = .15f;

    [Space]
    public float DashDuration = .25f;
    public float DashSpeed = 20f;

    [Header("Ground Detection Settings")]
    [SerializeField] private float groundCheckDistance = 1.4f;
    [SerializeField] private float wallCheckDistance = 0.5f;
    [SerializeField] private LayerMask groundLayer;
    private bool grounded;
    private bool wallDetected;
    private bool edgeGrabDetected;

    public void Awake()
    { 
        Animator = GetComponentInChildren<Animator>();
        Rigidbody = GetComponent<Rigidbody2D>();
        InputSet = new PlayerInputSet();

        stateMachine = new StateMachine();
        IdleState = new Player_IdleState(this, stateMachine);
        MoveState = new Player_MoveState(this, stateMachine);
        JumpState = new Player_JumpState(this, stateMachine);
        FallState = new Player_FallState(this, stateMachine);
        WallSlideState = new Player_WallSlideState(this, stateMachine);
        WallJumpState = new Player_WallJumpState(this, stateMachine);
        DashState = new Player_DashState(this, stateMachine);
        BasicAttackState = new Player_BasicAttackState(this, stateMachine);
        EdgeGrabState = new Player_EdgeGrabState(this, stateMachine);

        ProcessInititalState();

        Debug.LogWarning("TODO: Fix wall slide entered when head alone touches wall");
    }

    private void OnValidate()
    {
        ProcessInititalState();
    }

    private void ProcessInititalState()
    {
        if (CurrentDirection == EntityDirection.Left)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        HandleCollisionDection();
    }

    private void OnEnable()
    { 
        InputSet.Enable();

        InputSet.Player.Movement.performed += ctx => MovementInput = ctx.ReadValue<Vector2>();

        InputSet.Player.Movement.canceled += ctx => MovementInput = Vector2.zero;
    }

    private void OnDisable()
    {
        InputSet.Disable();
    }

    public void Start()
    {
        stateMachine.Initialize(IdleState);
    }

    public void Update()
    {
        stateMachine.UpdateCurrentState();
        HandleCollisionDection();
    }

    private IEnumerator EnterAttackStateWithDelayCo() 
    { 
        yield return new WaitForEndOfFrame();
        stateMachine.ChangeState(BasicAttackState);
    }

    public void QueueAttackState()
    {
        if (queuedAttackState != null)
            StopCoroutine(queuedAttackState);
        queuedAttackState = StartCoroutine(EnterAttackStateWithDelayCo());
    }

    public void SetVelocity(float x, float y)
    {
        Rigidbody.linearVelocity = new Vector2(x, y);
        HandleFlip(x);
    }

    private void HandleFlip(float xVelocity)
    {
        if (xVelocity > 0 && CurrentDirection == EntityDirection.Left)
            FlipDirection();
        else if (xVelocity < 0 && CurrentDirection == EntityDirection.Right)
            FlipDirection();
    }

    public void FlipDirection()
    {
        if (CurrentDirection == EntityDirection.Right)
            CurrentDirection = EntityDirection.Left;
        else
            CurrentDirection = EntityDirection.Right; 

        transform.Rotate(0f, 180f, 0f);
    }

    public void CallAnimationTrigger()
    {
        stateMachine.CurrentState.CallAnimationTrigger();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.purple;
        
        // Ground Check
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));

        // Wall Check
        int directionMultiplier = CurrentDirection == EntityDirection.Right ? 1 : -1;
        //Gizmos.DrawLine(transform.position, transform.position + new Vector3(wallCheckDistance * directionMultiplier, 0));

        // Edge Check
        Gizmos.DrawLine(transform.position + new Vector3(0, edgeCheckHeight), transform.position + new Vector3(wallCheckDistance * directionMultiplier, edgeCheckHeight));
        // Check Air Above Edge
        Gizmos.DrawLine(transform.position + new Vector3(0, edgeCheckHeight + edgeAirDetectionOffset), transform.position + new Vector3(wallCheckDistance * directionMultiplier, edgeCheckHeight + edgeAirDetectionOffset));
        // /* Check Foot Ledge */ Gizmos.DrawLine(transform.position + new Vector3(wallCheckDistance * directionMultiplier, 0), transform.position + new Vector3(wallCheckDistance * directionMultiplier, -groundCheckDistance));
    }

    public void HandleCollisionDection()
    {
        grounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        //wallDetected = Physics2D.Raycast(transform.position, CurrentDirection == EntityDirection.Right ? Vector2.right : Vector2.left, wallCheckDistance, groundLayer);
        wallDetected = Physics2D.Raycast(transform.position + new Vector3(0, edgeCheckHeight), CurrentDirection == EntityDirection.Right ? Vector2.right : Vector2.left, wallCheckDistance, groundLayer);
        bool airAboveEdge = Physics2D.Raycast(transform.position + new Vector3(0, edgeCheckHeight + edgeAirDetectionOffset), CurrentDirection == EntityDirection.Right ? Vector2.right : Vector2.left, wallCheckDistance, groundLayer);
        edgeGrabDetected = wallDetected && !airAboveEdge;
    }

    public bool CanAttack()
    {
        float time = Time.time;

        if (!IsGrounded())
            if (canAttack)
            {
                canAttack = false;
                return true;
            }
            else
            {
                return false;
            }

        return canAttack;
    }

    public void ResetCanAttack()
    {
        canAttack = IsGrounded();
    }

    public bool IsGrounded()
    {
        return grounded;
    }

    public bool IsTouchingWall()
    {
        return wallDetected;
    }

    public bool CanGrabEdge()
    {
        return edgeGrabDetected;
    }
}
