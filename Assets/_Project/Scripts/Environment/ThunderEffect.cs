using UnityEngine;
using Unity.Cinemachine;

public class ThunderEffect : MonoBehaviour
{
    private CinemachineImpulseSource impulseSource;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        if(impulseSource == null)
            impulseSource = GetComponent<CinemachineImpulseSource>();

    }

    private void Update()
    {

    }

    public void TriggerThunder()
    {
        if (impulseSource == null) return;
        if(audioSource != null)
        {
            audioSource.Play();
        }
        impulseSource.GenerateImpulse();
    }
}
