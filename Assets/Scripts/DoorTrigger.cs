using UnityEngine;
using UnityEngine.InputSystem;

public class DoorTrigger : MonoBehaviour
{
    public Transform doorPivot;
    public float openAngle = -90f;
    public float openSpeed = 2f;

    [Header("Start Game")]
    public bool startsGame = false;

    private bool playerInRange = false;
    private bool isOpen = false;
    private bool gameStartedAlready = false;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        closedRotation = doorPivot.rotation;
        openRotation = closedRotation * Quaternion.Euler(0f, openAngle, 0f);
    }

    void Update()
    {
        if (playerInRange && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            isOpen = !isOpen;

            if (isOpen && startsGame && !gameStartedAlready && HealthManager.Instance != null)
            {
                gameStartedAlready = true;
                HealthManager.Instance.StartGame();
            }
        }

        Quaternion targetRotation = isOpen ? openRotation : closedRotation;
        doorPivot.rotation = Quaternion.Slerp(doorPivot.rotation, targetRotation, Time.deltaTime * openSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>() != null || other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CharacterController>() != null || other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}