using UnityEngine;

public class CookingPotItem : HoldableItemBase
{
    [Header("Cooking Pot Specifics")]
    [SerializeField] private ParticleSystem steamParticles;
    [SerializeField] private AudioSource waterSloshAudio;
    [SerializeField] private AudioSource boilWaterAudio;

    [SerializeField] private GameObject waterInsideMesh;

    [Header("Story Gate (nồi bị khóa tới khi phát hiện ấm hỏng)")]
    [SerializeField] private DialogueData useKettleFirstDialogue;

    public bool HasWater { get; private set; }
    public bool IsBoiled { get; private set; }

    private bool IsUnlocked => StoryEventBus.Instance.HasFlag(StoryFlag.KettleFoundBroken) ? true : false;

    public override void Interact(Player player)
    {
        if (!IsUnlocked)
        {
            StoryEventBus.Instance.RequestDialogue(useKettleFirstDialogue);
            return;
        }

        base.Interact(player);
    }

    public override string InteractionPrompt
    {
        get
        {
            if (!IsUnlocked)
                return "[E] Use";
            if (IsBoiled)
                return "[E] Holding a pot of boiling water";
            if (HasWater)
                return "[E] Holding a pot of water (unboiled)";
            return isHeld ? string.Empty : $"[E] Pick up {itemName}";
        }
    }

    public void FillWater()
    {
        HasWater = true;
        waterInsideMesh.SetActive(true);
        if (waterSloshAudio != null)
            waterSloshAudio.Play();
    }

    public void SetBoiled()
    {

        IsBoiled = true;
        if (steamParticles != null)
            steamParticles.Play();
        if (boilWaterAudio != null)
            boilWaterAudio.Play();
    }
}
