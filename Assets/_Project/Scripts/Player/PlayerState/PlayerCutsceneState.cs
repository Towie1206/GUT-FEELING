using UnityEngine;

public class PlayerCutsceneState : PlayerState
{
    public PlayerCutsceneState(Player player, StateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.movement.Stop();
        player.movement.SetMoveInput(Vector2.zero);
        if (player.look != null)
        {
            player.look.enabled = false;
        }

        if (player.interaction != null)
        {
            player.interaction.enabled = false;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
        if (player.look != null)
        {
            player.look.enabled = true;
            player.look.ResetCameraTransform();
        }

        if (player.interaction != null)
        {
            player.interaction.enabled = true;
        }
    }
}
