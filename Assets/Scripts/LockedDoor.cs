using UnityEngine;
using UnityEngine.InputSystem;

public class LockedDoor : MonoBehaviour
{
    public Transform doorPivot;
    public int requiredKeys = 2;
    public float openAngle = -90f;
    public float openSpeed = 2f;

    private bool playerInRange = false;
    private bool isOpening = false;
    private bool hasOpened = false;

    private Quaternion closedRotation;
    private Quaternion targetRotation;

    private PlayerKeys currentPlayerKeys;

    private void Start()
    {
        if (doorPivot != null)
        {
            closedRotation = doorPivot.localRotation;
            targetRotation = closedRotation * Quaternion.Euler(0f, openAngle, 0f);
        }
    }

    private void Update()
    {
        if (playerInRange && !hasOpened && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (currentPlayerKeys != null)
            {
                if (currentPlayerKeys.keyCount >= requiredKeys)
                {
                    currentPlayerKeys.keyCount -= requiredKeys;
                    hasOpened = true;
                    isOpening = true;
                }
                else
                {
                    if (GameMessageUI.Instance != null)
                    {
                        int missingKeys = requiredKeys - currentPlayerKeys.keyCount;

                        if (missingKeys == 1)
                        {
                            GameMessageUI.Instance.ShowMessage("Door is closed, 1 more key required");
                        }
                        else
                        {
                            GameMessageUI.Instance.ShowMessage("Door is closed, " + missingKeys + " keys required");
                        }
                    }
                }
            }
            else
            {
                Debug.LogWarning("LockedDoor: No PlayerKeys found in trigger.");
            }
        }

        if (isOpening && doorPivot != null)
        {
            doorPivot.localRotation = Quaternion.Slerp(
                doorPivot.localRotation,
                targetRotation,
                Time.deltaTime * openSpeed
            );
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>() != null || other.CompareTag("Player"))
        {
            playerInRange = true;
            currentPlayerKeys = other.GetComponent<PlayerKeys>();

            if (currentPlayerKeys == null)
            {
                currentPlayerKeys = other.GetComponentInParent<PlayerKeys>();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CharacterController>() != null || other.CompareTag("Player"))
        {
            playerInRange = false;
            currentPlayerKeys = null;
        }
    }
}