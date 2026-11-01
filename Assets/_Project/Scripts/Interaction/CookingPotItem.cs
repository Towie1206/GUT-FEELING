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
            if (isHeld) return string.Empty;
            if (!IsUnlocked) return "[E] Use";
            if (IsBoiled) return "[E] Pick up boil pot (Cooked)";
            if (HasWater) return "[E] Pick up boil pot";
            return $"[E] Nhặt {itemName}";
        }
    }

    public void FillWater()
    {
        HasWater = true;
        if (waterInsideMesh != null)
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

    public void DetachForPouring()
    {
        isHeld = false;
        transform.SetParent(null);
        transform.localScale = originalScale;
        rb.isKinematic = true;
        rb.useGravity = false;
        col.enabled = false;
    }

    public void EmptyWater()
    {
        HasWater = false;
        IsBoiled = false;
        waterInsideMesh.SetActive(false);
        if (steamParticles != null)
            steamParticles.Stop();
        if (boilWaterAudio != null)
            boilWaterAudio.Stop();
    }
}
