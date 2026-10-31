using System.Collections;
using UnityEngine;

public class ElectricStoveController : MonoBehaviour, IInteractable
{
    [Header("Stove References")]
    [SerializeField] private ItemSocket potSocket;
    [SerializeField] private GameObject fireFlameVFX;
    [SerializeField] private AudioSource igniteAudio;
    [SerializeField] private AudioSource boilingAudio;
    [SerializeField] private float boilDuration = 7.5f;

    [Header("Story & Feedback")]
    [SerializeField] private DialogueData noPotDialogue;
    [SerializeField] private DialogueData emptyPotDialogue;
    [SerializeField] private DialogueData waterBoiledDialogue;

    private bool isBoiling;

    public bool IsBoiling => isBoiling;

    public string InteractionPrompt => isBoiling ? "Boiling..." : "[E] Turn On";

    public void Interact(Player player)
    {

        if (isBoiling) return;
        CookingPotItem placedPot = potSocket.GetComponentInChildren<CookingPotItem>();
        if (placedPot == null)
        {
            StoryEventBus.Instance.RequestDialogue(noPotDialogue);
            return;
        }
        if (!placedPot.HasWater)
        {
            StoryEventBus.Instance.RequestDialogue(emptyPotDialogue);
            return;
        }
        if (placedPot.IsBoiled)
        {
            StoryEventBus.Instance.RequestDialogue(waterBoiledDialogue);
            return;
        }
        StartCoroutine(BoilRoutine(placedPot));

    }

    private IEnumerator BoilRoutine(CookingPotItem pot)
    {
        isBoiling = true;

        Collider potCol = pot.GetComponent<Collider>();
        if (potCol != null) potCol.enabled = false;

        fireFlameVFX.SetActive(true);

        if (igniteAudio != null)
            igniteAudio.Play();

        if (boilingAudio != null)
            boilingAudio.Play();

        yield return new WaitForSeconds(boilDuration);

        if (boilingAudio != null)
            boilingAudio.Stop();

        fireFlameVFX.SetActive(false);

        pot.SetBoiled();

        if (potCol != null) potCol.enabled = true;

        isBoiling = false;

        StoryEventBus.Instance.SetFlag(StoryFlag.WaterBoiling, true);
        StoryEventBus.Instance.RequestDialogue(waterBoiledDialogue);
    }
}
