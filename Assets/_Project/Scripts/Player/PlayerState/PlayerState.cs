using UnityEngine;

public class PlayerState : EntityState
{
    protected Player player;
    protected PlayerInput input;

    public PlayerState(Player player, StateMachine stateMachine) : base(stateMachine)
    {
        this.player = player;
        this.input = player.input;
    }

   
}
