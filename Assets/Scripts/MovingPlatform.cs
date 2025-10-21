using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveDistance = 3f;
    public float moveSpeed = 2f;
    public bool moveHorizontally = true;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void FixedUpdate()
    {
        float offset = Mathf.PingPong(Time.time * moveSpeed, moveDistance);

        if (moveHorizontally)
            transform.position = startPos + new Vector3(offset - (moveDistance / 2f), 0, 0);
        else
            transform.position = startPos + new Vector3(0, offset - (moveDistance / 2f), 0);
    }
}



