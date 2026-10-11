using UnityEngine;

public class PlayerInspectState : PlayerState
{
    public PlayerInspectState(Player player, StateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.movement.Stop();
        player.movement.SetMoveInput(Vector2.zero);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    public override void Update()
    {
        base.Update();
        Vector2 mouseDelta = input.Player.Look.ReadValue<Vector2>();
        if (input.Player.InspectRotate.IsPressed())
        {
            player.inspect.RotateCurrentItem(mouseDelta);
        }

        if (input.Player.Interact.WasPressedThisFrame() || input.Player.Zoom.WasPressedThisFrame())
        {
            player.inspect.StopInspect();
            stateMachine.ChangeState(player.freeState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
