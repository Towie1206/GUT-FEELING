using System;
using System.Xml.Serialization;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [Header("Hand Slots")]
    [SerializeField] private Transform heldPointR;
    [SerializeField] private Transform heldPointL;

    public IHoldable CurrentItemHold { get; private set; }
    public IHoldable LeftHandItemHold { get; private set; }

    public bool HasItem => CurrentItemHold != null;
    public bool HasLeftHandItem => LeftHandItemHold != null;

    public void Equip(IHoldable item)
    {
        if (CurrentItemHold != null)
        {
            DropCurrentItem(transform.forward);
        }
        CurrentItemHold = item;
        CurrentItemHold.OnEquip(heldPointR);
    }

   public void EquipLeftHand(IHoldable item)
    {
        if (LeftHandItemHold != null)
        {
            DropLeftHandItem(transform.forward);
        }
        LeftHandItemHold = item;
        LeftHandItemHold.OnEquip(heldPointL);
    }

    public void UseCurrentItem()
    {
        if (!HasItem) return;

        CurrentItemHold.OnUse();
    }
    public void UseLeftHandItem()
    {
        if (!HasLeftHandItem) return;

        LeftHandItemHold.OnUse();
    }
    public void DropCurrentItem(Vector3 throwDirection)
    {
        if (!HasItem) return;

        CurrentItemHold.OnDrop(throwDirection);
        CurrentItemHold = null;
    }
    public void DropLeftHandItem(Vector3 throwDirection)
    {
        if (!HasLeftHandItem) return;

        LeftHandItemHold.OnDrop(throwDirection);
        LeftHandItemHold = null;
    }

    public void UpdateSway(float lookDelta)
    {
        if (HasItem) CurrentItemHold.OnSway(lookDelta);
        if (HasLeftHandItem) LeftHandItemHold.OnSway(lookDelta);;
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
