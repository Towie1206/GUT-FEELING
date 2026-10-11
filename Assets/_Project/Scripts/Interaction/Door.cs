using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour,IInteractable
{
    [SerializeField] private Transform doorTransform;
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
        openRotation = closedRotation * Quaternion.Euler(0, 0, openAngle); // giống + trong vector3
    }

    public string InteractionPrompt => isOpen? "Close Door" : "Open Door";

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
