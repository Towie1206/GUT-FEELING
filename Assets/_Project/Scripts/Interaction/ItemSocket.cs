using UnityEngine;
using UnityEngine.Events;

public class ItemSocket : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform attachPoint;
    [SerializeField] private string requiredItemTag = "";
    [SerializeField] private AudioSource placeAudio;

    public UnityEvent OnItemPlaced;
    public UnityEvent OnItemRemoved;

    private IHoldable currentItem;
    public bool HasItem => currentItem != null;

    public string InteractionPrompt => HasItem ? string.Empty : "Đặt vật phẩm";

    public void Interact(Player player)
    {
        IPlaceable placeableItem = player.hand.CurrentItemHold as IPlaceable;
        if (placeableItem == null) return;

        // Kiểm tra có đúng mã đồ Socket cần không:
        if (!string.IsNullOrEmpty(requiredItemTag) && placeableItem.PlaceableID != requiredItemTag)
            return;

        // Bàn tay nhả đồ ra, thì Socket HỨNG LẤY NGAY LẬP TỨC!
        currentItem = player.hand.ReleaseCurrentItem(); // <-- currentItem nhận đồ ở đây!
                                                        
        placeableItem.OnPlace(attachPoint); // Báo cho món đồ tự bay vào attachPoint:
        if (placeAudio != null) placeAudio.Play();
        OnItemPlaced?.Invoke();
    }

}
