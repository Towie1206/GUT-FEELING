using System;
using UnityEngine;
using UnityEngine.Playables;
using Unity.Cinemachine;

/// <summary>
/// Bộ điều khiển trung tâm cho các phân cảnh Cutscene sử dụng Unity Timeline và CinemachineImpulseSource.
/// Thực thi hợp đồng ICutsceneService theo nguyên lý SRP và DIP.
/// </summary>
public class CutsceneController : MonoBehaviour, ICutsceneService
{
    public static ICutsceneService Instance { get; private set; }

    [Header("Default Components (Optional)")]
    [SerializeField] private PlayableDirector defaultDirector;
    [SerializeField] private CinemachineImpulseSource impulseSource;
    [SerializeField] private Player player;

    public bool IsPlaying { get; private set; }

    public event Action OnCutsceneStarted;
    public event Action OnCutsceneEnded;

    private Action currentOnComplete;

    private void Awake()
    {
        // TODO:
        // Khởi tạo Singleton Instance (nếu Instance == null thì gán Instance = this, ngược lại Destroy(gameObject))
    }

    public void PlayTimeline(PlayableDirector director, Action onComplete = null)
    {
        // TODO:
        // 1. Kiểm tra nếu director == null hoặc đang IsPlaying == true thì thoát hàm (return)
        // 2. Đặt IsPlaying = true và lưu onComplete vào biến currentOnComplete
        // 3. Đăng ký lắng nghe sự kiện khi Timeline chạy xong: director.stopped += OnDirectorStopped
        // 4. Gọi RaiseCutsceneStarted() (và/hoặc chuyển Player sang trạng thái khóa điều khiển)
        // 5. Gọi director.Play() để bắt đầu chạy Timeline
    }

    public void TriggerCameraShake(float impulseForce = 1.0f)
    {
        // TODO:
        // Kiểm tra nếu impulseSource != null -> gọi impulseSource.GenerateImpulse(impulseForce)
    }

    private void OnDirectorStopped(PlayableDirector director)
    {
        // TODO:
        // 1. Hủy đăng ký sự kiện: director.stopped -= OnDirectorStopped
        // 2. Đặt lại IsPlaying = false
        // 3. Gọi RaiseCutsceneEnded() để trả quyền điều khiển về cho người chơi
        // 4. Gọi currentOnComplete?.Invoke() rồi gán currentOnComplete = null
    }

    protected virtual void RaiseCutsceneStarted() => OnCutsceneStarted?.Invoke();
    protected virtual void RaiseCutsceneEnded() => OnCutsceneEnded?.Invoke();
}
