using UnityEngine;

public abstract class EntityState 
{
    protected StateMachine assignedMachine;
    protected string name;
    protected Player player;
    private StateMachine machine;
    private string v;

    public EntityState(Player player, StateMachine machine, string name)
    {
        this.player = player;
        this.assignedMachine = machine;
        this.name = name;
    }

    protected EntityState(StateMachine machine, string v)
    {
        this.machine = machine;
        this.v = v;
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
