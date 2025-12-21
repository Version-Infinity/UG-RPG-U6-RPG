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


    public Vector2 MovementInput { get; private set; } = Vector2.zero;

    [Header("Movement Settings")]
    public float MoveSpeed = 8f;
    public float JumpForce = 12f;
    [Range(0, 1)]
    public float InAirMoveMultiplier = 0.5f;
    [SerializeField] private EntityDirection currentDirection = EntityDirection.Right;

    [Header("Ground Detection Settings")]
    [SerializeField] private float rayLength = 0.8f;
    [SerializeField] private LayerMask groundLayer;
    public bool Grounded { get; private set; }

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

        // Set initial rotation based on currentDirection
        if (currentDirection == EntityDirection.Left)
        {
            transform.Rotate(0f, 180f, 0f);
        }
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
        if (xVelocity > 0 && currentDirection == EntityDirection.Left)
            FlipEntityDirection();
        else if (xVelocity < 0 && currentDirection == EntityDirection.Right)
            FlipEntityDirection();
    }

    private void FlipEntityDirection()
    {
        if (currentDirection == EntityDirection.Right)
            currentDirection = EntityDirection.Left;
        else
            currentDirection = EntityDirection.Right; 

        transform.Rotate(0f, 180f, 0f);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -rayLength));
    }

    public void HandleCollisionDection()
    {
        Grounded = Physics2D.Raycast(transform.position, Vector2.down, rayLength, groundLayer);
    }

    public bool IsGrounded()
    {
        return Grounded;
    }

}
