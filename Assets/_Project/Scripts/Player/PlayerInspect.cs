using System.Collections;
using UnityEngine;

public class PlayerInspect : MonoBehaviour
{
    [SerializeField] private Transform inspectPoint;
    [SerializeField] private float transitionSpeed = 5f;
    [SerializeField] private float rotateSpeed = 5f;

    private IInspectable currentItemInspect;
    private Vector3 originalPos;
    private Quaternion originalRot;
    private Transform originalParent;
    private Coroutine moveCo;

    public void StartInspect(InspectableItem item)
    {
        currentItemInspect = item;
        
        originalPos = currentItemInspect.itemTransform.position;
        originalRot = currentItemInspect.itemTransform.rotation;
        originalParent = currentItemInspect.itemTransform.parent;

        currentItemInspect.OnStartInspect();

        if(moveCo != null)
            StopCoroutine(moveCo);
        moveCo = StartCoroutine(MoveToInspectPoint(inspectPoint.position, inspectPoint.rotation * Quaternion.Euler(item.DefaultInspectRotation)));
    }

    public void RotateCurrentItem(Vector2 mouseDelta)
    {
        //Rotate(Trục xoay, Góc xoay, Hệ quy chiếu)
        currentItemInspect.itemTransform.Rotate(Vector3.up, -mouseDelta.x * rotateSpeed, Space.World); //Xoay Trái / Phải (Xoay quanh trục thẳng đứng)

        currentItemInspect.itemTransform.Rotate(inspectPoint.right, mouseDelta.y * rotateSpeed, Space.World); //Lật Lên / Xuống (Ngửa mặt trên / Úp mặt dưới)
    }

    public void StopInspect()
    {
        currentItemInspect.OnEndInspect();

        if (moveCo != null)
            StopCoroutine(moveCo);
        moveCo = StartCoroutine(ReturnItem());

    }

    private IEnumerator MoveToInspectPoint(Vector3 targetPos, Quaternion targetRot)
    {
        float t = 0f;
        Vector3 startPos = currentItemInspect.itemTransform.position;
        Quaternion startRot = currentItemInspect.itemTransform.rotation;
        while (t < 1f)
        {
            t += Time.deltaTime * transitionSpeed;
            currentItemInspect.itemTransform.position = Vector3.Lerp(startPos, targetPos, t);
            currentItemInspect.itemTransform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }
        currentItemInspect.itemTransform.SetParent(inspectPoint);
    }

    private IEnumerator ReturnItem()
    {
        currentItemInspect.itemTransform.SetParent(originalParent);

        float t = 0f;
        Vector3 startPos = currentItemInspect.itemTransform.position;
        Quaternion startRot = currentItemInspect.itemTransform.rotation;
        while (t < 1f)
        {
            t += Time.deltaTime * transitionSpeed;
            currentItemInspect.itemTransform.position = Vector3.Lerp(startPos, originalPos, t);
            currentItemInspect.itemTransform.rotation = Quaternion.Slerp(startRot, originalRot, t);
            yield return null;
        }

        currentItemInspect.itemTransform.position = originalPos;
        currentItemInspect.itemTransform.rotation = originalRot;
        currentItemInspect.OnEndInspect();
        currentItemInspect = null;
    }
}
