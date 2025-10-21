using UnityEngine;

public class FallReset : MonoBehaviour
{
    [Header("Respawn Settings")]
    public float fallThreshold = -10f;

    private Rigidbody rb;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = player.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (player != null && player.position.y < fallThreshold)
        {
            Respawn();
        }
    }

    void Respawn()
    {
        Vector3 respawnPoint = GameManager.Instance.GetRespawnPoint();
        player.position = respawnPoint;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}


