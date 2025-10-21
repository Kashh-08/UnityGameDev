using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.SetCheckpoint(transform.position);
            Debug.Log("Checkpoint reached at: " + transform.position);

            // Tell UI to show checkpoint text
            GameUI.instance.ShowCheckpointMessage();
        }
    }
}

