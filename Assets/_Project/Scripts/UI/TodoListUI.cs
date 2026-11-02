#pragma warning disable CS0414, CS0649
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Giao diện Danh sách Việc nhà Đời thường (Mundane To-Do List).
/// Tái hiện cơ chế Checklist UI trực quan từ Video 1 của BatPan (EcQPcTblr4g)
/// kết hợp phong cách chữ trắng tối giản đặc trưng của dòng game Fears to Fathom.
/// Lắng nghe StoryEventBus để tự động cập nhật, gạch bỏ việc cũ và hiện việc mới.
/// </summary>
public class TodoListUI : MonoBehaviour
{
    [Serializable]
    public class TodoItem
    {
        public string taskDescription;
        public StoryFlag completionFlag;
        [HideInInspector] public bool isCompleted;
    }

    [Header("UI Components")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI todoHeaderLabel;
    [SerializeField] private TextMeshProUGUI todoContentLabel; // Hiển thị các dòng [ ] hoặc [x]
    [SerializeField] private AudioSource taskCompleteAudio;

    [Header("List Configuration")]
    [SerializeField] private List<TodoItem> act1TodoList = new List<TodoItem>
    {
        new TodoItem { taskDescription = "Kéo khóa mở vali & đặt chìa khóa lên bàn", completionFlag = StoryFlag.KeyPlacedOnTable },
        new TodoItem { taskDescription = "Bật Tivi CRT theo dõi bản tin dự báo bão", completionFlag = StoryFlag.TVNewsWatched },
        new TodoItem { taskDescription = "Hứng nước và đặt ấm lên bếp ga đun sôi", completionFlag = StoryFlag.WaterBoiling },
        new TodoItem { taskDescription = "Chế nước sôi vào bát mì và ăn lót dạ", completionFlag = StoryFlag.NoodleCooked },
        new TodoItem { taskDescription = "Kiểm tra và đóng chốt cửa kính ban công", completionFlag = StoryFlag.SeenStrangerInRain }
    };

    [Header("Fade & Animation Settings")]
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private bool hideWhenAllCompleted = true;

    private Coroutine fadeRoutine;

    private void Start()
    {
        // TODO:
        // 1. Đăng ký lắng nghe sự kiện cờ cốt truyện:
        //    if (StoryEventBus.Instance != null)
        //        StoryEventBus.Instance.OnStoryFlagChanged += HandleStoryFlagChanged;
        // 2. Làm mới danh sách hiển thị ban đầu: RefreshTodoListDisplay();
    }

    private void OnDestroy()
    {
        // TODO:
        // Hủy đăng ký StoryEventBus.Instance.OnStoryFlagChanged -= HandleStoryFlagChanged;
    }

    private void HandleStoryFlagChanged(StoryFlag flag)
    {
        // TODO:
        // 1. Duyệt qua act1TodoList tìm TodoItem có completionFlag == flag
        // 2. Nếu tìm thấy và !item.isCompleted:
        //    - Đánh dấu item.isCompleted = true
        //    - Phát âm thanh hoàn thành task (taskCompleteAudio.Play())
        //    - Cập nhật lại chuỗi hiển thị todoContentLabel (hiển thị gạch ngang <s>...</s> hoặc [x])
        //    - Nếu tất cả đã hoàn thành và hideWhenAllCompleted == true:
        //      Chạy Coroutine mờ dần CanvasGroup (FadeOut)
    }

    public void RefreshTodoListDisplay()
    {
        // TODO:
        // Xây dựng StringBuilder định dạng danh sách:
        // - Các task đã hoàn thành: <color=#888888><s>[✓] {taskDescription}</s></color>
        // - Task hiện tại cần làm: <color=#FFFFFF><b>[ ] {taskDescription}</b></color>
        // - Gán chuỗi kết quả vào todoContentLabel.text
    }

    public void SetVisible(bool visible)
    {
        // TODO:
        // Chạy Coroutine Fade Alpha của canvasGroup từ giá trị hiện tại về 1 (nếu visible) hoặc 0 (nếu ẩn)
    }
}
