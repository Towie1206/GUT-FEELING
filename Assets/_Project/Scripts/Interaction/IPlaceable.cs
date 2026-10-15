using UnityEngine;

public interface IPlaceable 
{
    string PlaceableID { get; }// Tên định danh của món đồ (vd: "NoodleBowl", "Kettle", "Fuse") để Socket kiểm tra đúng đồ mới cho đặt
    void OnPlace(Transform socketPoint);
}
