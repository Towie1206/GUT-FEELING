using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Typewriter Settings")]
    [SerializeField] private float typeSpeed = 0.04f;

    [Header("Audio Settings")]

    [SerializeField] private AudioSource typingAudioSource;

    [SerializeField] private bool randomizePitch = true;
    [SerializeField] private float minPitch = 0.9f;
    [SerializeField] private float maxPitch = 1.1f;

    private Coroutine typingCoroutine;
    public bool IsDisplaying { get; private set; }

    private void Awake()
    {
        if (dialogueBox != null) dialogueBox.SetActive(false);
    }

    public void ShowMessage(string message)
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeTextRoutine(message));
    }

    private IEnumerator TypeTextRoutine(string fullMessage)
    {
        IsDisplaying = true;
        dialogueBox.SetActive(true);

        dialogueText.text = fullMessage;

        dialogueText.maxVisibleCharacters = 0;

        for (int i = 0; i < fullMessage.Length; i++)
        {
            char c = fullMessage[i];
            dialogueText.maxVisibleCharacters++;

            if (c != ' ')
                PlayTypingSound(typingAudioSource);

            yield return new WaitForSeconds(typeSpeed);
        }

        // Chờ 3 giây cho người chơi đọc xong rồi tắt
        yield return new WaitForSeconds(3f);

        dialogueBox.SetActive(false);
        IsDisplaying = false;
    }

    private void PlayTypingSound(AudioSource audio)
    {
        if (audio == null) return;

        if (randomizePitch)
            audio.pitch = Random.Range(minPitch, maxPitch);
        else
            audio.pitch = 1f;

        audio.PlayOneShot(audio.clip);
    }
}