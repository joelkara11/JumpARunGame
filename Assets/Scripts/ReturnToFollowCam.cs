using UnityEngine;

public class ReturnToFollowCam : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject skyfallCamera;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mainCamera.SetActive(true);
            skyfallCamera.SetActive(false);
        }
    }
}