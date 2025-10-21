using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class IntroTypewriter : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI introText;      // assign in inspector (IntroText)
    public CanvasGroup introCanvasGroup;   // assign in inspector (same object or parent)

    [Header("Text")]
    [TextArea(3,6)]
    public string message = "Wake up, traveler. Your climb begins now.";
    public float charDelay = 0.03f;        // time between each character
    public float fadeDuration = 0.6f;      // fade in / out duration
    public float holdDuration = 1.2f;      // time to keep full text visible before fade out

    [Header("Player control")]
    public GameObject playerObject;        // assign your Player GameObject (optional)
    public MonoBehaviour[] disableDuringIntro; // list of player scripts to disable (e.g., PlayerController)

    [Header("Events (optional)")]
    public UnityEvent onIntroComplete;     // hook extra behavior from inspector

    private Coroutine typingCoroutine;
    private bool skipping = false;

    void Start()
    {
        // Defensive defaults
        if (introText == null) Debug.LogError("IntroTypewriter: assign Intro Text (TMP) in inspector.");
        if (introCanvasGroup == null)
            introCanvasGroup = introText.GetComponent<CanvasGroup>();

        // Begin the intro sequence
        StartCoroutine(RunIntroSequence());
    }

    void Update()
    {
        // allow skipping with Space or mouse click
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && typingCoroutine != null)
        {
            skipping = true;
        }
    }

    IEnumerator RunIntroSequence()
    {
        // 1) Disable listed player scripts so the player can't move during intro
        SetPlayerScriptsEnabled(false);

        // 2) Start with panel fully transparent
        if (introCanvasGroup != null)
        {
            introCanvasGroup.alpha = 0f;
            introCanvasGroup.blocksRaycasts = true; // prevent UI clicks to pass through while intro runs
        }

        // 3) Fade in the text panel
        yield return StartCoroutine(FadeCanvas(introCanvasGroup, 0f, 1f, fadeDuration));

        // 4) Typewriter effect
        introText.text = "";
        typingCoroutine = StartCoroutine(TypeText(message));
        yield return typingCoroutine;
        typingCoroutine = null;

        // 5) Wait a bit (allow player to read) unless skipping
        float t = 0f;
        while (!skipping && t < holdDuration)
        {
            t += Time.deltaTime;
            yield return null;
        }

        // 6) Fade out
        yield return StartCoroutine(FadeCanvas(introCanvasGroup, 1f, 0f, fadeDuration));
        if (introCanvasGroup != null) introCanvasGroup.blocksRaycasts = false;

        // 7) Re-enable player control
        SetPlayerScriptsEnabled(true);

        // 8) Signal completion
        onIntroComplete?.Invoke();

        // Optional: destroy this UI object if you don't need it anymore
        // Destroy(gameObject);
    }

    IEnumerator TypeText(string msg)
    {
        for (int i = 0; i < msg.Length; i++)
        {
            if (skipping)
            {
                // If skipping, show full text immediately
                introText.text = msg;
                yield break;
            }

            introText.text += msg[i];
            yield return new WaitForSeconds(charDelay);
        }
    }

    IEnumerator FadeCanvas(CanvasGroup cg, float from, float to, float duration)
    {
        if (cg == null)
            yield break;

        float t = 0f;
        cg.alpha = from;
        while (t < duration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }
        cg.alpha = to;
    }

    void SetPlayerScriptsEnabled(bool enabled)
    {
        // disable/enable any MonoBehaviours you assign in inspector
        if (disableDuringIntro != null)
        {
            foreach (var mb in disableDuringIntro)
            {
                if (mb != null) mb.enabled = enabled;
            }
        }

        // optional: enable/disable entire player GameObject
        if (playerObject != null)
            playerObject.SetActive(enabled);
    }
}

