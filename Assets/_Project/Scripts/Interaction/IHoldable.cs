using UnityEngine;

public interface IHoldable
{
    void OnEquip(Transform handSlot);

    void OnUse();

    void OnDrop(Vector3 throwDirection);

    void OnSway(float lookDelta);
}