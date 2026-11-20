using UnityEngine;

public class FuseItem : HoldableItemBase
{
    [Header("Fuse Story Settings")]
    [SerializeField] private bool setFlagOnPickup = true;
    [SerializeField] private StoryFlag pickupFlag = StoryFlag.RedFuseFound;

    public override void Interact(Player player)
    {
        base.Interact(player);

        if(setFlagOnPickup && StoryEventBus.Instance != null)
        {
            StoryEventBus.Instance.SetFlag(pickupFlag, true);
        }
    }
}
