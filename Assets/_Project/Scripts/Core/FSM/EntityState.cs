using UnityEngine;

public abstract class EntityState
{
    protected StateMachine stateMachine;

    protected float stateTimer;

    public EntityState(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
    public virtual void Enter()
    {

    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {

    }
}
