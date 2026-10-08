public class StateMachine
{
    public EntityState currentState { get; private set; }

    public bool canChangeState;

    public void Initialize(EntityState startingState)
    {
        canChangeState = true;
        currentState = startingState;
        currentState.Enter();
    }

    public void ChangeState(EntityState newState)
    {
        if (!canChangeState)
            return;

        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
