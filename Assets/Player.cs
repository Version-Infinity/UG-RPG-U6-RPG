using UnityEngine;

public class Player : MonoBehaviour
{
    private StateMachine machine;
    private PlayerInputSet inputSet;

    public Player_IdleState IdleState { get; private set; }
    public Player_MoveState MoveState { get; private set; }

    public Vector2 MovementInput { get; private set; } = Vector2.zero;

    public void Awake()
    {
        machine = new StateMachine();
        inputSet = new PlayerInputSet();
        IdleState = new Player_IdleState(this, machine);
        MoveState = new Player_MoveState(this, machine);
    }

    private void OnEnable()
    { 
        inputSet.Enable();

        inputSet.Player.Movement.performed += ctx => MovementInput = ctx.ReadValue<Vector2>();

        inputSet.Player.Movement.canceled += ctx => MovementInput = Vector2.zero;
    }

    private void OnDisable()
    {
        inputSet.Disable();
    }

    public void Start()
    {
        machine.Initialize(IdleState);
    }

    public void Update()
    {
        machine.CurrentState.Update();
    }
}
