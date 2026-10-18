using UnityEngine;

public class PlayerPhoneState : PlayerState
{
    public PlayerPhoneState(Player player, StateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetPhoneActive(true);
    }

    public override void Update()
    {
        base.Update();
        if (input.Player.Phone.WasPerformedThisFrame())
        {
            player.ClosePhone();
        }
    }

    public override void Exit()
    {
        base.Exit();

        player.SetPhoneActive(false);
    }
}
