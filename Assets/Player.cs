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
    public float MoveSpeed;
    public float JumpForce;
    [SerializeField] private EntityDirection currentDirection;

    public void Awake()
    { 
        currentDirection = EntityDirection.Right;
        MoveSpeed = 8f;
        JumpForce = 5f;
        Animator = GetComponentInChildren<Animator>();
        Rigidbody = GetComponent<Rigidbody2D>();
        InputSet = new PlayerInputSet();

        machine = new StateMachine();
        IdleState = new Player_IdleState(this, machine);
        MoveState = new Player_MoveState(this, machine);
        JumpState = new Player_JumpState(this, machine);
        FallState = new Player_FallState(this, machine);
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
    }

    public void SetVelocity(float x, float y)
    {
        Rigidbody.linearVelocityX = x;
        Rigidbody.linearVelocityY = y;
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
}
