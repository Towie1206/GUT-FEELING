using System;
using UnityEngine.Playables;

/// <summary>
/// Hợp đồng dịch vụ điều khiển các phân cảnh điện ảnh (Unity Timeline & Cinemachine Impulse).
/// Giúp tách biệt (Decouple) các Trigger cốt truyện khỏi chi tiết cài đặt của PlayableDirector.
/// </summary>
public interface ICutsceneService
{
    /// <summary>
    /// Cho biết hiện tại có đang phát một đoạn Cutscene nào hay không.
    /// </summary>
    bool IsPlaying { get; }

    /// <summary>
    /// Sự kiện phát ra ngay khi bắt đầu Cutscene (để Player chuyển sang PlayerCutsceneState).
    /// </summary>
    event Action OnCutsceneStarted;

    /// <summary>
    /// Sự kiện phát ra ngay khi Cutscene kết thúc (để trả quyền điều khiển về PlayerFreeState).
    /// </summary>
    event Action OnCutsceneEnded;

    /// <summary>
    /// Phát một Timeline cụ thể thông qua PlayableDirector và gọi callback khi hoàn tất.
    /// </summary>
    void PlayTimeline(PlayableDirector director, Action onComplete = null);

    /// <summary>
    /// Kích hoạt rung lắc camera qua CinemachineImpulseSource (sấm nổ, đập cửa, hất ngã).
    /// </summary>
    void TriggerCameraShake(float impulseForce = 1.0f);
}
