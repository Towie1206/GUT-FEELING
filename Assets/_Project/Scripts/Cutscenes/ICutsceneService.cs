using System;
using UnityEngine.Playables;

public interface ICutsceneService
{
    bool IsPlaying { get; }

    event Action OnCutsceneStarted;

    event Action OnCutsceneEnded;

    void PlayTimeline(PlayableDirector director, Action onComplete = null);

    void TriggerCameraShake(float impulseForce = 1.0f);
}
