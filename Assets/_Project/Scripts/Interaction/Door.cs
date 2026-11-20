using System.Collections;
using UnityEngine;

public enum RotationAxis
{
    X,
    Y,
    Z
}
public class Door : MonoBehaviour,IInteractable
{
    [SerializeField] private Transform doorTransform;
    [SerializeField] private RotationAxis rotationAxis = RotationAxis.Z;
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openSpeed = 3f;
    [SerializeField] private AudioSource doorAudio;


    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Coroutine currentCoroutine;

    private void Start()
    {
        closedRotation = doorTransform.localRotation;
        switch(rotationAxis)
        {
            case RotationAxis.X:
                openRotation = closedRotation * Quaternion.Euler(openAngle, 0, 0);
                break;
            case RotationAxis.Y:
                openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);
                break;
            case RotationAxis.Z:
                openRotation = closedRotation * Quaternion.Euler(0, 0, openAngle);
                break;
        }
        // openRotation = closedRotation * Quaternion.Euler(0, 0, openAngle);  giống + trong vector3
    }

    public string InteractionPrompt => isOpen? "[E] Close Door" : "[E] Open Door";

    public void Interact(Player player)
    {
        isOpen = !isOpen;
        Quaternion target = isOpen ? openRotation : closedRotation;
        if(currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(RotateDoorRoutine(target));
    }

    private IEnumerator RotateDoorRoutine(Quaternion target)
    {
        if(doorAudio != null)
            doorAudio.Play();
        Quaternion startRotation = doorTransform.localRotation;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * openSpeed;
            doorTransform.localRotation = Quaternion.Slerp(startRotation, target, t);
            yield return null;
        }
        doorTransform.localRotation = target; // Đảm bảo cửa khép kín 100% không bị khe hở
    }    


}
