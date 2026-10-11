using UnityEngine;

public class InspectableItem : MonoBehaviour, IInteractable, IInspectable
{
    [Header("Inspect Settings")]
    [Tooltip("Góc xoay khi đưa lên trước mắt (ví dụ tờ giấy cần dựng đứng 90 độ để dễ đọc)")]
    [SerializeField] private Vector3 defaultInspectRotation = new Vector3(0f, 0f, 0f);

    [Tooltip("Dòng chữ hiển thị khi ngắm vào vật thể")]
    [SerializeField] private string promptText = "Soi vật thể [E]";

    private Collider itemCollider;
    private void Awake()
    {
        itemCollider = GetComponent<Collider>();
    }

    #region IInteractable Implementation
    public string InteractionPrompt => promptText;
    public void Interact(Player player)
    {
        if (player.inspect != null)
        {
            player.inspect.StartInspect(this);

            player.stateMachine.ChangeState(player.inspectState);
        }
    }

    #endregion

    #region IInspectable Implementation
    public Transform itemTransform => transform;
    public Vector3 DefaultInspectRotation => defaultInspectRotation;

    public void OnStartInspect()
    {

        itemCollider.enabled = false;
    }

    public void OnEndInspect()
    {

        itemCollider.enabled = true;
    }

    #endregion


}
