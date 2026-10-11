using UnityEngine;

public interface IInspectable 
{
    // Transform của đồ vật để di chuyển và xoay
    Transform itemTransform { get; }

    // Góc quay mặc định khi đưa lên trước mắt (ví dụ tờ giấy cần xoay đứng lên để đọc)
    Vector3 DefaultInspectRotation { get; }

    // Được gọi khi bắt đầu soi
    void OnStartInspect();

    // Được gọi khi cất đồ vật về chỗ cũ
    void OnEndInspect();
}
