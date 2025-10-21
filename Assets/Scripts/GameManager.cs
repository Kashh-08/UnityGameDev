using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Vector3 respawnPoint;

    [Header("References")]
    public Transform respawnTransform; // Drag your RespawnPoint empty object here in Inspector

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        //  Use assigned respawnTransform if available
        if (respawnTransform != null)
        {
            respawnPoint = respawnTransform.position;
        }
        else
        {
            // fallback if not assigned
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                respawnPoint = player.transform.position;
        }
    }

    public void SetCheckpoint(Vector3 newPoint)
    {
        respawnPoint = newPoint;
    }

    public Vector3 GetRespawnPoint()
    {
        return respawnPoint;
    }
}

