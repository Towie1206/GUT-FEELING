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
        if (!player.hand.HasItem) return;

        if (HasItem) return; //socket đã có đồ rồi thì không cho đặt thêm


    }    

}
