using UnityEditor;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class Player : MonoBehaviour
{
    private StateMachine machine;
    public PlayerInputSet InputSet { get; private set; }

    public Rigidbody2D Rigidbody { get; private set; }
    public Animator Animator { get; private set; }
    public Player_IdleState IdleState { get; private set; }
    public Player_MoveState MoveState { get; private set; }
    public Player_JumpState JumpState { get; private set; }
    public Player_FallState FallState { get; private set; }
    public Player_WallSlideState WallSlideState { get; private set; }
    public Player_WallJumpState WallJumpState { get; private set; }

    public Vector2 MovementInput { get; private set; } = Vector2.zero;

    [Header("Movement Settings")]
    public EntityDirection CurrentDirection { get; private set; } = EntityDirection.Right;
    public int CurrentDirectionX { get { return CurrentDirection == EntityDirection.Right ? 1 : -1; } }
    public float MoveSpeed = 8f;
    public float JumpForce = 12f;
    public float WallSlideSpeed = .3f;
    [Range(0, 1)]
    public float InAirMoveMultiplier = 0.5f;
    public Vector2 WallJumpForce;


    [Header("Ground Detection Settings")]
    [SerializeField] private float groundCheckDistance = 1.4f;
    [SerializeField] private float wallCheckDistance = 0.5f;
    [SerializeField] private LayerMask groundLayer;
    public bool Grounded { get; private set; }
    public bool WallDetected { get; private set; }

    public void Awake()
    { 
        Animator = GetComponentInChildren<Animator>();
        Rigidbody = GetComponent<Rigidbody2D>();
        InputSet = new PlayerInputSet();

        machine = new StateMachine();
        IdleState = new Player_IdleState(this, machine);
        MoveState = new Player_MoveState(this, machine);
        JumpState = new Player_JumpState(this, machine);
        FallState = new Player_FallState(this, machine);
        WallSlideState = new Player_WallSlideState(this, machine);
        WallJumpState = new Player_WallJumpState(this, machine);
        ProcessInititalState();
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
        machine.Initialize(IdleState);
    }

    public void Update()
    {
        machine.UpdateCurrentState();
        HandleCollisionDection();
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


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.purple;
        
        // Ground Check
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));

        // Wall Check
        int directionMultiplier = CurrentDirection == EntityDirection.Right ? 1 : -1;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(wallCheckDistance * directionMultiplier, 0));
    }

    public void HandleCollisionDection()
    {
        Grounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        WallDetected = Physics2D.Raycast(transform.position, CurrentDirection == EntityDirection.Right ? Vector2.right : Vector2.left, wallCheckDistance, groundLayer);
    }

    public bool IsGrounded()
    {
        return Grounded;
    }

    public bool IsTouchingWall()
    {
        return WallDetected;
    }

}
