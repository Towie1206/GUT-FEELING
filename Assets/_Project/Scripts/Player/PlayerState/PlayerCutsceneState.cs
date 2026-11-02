using UnityEngine;

/// <summary>
/// Trạng thái khóa toàn bộ điều khiển di chuyển và xoay góc nhìn của Player
/// khi đang diễn ra Cutscene (Timeline + Cinemachine blend) hoặc đang ngồi chơi máy tính PC 2D.
/// Kế thừa trực tiếp từ PlayerState trong kiến trúc FSM hiện có.
/// </summary>
public class PlayerCutsceneState : PlayerState
{
    public PlayerCutsceneState(Player player, StateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // TODO:
        // 1. Dừng ngay lập tức vận tốc của nhân vật:
        //    player.movement.Stop();
        //    player.movement.SetMoveInput(Vector2.zero);
        // 2. Hủy mọi tiến trình đang giữ phím tương tác nếu có:
        //    player.interaction.CancelHold();
        // 3. Ẩn con trỏ chuột và khóa vào giữa màn hình để xem Cutscene trọn vẹn:
        //    Cursor.lockState = CursorLockMode.Locked;
        //    Cursor.visible = false;
    }

    public override void Update()
    {
        base.Update();

        // TODO:
        // ĐỂ TRỐNG (Không gọi player.movement hay player.look ở đây)
        // để CinemachineBrain và Unity Timeline toàn quyền điều khiển góc máy quay!
    }

    public override void Exit()
    {
        base.Exit();

        // TODO (Tùy chọn):
        // Khôi phục các trạng thái cần thiết khi kết thúc Cutscene và quay lại PlayerFreeState
    }
}
