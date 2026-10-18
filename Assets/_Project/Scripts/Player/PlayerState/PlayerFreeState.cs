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
        player.look.Look(player.mousePosition);

        // 1. Chạy nhanh (Giữ Left Shift) Ngồi khom (Bấm C hoặc Left Ctrl)
        player.movement.UpdateMovementSpeed(input.Player.Sprint.IsPressed(), input.Player.Crouch.IsPressed());
    
        // 3. Zoom Camera (Giữ Chuột Phải)
        player.look.SetZoom(input.Player.Zoom.IsPressed());

        if(input.Player.Interact.WasPressedThisFrame())
        {
            player.interaction.TryToInteract(player);
            player.interaction.StartHoldWorld();
        }

        if (input.Player.Toggle.WasPressedThisFrame()) 
        {
            player.hand.UseCurrentItem();
        }

        if (input.Player.Throw.WasPressedThisFrame())
        {
            player.hand.DropCurrentItem(player.look.CameraTransform.forward);
        }


        if (input.Player.Interact.IsPressed())
        {
            player.interaction.UpdateHold(player);
        }

        if (input.Player.Interact.WasReleasedThisFrame())
        {
            player.interaction.CancelHold();
        }

        if (input.Player.AltUse.WasPressedThisFrame())
        {
            player.interaction.StartHoldHand(player);
        }
        if (input.Player.AltUse.IsPressed())
        {
            player.interaction.UpdateHold(player);
        }
        if (input.Player.AltUse.WasReleasedThisFrame())
        {
            player.interaction.CancelHold();
        }
        if(input.Player.Phone.WasPerformedThisFrame())
        {
            stateMachine.ChangeState(player.phoneState);
        }    

        player.hand.UpdateSway(player.mousePosition.x);
    }
    public override void Exit()
    {
        base.Exit();
        player.movement.Stop();
        player.interaction.CancelHold();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
