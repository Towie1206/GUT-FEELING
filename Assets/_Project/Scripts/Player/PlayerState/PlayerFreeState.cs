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

        // 1. Chạy nhanh (Giữ Left Shift) Ngồi khom (Bấm C hoặc Left Ctrl)
        player.movement.UpdateMovementSpeed(input.Player.Sprint.IsPressed(), input.Player.Crouch.IsPressed());
    
        // 3. Zoom Camera (Giữ Chuột Phải)
        player.look.SetZoom(input.Player.Zoom.IsPressed());

        if(input.Player.Interact.WasPressedThisFrame())
        {
            player.interaction.TryToInteract();
        }

        if (input.Player.Toggle.WasPressedThisFrame()) 
        {
            player.hand.UseCurrentItem();
        }

        if (input.Player.Throw.WasPressedThisFrame())
        {
            player.hand.DropCurrentItem(player.look.transform.forward);
        }

        player.hand.UpdateSway(input.Player.Look.ReadValue<Vector2>().x);
    }
    public override void Exit()
    {
        base.Exit();
        player.movement.Stop();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
