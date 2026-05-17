using UnityEngine;
using UnityEngine.InputSystem;

public class JewelTrigger : MonoBehaviour
{
    public GameObject victoryText;

    private bool playerInRange = false;

    private void Start()
    {
        if (victoryText != null)
            victoryText.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            Debug.Log("Victory triggered!");

            if (victoryText != null)
                victoryText.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.GetComponentInParent<Character>() != null)
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.GetComponentInParent<Character>() != null)
            playerInRange = false;
    }
}