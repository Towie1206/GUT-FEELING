using System.Collections;
using UnityEngine;

public class FuseBoxPuzzle : BasePuzzle, IInteractable
{
    [Header("Fuse Sockets")]
    [SerializeField] private ItemSocket redFuseSocket;
    [SerializeField] private ItemSocket yellowFuseSocket;

    [Header("Box Door & Lever Transforms")]
    [SerializeField] private Transform[] Fuses;
    [SerializeField] private Transform[] breakerSwitchLevers;
    [SerializeField] private float leverUpAngle = 0f;
    [SerializeField] private float leverDownAngle = 75f;
    [SerializeField] private float leverUpDuration = 1.0f;
    private Collider col;


    [Header("Audio & VFX")]
    [SerializeField] private AudioSource switchFlipAudio;
    [SerializeField] private ParticleSystem sparkVFX;
    [SerializeField] private DialogueData missingFusesDialogue;

    [Header("Fuse Burned")]
    [SerializeField] private MeshRenderer[] burnedFuseRenderer;
    [SerializeField] private Transform[] burnedFuseTransform;
    [SerializeField] private Material burnedFuseMaterial;

    [Header("Minigame 2D")]
    [SerializeField] private FuseBoxMinigameUI minigameUI;
    private Player currentPlayer;

    private bool Phase1;
    private bool isAnimating;
    private bool isTakeOffFuses = false;

    public bool IsBoxOpened => Phase1;

    public string InteractionPrompt
    {
        get
        {
            if (IsSolved)
            {
                return string.Empty;
            }
            else if (!Phase1)
            {
                return "[E] Flip Breaker";
            }
            else if (!isTakeOffFuses)
            {
                return "[E] Remove Burned Fuses";
            }
            else
            {
                return "[E] Reset Breaker Switch";
            }
        }
    }

    private void Awake()
    {
        col = GetComponent<Collider>();
    }
    private void OnEnable()
    {
        minigameUI.OnAllLeversLocked += HandleMinigameCompleted;
        redFuseSocket.OnItemPlaced += OnFusePlaced;
        yellowFuseSocket.OnItemPlaced += OnFusePlaced;
        StoryEventBus.Instance.OnStoryFlagChanged += HandleStoryFlagChanged;
    }

    private void HandleMinigameCompleted()
    {
        minigameUI.Hide();
        currentPlayer.stateMachine.ChangeState(currentPlayer.freeState);
        StartCoroutine(FlipLever());
    }

    private void HandleStoryFlagChanged(StoryFlag flag)
    {
        if (flag == StoryFlag.PowerOutageOccurred)
        {
            foreach (var fuse in Fuses)
            {
                fuse.localRotation = Quaternion.Euler(leverDownAngle, 0f, 90f);
            }
        }
    }

    private void OnDisable()
    {
        minigameUI.OnAllLeversLocked -= HandleMinigameCompleted;
        redFuseSocket.OnItemPlaced -= OnFusePlaced;
        yellowFuseSocket.OnItemPlaced -= OnFusePlaced;
        StoryEventBus.Instance.OnStoryFlagChanged -= HandleStoryFlagChanged;
    }
    public void Interact(Player player)
    {
        if (IsSolved || isAnimating) return;
        currentPlayer = player;

        if (!Phase1)
        {
            Phase1 = true;
            StartCoroutine(BreakerSwitch());
        }

        else if (!isTakeOffFuses)
        {
            TakeOffFuses();
        }

        else
        {
            EvaluatePuzzle();
        }
    }

    public override void EvaluatePuzzle()
    {
        if (redFuseSocket.HasItem && yellowFuseSocket.HasItem)
        {
            currentPlayer.stateMachine.ChangeState(currentPlayer.cutsceneState);
            minigameUI.Show();

        }

        else
        {
            // Chưa đủ 2 cầu chì
            if (sparkVFX != null)
                sparkVFX.Play();
            if (missingFusesDialogue != null)
                StoryEventBus.Instance.RequestDialogue(missingFusesDialogue);
        }


    }

    public override void ResetPuzzle()
    {
        // TODO:
        // Đặt lại IsSolved = false và góc xoay cần gạt về trạng thái ban đầu nếu cần
    }

    private IEnumerator FlipLever()
    {
        isAnimating = true;
        if (switchFlipAudio != null) switchFlipAudio.Play();

        Quaternion[] startRotations = new Quaternion[breakerSwitchLevers.Length];
        for (int i = 0; i < breakerSwitchLevers.Length; i++)
        {
            if (breakerSwitchLevers[i] != null)
                startRotations[i] = breakerSwitchLevers[i].localRotation;
        }

        float elapsedTime = 0f;
        while (elapsedTime < leverUpDuration)
        {
            float t = Mathf.Clamp01(elapsedTime / leverUpDuration);
            for (int i = 0; i < breakerSwitchLevers.Length; i++)
            {
                if (breakerSwitchLevers[i] != null)
                    breakerSwitchLevers[i].localRotation = Quaternion.Slerp(startRotations[i], Quaternion.Euler(0f, leverUpAngle, 0f), t);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isAnimating = false;
        CompletePuzzle();
    }
    private IEnumerator BreakerSwitch()
    {
        StoryEventBus.Instance.SetFlag(StoryFlag.FuseBoxOpened, true);
        isAnimating = true;
        float elapsedTime = 0f;
        Quaternion[] startRotations = new Quaternion[Fuses.Length];
        for (int i = 0; i < Fuses.Length; i++)
        {
            if (Fuses[i] != null)
                startRotations[i] = Fuses[i].localRotation;
        }
        while (elapsedTime < leverUpDuration)
        {
            float t = Mathf.Clamp01(elapsedTime / leverUpDuration);
            if (t < .75f)
                for (int i = 0; i < Fuses.Length; i++)
                {
                    if (Fuses[i] != null)
                        Fuses[i].localRotation = Quaternion.Slerp(startRotations[i], Quaternion.Euler(leverUpAngle, 0f, 90f), t);
                }
            else if (t >= .75f)
                for (int i = 0; i < Fuses.Length; i++)
                {
                    if (Fuses[i] != null)
                        Fuses[i].localRotation = Quaternion.Slerp(startRotations[i], Quaternion.Euler(leverDownAngle, 0f, 90f), t);
                }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (missingFusesDialogue != null)
        {
            StoryEventBus.Instance.RequestDialogue(missingFusesDialogue);
        }
        isAnimating = false;


        if (sparkVFX != null) sparkVFX.Play();
        foreach (var renderer in burnedFuseRenderer)
        {
            if (renderer != null)
                renderer.material = burnedFuseMaterial;
        }
    }

    private void TakeOffFuses()
    {
        foreach (var fuseTransform in burnedFuseTransform)
        {
            if (fuseTransform != null)
            {
                fuseTransform.SetParent(null);
                Rigidbody rb = fuseTransform.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.useGravity = true;
                Vector3 popDirection = (transform.forward + Vector3.down * 0.5f).normalized;
                rb.AddForce(popDirection * 2.5f, ForceMode.Impulse);
            }
        }
        isTakeOffFuses = true;
        col.enabled = false;
    }

    private void OnFusePlaced()
    {
        if (redFuseSocket.HasItem && yellowFuseSocket.HasItem)
        {
            col.enabled = true;
        }
    }
}
