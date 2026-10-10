using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [Header("Bobbing Settings")]
    [SerializeField] private float bobSpeedMultiplier = 6f; // Tốc độ bob khi di chuyển
    [Space]
    [SerializeField] private float bobAmountY = 0.05f; // Biên độ bob lên xuống
    [SerializeField] private float bobAmountX = 0.03f; // Biên độ bob sang hai bên
    [SerializeField] private float returnSpeed = 5f; // Tốc độ trở về vị trí ban đầu khi dừng di chuyển

    [SerializeField] private PlayerMovement playerMovement;

    private Vector3 defaultPosition;
    private float timer;

    private void Start()
    {
        defaultPosition = transform.localPosition;
    }

    private void Update()
    {
        HandleHeadBob();
    }
    private void HandleHeadBob()
    {
        if(playerMovement == null) return;

        // Kiểm tra xem người chơi có đang di chuyển không
        bool isMoving = playerMovement.moveInput.sqrMagnitude > 0.01f;

        if(isMoving)
        {
            // Thời gian trôi đi theo nhịp bước chân
            timer += Time.deltaTime * (playerMovement.currentSpeed * bobSpeedMultiplier);

            float newY = defaultPosition.y + Mathf.Sin(timer) * bobAmountY; // Trục Y nhún lên xuống theo từng bước chân
            float newX = defaultPosition.x + Mathf.Sin(timer * 0.5f) * bobAmountX;// Trục X lắc trái/phải theo nhịp đổi chân (chậm bằng một nửa trục Y)

            transform.localPosition = new Vector3(newX, newY, defaultPosition.z);
        }

        else
        {
            timer = 0;

            // Nếu đã về cực gần vị trí gốc -> Gán dứt điểm luôn!
            if (Vector3.Distance(transform.localPosition, defaultPosition) < 0.001f)
            {
                transform.localPosition = defaultPosition;
            }
            else
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, defaultPosition, Time.deltaTime * returnSpeed);
            }
        }
    }    

}