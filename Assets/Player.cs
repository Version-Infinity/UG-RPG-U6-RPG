using UnityEngine;

public class Player : MonoBehaviour
{
    private StateMachine machine;
;
    private EntityState idleState;

    public void Awake()
    {
        machine = new StateMachine();
        idleState = new EntityState(machine, "Idle");
    }

    public void Start()
    {
        machine.Initialize(idleState);
    }

    public void Update()
    {
        machine.CurrentState.Update();
    }
}
