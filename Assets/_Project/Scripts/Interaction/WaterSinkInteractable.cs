using System.Collections;
using UnityEngine;

public class WaterSinkInteractable : MonoBehaviour, IInteractable
{
    [Header("Sink Components")]
    [SerializeField] private AudioSource waterTapAudio;
    [SerializeField] private ParticleSystem waterStreamVFX;
    [SerializeField] private float fillDuration = 2.0f;

    [Header("Story Feedback")]
    [SerializeField] private DialogueData needKettleDialogue;
    [SerializeField] private DialogueData alreadyFilledDialogue;

    private bool isFilling;

    public string InteractionPrompt => isFilling ? "Filling..." : "[E] Turn on the tap to get water";


    private void OnDisable()
    {
        // Đề phòng trường hợp GameObject bị tắt giữa lúc đang xả nước
        if (isFilling)
        {
            isFilling = false;
            if (waterStreamVFX != null) waterStreamVFX.Stop();
            if (waterTapAudio != null) waterTapAudio.Stop();
        }
    }

    public void Interact(Player player)
    {
        if (isFilling) return;

        if (player.hand.CurrentItemHold is CookingPotItem pot)
        {
            if (!pot.HasWater)
            {
                StartCoroutine(FillWaterRoutine(pot));
            }
            else
            {
                StoryEventBus.Instance.RequestDialogue(alreadyFilledDialogue);
            }
        }
        else
        {
            StoryEventBus.Instance.RequestDialogue(needKettleDialogue);
        }
    }

    private IEnumerator FillWaterRoutine(CookingPotItem pot)
    {
        isFilling = true;

        if (waterStreamVFX != null) waterStreamVFX.Play();
        if (waterTapAudio != null) waterTapAudio.Play();

        yield return new WaitForSeconds(fillDuration);

        if (waterStreamVFX != null) waterStreamVFX.Stop();
        if (waterTapAudio != null) waterTapAudio.Stop();

        pot.FillWater();

        isFilling = false;
    }
}