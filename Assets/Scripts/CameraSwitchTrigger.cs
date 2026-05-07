using UnityEngine;

public class CameraSwitchTrigger : MonoBehaviour
{
    public Camera normalCamera;
    public Camera fixedCamera;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (normalCamera != null)
            normalCamera.gameObject.SetActive(false);

        if (fixedCamera != null)
            fixedCamera.gameObject.SetActive(true);
    }
}