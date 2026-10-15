using System;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private Transform heldPoint;

    public IHoldable CurrentItemHold { get; private set; }
    public bool HasItem => CurrentItemHold != null;

    public void Equip(IHoldable item)
    {
        if (CurrentItemHold != null)
        {
            DropCurrentItem(transform.forward);
        }
        CurrentItemHold = item;
        CurrentItemHold.OnEquip(heldPoint);
    }

    public void UseCurrentItem()
    {
        if (!HasItem) return;

        CurrentItemHold.OnUse();
    }
    public void DropCurrentItem(Vector3 throwDirection)
    {
        if (!HasItem) return;

        CurrentItemHold.OnDrop(throwDirection);
        CurrentItemHold = null;
    }
    public void UpdateSway(float lookDelta)
    {
        if (!HasItem) return;

        CurrentItemHold.OnSway(lookDelta);
    }

    public void AltUseCurrentItem()
    {
        if (!HasItem) return;
        if (CurrentItemHold is IAltUsable altUsable)
        {
            altUsable.OnAltUse();
        }
    }

    public IHoldable ReleaseCurrentItem()
    {
        if (!HasItem) return null;
        IHoldable item = CurrentItemHold;
        CurrentItemHold = null;
        return item;
    }    

}
