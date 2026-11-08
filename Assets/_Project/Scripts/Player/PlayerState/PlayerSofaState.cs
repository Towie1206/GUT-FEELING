using UnityEngine;

public class PlayerSofaState : PlayerState
{
    public PlayerSofaState(Player player, StateMachine stateMachine) : base(player, stateMachine)
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
        player.look.Look(player.mousePosition);

        player.look.SetZoom(input.Player.Zoom.IsPressed());

        if (input.Player.Interact.WasPressedThisFrame())
        {
            player.interaction.TryToInteract(player);
            player.interaction.StartHoldWorld();
        }

        if (input.Player.Toggle.WasPressedThisFrame())
        {
            player.hand.UseCurrentItem();
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
        if (input.Player.Phone.WasPerformedThisFrame())
        {
            stateMachine.ChangeState(player.phoneState);
        }
        if (input.Player.StandUp.WasPerformedThisFrame())
        {
            player.movement.StandUpFromSofa();
            stateMachine.ChangeState(player.freeState);
        }

        player.hand.UpdateSway(player.mousePosition.x);
    }

    public override void Exit()
    {
        base.Exit();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        player.interaction.CancelHold(); //để nhỡ đang giữ nút ăn mì mà bấm mở điện thoại hoặc đứng dậy thì nó ngắt thanh tiến trình ăn mì.
    }
}
