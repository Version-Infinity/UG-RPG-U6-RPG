using UnityEngine;

public class EntityState 
{
    protected StateMachine assignedMachine;
    protected string name;

    public EntityState(StateMachine machine, string name)
    {
        this.assignedMachine = machine;
        this.name = name;
    }

    public virtual void Enter()
    {
        Debug.Log($"{nameof(Enter)} {name}");
    }

    public virtual void Update()
    {
        Debug.Log($"{nameof(Update)} {name}");
    }

    public virtual void Exit()
    {
        Debug.Log($"{nameof(Exit)} {name}");
    }
}
