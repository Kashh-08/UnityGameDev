using UnityEngine;

public class MovingPlatformRider : MonoBehaviour
{
    private Transform activePlatform;
    private Vector3 lastPlatformPosition;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            activePlatform = collision.transform;
            lastPlatformPosition = activePlatform.position;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            activePlatform = null;
        }
    }

    void FixedUpdate()
    {
        if (activePlatform != null)
        {
            Vector3 platformDelta = activePlatform.position - lastPlatformPosition;
            transform.position += platformDelta;
            lastPlatformPosition = activePlatform.position;
        }
    }
}



