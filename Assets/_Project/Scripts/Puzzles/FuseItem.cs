#pragma warning disable CS0414, CS0649
using UnityEngine;

/// <summary>
/// Vật phẩm Cầu chì (Cầu chì Đỏ ở gầm giường, Cầu chì Vàng ở tủ bếp) dùng cho câu đố Hộp cầu dao Hồi 2.
/// Kế thừa từ HoldableItemBase để tái sử dụng toàn bộ cơ chế nhặt và gắn vào ItemSocket.
/// </summary>
public class FuseItem : HoldableItemBase
{
    [Header("Fuse Story Settings")]
    [SerializeField] private bool setFlagOnPickup = true;
    [SerializeField] private StoryFlag pickupFlag = StoryFlag.RedFuseFound;

    public override void Interact(Player player)
    {
        base.Interact(player);

        // TODO:
        // Nếu setFlagOnPickup == true và StoryEventBus.Instance != null:
        // -> Gọi StoryEventBus.Instance.SetFlag(pickupFlag, true) để đánh dấu đã tìm thấy cầu chì
    }
}
