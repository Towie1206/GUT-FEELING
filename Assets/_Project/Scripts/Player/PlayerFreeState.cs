using UnityEngine;

public class PlayerFreeState : PlayerState
{
    public PlayerFreeState(Player player, StateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public override void Update()
    {
        base.Update();
        player.movement.SetMoveInput(player.moveInput);
        player.look.Look(input.Player.Look.ReadValue<Vector2>());
    }
    public override void Exit()
    {
        base.Exit();
        player.movement.Stop();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
