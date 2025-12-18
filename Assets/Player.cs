using UnityEngine;

public class Player : MonoBehaviour
{
    private StateMachine machine;
    public PlayerIdleState IdelState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }

    public void Awake()
    {
        machine = new StateMachine();
        IdelState = new PlayerIdleState(this, machine);
        MoveState = new PlayerMoveState(this, machine);
    }

    public void Start()
    {
        machine.Initialize(IdelState);
    }

    public void Update()
    {
        machine.CurrentState.Update();
    }
}
