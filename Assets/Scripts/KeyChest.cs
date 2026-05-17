using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyChest : MonoBehaviour
{
    public Transform lid;
    public GameObject keyVisual;
    public int maxKeysForDoor = 2;

    public float openAngle = -110f;
    public float openSpeed = 2f;

    public float keyRiseHeight = 1.0f;
    public float keyRiseSpeed = 2f;
    public float keyLifetime = 3f;

    private bool playerInRange = false;
    private bool hasOpened = false;
    private bool isOpening = false;
    private bool keyIsRising = false;

    private PlayerKeys currentPlayerKeys;

    private Quaternion closedRotation;
    private Quaternion targetRotation;

    private Vector3 keyStartLocalPosition;
    private Vector3 keyTargetLocalPosition;

    private void Start()
    {
        if (lid != null)
        {
            closedRotation = lid.localRotation;
            targetRotation = closedRotation * Quaternion.Euler(openAngle, 0f, 0f);
        }

        if (keyVisual != null)
        {
            keyStartLocalPosition = keyVisual.transform.localPosition;
            keyTargetLocalPosition = keyStartLocalPosition + new Vector3(0f, keyRiseHeight, 0f);
            keyVisual.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInRange && !hasOpened && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            hasOpened = true;
            isOpening = true;

            if (currentPlayerKeys != null)
            {
                currentPlayerKeys.AddKey();

                if (GameMessageUI.Instance != null)
                {
                    GameMessageUI.Instance.ShowMessage(
                        "Key acquired: " + currentPlayerKeys.keyCount + " / " + maxKeysForDoor
                    );
                }
            }
            else
            {
                Debug.LogWarning("KeyChest: No PlayerKeys found in trigger.");
            }

            if (keyVisual != null)
            {
                keyVisual.SetActive(true);
                keyVisual.transform.localPosition = keyStartLocalPosition;
                keyIsRising = true;

                StartCoroutine(HideKeyAfterTime());
            }
        }

        if (isOpening && lid != null)
        {
            lid.localRotation = Quaternion.Slerp(
                lid.localRotation,
                targetRotation,
                Time.deltaTime * openSpeed
            );
        }

        if (keyIsRising && keyVisual != null)
        {
            keyVisual.transform.localPosition = Vector3.MoveTowards(
                keyVisual.transform.localPosition,
                keyTargetLocalPosition,
                keyRiseSpeed * Time.deltaTime
            );

            if (Vector3.Distance(keyVisual.transform.localPosition, keyTargetLocalPosition) < 0.01f)
            {
                keyVisual.transform.localPosition = keyTargetLocalPosition;
                keyIsRising = false;
            }
        }
    }

    private IEnumerator HideKeyAfterTime()
    {
        yield return new WaitForSeconds(keyLifetime);

        if (keyVisual != null)
        {
            keyVisual.SetActive(false);
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