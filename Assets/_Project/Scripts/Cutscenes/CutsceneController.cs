using System;
using UnityEngine;
using UnityEngine.Playables;
using Unity.Cinemachine;

public class CutsceneController : MonoBehaviour, ICutsceneService
{
    public static ICutsceneService Instance { get; private set; }

    [Header("Default Components (Optional)")]
    [SerializeField] private PlayableDirector defaultDirector;
    [SerializeField] private CinemachineImpulseSource impulseSource;

    public bool IsPlaying { get; private set; }

    public event Action OnCutsceneStarted;
    public event Action OnCutsceneEnded;

    private Action currentOnComplete;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void PlayTimeline(PlayableDirector director, Action onComplete = null)
    {
        if(director == null || IsPlaying)
            return;
        IsPlaying = true;
        currentOnComplete = onComplete;
        director.stopped += OnDirectorStopped;
        RaiseCutsceneStarted();
        director.Play();
    }

    public void TriggerCameraShake(float impulseForce = 1.0f)
    {
        if(impulseSource != null)
            impulseSource.GenerateImpulse(impulseForce);
    }

    private void OnDirectorStopped(PlayableDirector director)
    {
        director.stopped -= OnDirectorStopped;
        IsPlaying = false;
        RaiseCutsceneEnded();
        currentOnComplete?.Invoke();
        currentOnComplete = null;
    }

    protected virtual void RaiseCutsceneStarted() => OnCutsceneStarted?.Invoke();
    protected virtual void RaiseCutsceneEnded() => OnCutsceneEnded?.Invoke();
}
