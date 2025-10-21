using UnityEngine;
using TMPro;
using System.Collections;

public class GameUI : MonoBehaviour
{
    public static GameUI instance; // Singleton for easy access

    [Header("UI References")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI heightText;
    public TextMeshProUGUI checkpointText; // Assign in Inspector

    [Header("Player Reference")]
    public Transform player;

    private float startTime;
    private float baseHeight;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        startTime = Time.time;

        if (player != null)
            baseHeight = player.position.y;

        if (checkpointText != null)
            checkpointText.text = ""; // Hide at start
    }

    void Update()
    {
        UpdateTimer();
        UpdateHeight();
    }

    void UpdateTimer()
    {
        float elapsed = Time.time - startTime;

        int minutes = Mathf.FloorToInt(elapsed / 60f);
        int seconds = Mathf.FloorToInt(elapsed % 60f);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    
    void UpdateHeight()
    {
        if (player != null)
        {
            float currentHeight = player.position.y - baseHeight;
            if (currentHeight < 0) currentHeight = 0;

            heightText.text = "Height: " + Mathf.FloorToInt(currentHeight) + "m";
        }
    }

    // Show checkpoint text briefly
    public void ShowCheckpointMessage()
    {
        if (checkpointText != null)
            StartCoroutine(CheckpointRoutine());
    }

    IEnumerator CheckpointRoutine()
    {
        checkpointText.text = "Checkpoint Reached!";
        yield return new WaitForSeconds(2f);
        checkpointText.text = "";
    }
}
