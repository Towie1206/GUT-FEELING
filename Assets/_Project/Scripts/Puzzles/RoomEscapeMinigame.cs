#pragma warning disable CS0414, CS0649
using UnityEngine;
using Unity.Cinemachine;

/// <summary>
/// Minigame 2D Pixel Meta-Horror "RoomEscape.exe" trên chiếc máy tính CRT cũ (Tuần 17 - 18).
/// Do tên Kẻ Ký Sinh (cựu lập trình viên) cài sẵn để thao túng tâm lý (Gaslighting) và dụ Nam tìm ra két sắt sau gương toilet.
/// Sử dụng Cinemachine Virtual Camera để blend mượt mà từ góc nhìn 3D FPS vào màn hình 2D CRT.
/// </summary>
public class RoomEscapeMinigame : BasePuzzle, IInteractable
{
    [Header("Cinemachine Camera Transition")]
    [SerializeField] private CinemachineCamera pcMonitorVCam;
    [SerializeField] private int activeCameraPriority = 20;
    [SerializeField] private int inactiveCameraPriority = 0;

    [Header("CRT RenderTexture & 2D Camera (Optional GPU Optimization)")]
    [SerializeField] private Camera minigame2DCamera;
    [SerializeField] private MeshRenderer crtScreenRenderer;
    [SerializeField] private Material crtOffMaterial;
    [SerializeField] private Material crtMinigameMaterial;

    [Header("2D Pixel Minigame References")]
    [SerializeField] private GameObject minigame2DRoot;
    [SerializeField] private RectTransform pixelPlayerTransform;
    [SerializeField] private RectTransform pixelMirrorTransform;
    [SerializeField] private RectTransform pixelSafeIcon;
    [SerializeField] private float pixelMoveSpeed = 120f;
    [SerializeField] private float mirrorInteractDistance = 40f;

    [Header("Audio")]
    [SerializeField] private AudioSource pcBootBeepAudio;
    [SerializeField] private AudioSource retro8BitBgmAudio;
    [SerializeField] private AudioSource pixelMirrorSlideAudio;

    private Player currentPlayer;
    private bool isPlayingMinigame;
    private bool isMirrorMovedIn2D;
    private int launchFrame = -1;

    public bool IsPlayingMinigame => isPlayingMinigame;

    public string InteractionPrompt
    {
        get
        {
            // TODO:
            // - Nếu đang ngồi chơi (isPlayingMinigame) hoặc đã giải xong (IsSolved) -> trả về string.Empty
            // - Ngược lại -> trả về "[E] Sử dụng Máy tính PC (RoomEscape.exe)"
            return (isPlayingMinigame || IsSolved) ? string.Empty : "[E] Sử dụng Máy tính PC";
        }
    }

    private void Start()
    {
        // TODO:
        // Mặc định tắt minigame2DCamera (nếu có) và minigame2DRoot khi chưa bật máy tính để tiết kiệm GPU
    }

    private void Update()
    {
        // TODO:
        // 1. Nếu !isPlayingMinigame hoặc Time.frameCount <= launchFrame (tránh nhận nhầm phím [E] vừa bấm để mở máy!) thì return
        // 2. Đọc input di chuyển (currentPlayer.moveInput) để di chuyển nhân vật pixel (pixelPlayerTransform.anchoredPosition)
        // 3. Nếu người chơi bấm phím Tương tác [E] khi nhân vật pixel đứng gần pixelMirrorTransform (Vector2.Distance <= mirrorInteractDistance):
        //    -> Gọi OnPixelCharacterMovedMirrorIn2D()
        // 4. Nếu người chơi bấm phím Thoát (Tab / Chuột phải):
        //    -> Gọi ExitMinigame()
    }

    public void Interact(Player player)
    {
        // TODO:
        // 1. Kiểm tra chỉ cho phép chơi sau khi đã có điện lại (StoryEventBus.Instance.HasFlag(StoryFlag.PowerRestored))
        // 2. Lưu tham chiếu currentPlayer = player
        // 3. Gọi LaunchMinigame()
    }

    public void LaunchMinigame()
    {
        // TODO:
        // 1. Đặt isPlayingMinigame = true và lưu launchFrame = Time.frameCount
        // 2. Chuyển trạng thái của Player sang khóa di chuyển 3D (vd: currentPlayer.stateMachine.ChangeState(new PlayerCutsceneState(currentPlayer, currentPlayer.stateMachine)))
        // 3. Nâng Priority của pcMonitorVCam lên activeCameraPriority (20) để CinemachineBrain tự blend góc nhìn vào màn hình CRT
        // 4. Bật minigame2DCamera (nếu dùng RenderTexture), gán crtMinigameMaterial cho crtScreenRenderer
        // 5. Bật minigame2DRoot.SetActive(true) và phát nhạc 8-bit rùng rợn (retro8BitBgmAudio)
    }

    public void ExitMinigame()
    {
        // TODO:
        // 1. Đặt isPlayingMinigame = false
        // 2. Hạ Priority của pcMonitorVCam về inactiveCameraPriority (0) để Cinemachine trả góc nhìn về FPS
        // 3. Tắt minigame2DCamera (nếu muốn tiết kiệm GPU khi rời máy tính)
        // 4. Chuyển trạng thái của currentPlayer về currentPlayer.freeState
    }

    public void OnPixelCharacterMovedMirrorIn2D()
    {
        // TODO:
        // 1. Nếu isMirrorMovedIn2D == true thì return
        // 2. Đặt isMirrorMovedIn2D = true
        // 3. Dịch chuyển pixelMirrorTransform sang một bên để lộ biểu tượng Két sắt (pixelSafeIcon.gameObject.SetActive(true))
        // 4. Phát âm thanh pixelMirrorSlideAudio
        // 5. Gọi EvaluatePuzzle()
    }

    public override void EvaluatePuzzle()
    {
        // TODO:
        // Nếu isMirrorMovedIn2D == true:
        // 1. Bật cờ StoryEventBus.Instance.SetFlag(StoryFlag.BathroomSafeRevealed, true)
        // 2. Gọi CompletePuzzle() (bật cờ StoryFlag.RoomEscapeMinigameCompleted và bắn độc thoại gợi ý ra Toilet kiểm tra gương thật!)
        // 3. Tự động gọi ExitMinigame() sau một khoảng trễ ngắn
    }

    public override void ResetPuzzle()
    {
        // TODO:
        // Đưa vị trí pixelPlayerTransform và pixelMirrorTransform về ban đầu
    }
}
