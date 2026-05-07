using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class TrapChest : MonoBehaviour
{
    public Transform lid;

    public GameObject bomb;
    public GameObject bomb2;
    public GameObject bomb3;
    public GameObject bomb4;
    public GameObject bomb5;

    public TextMeshProUGUI bombCountdownText;
    public float countdownDuration = 5f;

    public Transform player;
    public Transform trapRespawnPoint;

    public Light bombGlow;

    public Transform trapDoor;
    public float trapDoorCloseSpeed = 2f;
    public float trapDoorCloseAngle = 90f;

    public float openAngle = -110f;
    public float openSpeed = 2f;

    public float bombUpForce = 4f;
    public float bombForwardForce = 3f;
    public float timeBetweenBombs = 0.2f;
    public float bombLifetime = 4f;

    private bool playerInRange = false;
    private bool isOpening = false;
    private bool hasOpened = false;

    private Quaternion closedRotation;
    private Quaternion targetRotation;

    private bool isTrapDoorClosing = false;
    private Quaternion trapDoorClosedRotation;
    private Quaternion trapDoorTargetRotation;

    private Rigidbody bombRb;
    private Rigidbody bomb2Rb;
    private Rigidbody bomb3Rb;
    private Rigidbody bomb4Rb;
    private Rigidbody bomb5Rb;

    private Vector3 bombStartWorldPosition;
    private Quaternion bombStartWorldRotation;

    private Vector3 bomb2StartWorldPosition;
    private Quaternion bomb2StartWorldRotation;

    private Vector3 bomb3StartWorldPosition;
    private Quaternion bomb3StartWorldRotation;

    private Vector3 bomb4StartWorldPosition;
    private Quaternion bomb4StartWorldRotation;

    private Vector3 bomb5StartWorldPosition;
    private Quaternion bomb5StartWorldRotation;

    private Collider[] chestColliders;

    private void Start()
    {
        if (lid != null)
        {
            closedRotation = lid.localRotation;
            targetRotation = closedRotation * Quaternion.Euler(openAngle, 0f, 0f);
        }

        if (trapDoor != null)
        {
            trapDoorClosedRotation = trapDoor.localRotation;
            trapDoorTargetRotation = trapDoorClosedRotation * Quaternion.Euler(0f, trapDoorCloseAngle, 0f);
        }

        chestColliders = GetComponentsInParent<Collider>();

        SetupBomb(ref bomb, ref bombRb, out bombStartWorldPosition, out bombStartWorldRotation);
        SetupBomb(ref bomb2, ref bomb2Rb, out bomb2StartWorldPosition, out bomb2StartWorldRotation);
        SetupBomb(ref bomb3, ref bomb3Rb, out bomb3StartWorldPosition, out bomb3StartWorldRotation);
        SetupBomb(ref bomb4, ref bomb4Rb, out bomb4StartWorldPosition, out bomb4StartWorldRotation);
        SetupBomb(ref bomb5, ref bomb5Rb, out bomb5StartWorldPosition, out bomb5StartWorldRotation);

        if (bombGlow != null)
        {
            bombGlow.enabled = false;
        }
        if (bombCountdownText != null)
        {
            bombCountdownText.gameObject.SetActive(false);
        }
    }

    private void SetupBomb(ref GameObject bombObj, ref Rigidbody rb, out Vector3 startPos, out Quaternion startRot)
    {
        startPos = Vector3.zero;
        startRot = Quaternion.identity;

        if (bombObj != null)
        {
            startPos = bombObj.transform.position;
            startRot = bombObj.transform.rotation;

            rb = bombObj.GetComponent<Rigidbody>();

            bombObj.SetActive(false);

            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = true;
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }

    private void Update()
    {
        if (playerInRange && !hasOpened && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            isOpening = true;
            hasOpened = true;
            isTrapDoorClosing = true;

            if (bombGlow != null)
            {
                bombGlow.enabled = true;
            }

            StartCoroutine(LaunchBombs());
            StartCoroutine(ShowCountdown());
        }

        if (isOpening && lid != null)
        {
            lid.localRotation = Quaternion.Slerp(
                lid.localRotation,
                targetRotation,
                Time.deltaTime * openSpeed
            );
        }
        if (isTrapDoorClosing && trapDoor != null)
        {
            trapDoor.localRotation = Quaternion.Slerp(
                trapDoor.localRotation,
                trapDoorTargetRotation,
                Time.deltaTime * trapDoorCloseSpeed
            );
        }
    }

    private IEnumerator LaunchBombs()
    {
        LaunchSingleBomb(bomb, bombRb, bombStartWorldPosition, bombStartWorldRotation);
        yield return new WaitForSeconds(timeBetweenBombs);

        LaunchSingleBomb(bomb2, bomb2Rb, bomb2StartWorldPosition, bomb2StartWorldRotation);
        yield return new WaitForSeconds(timeBetweenBombs);

        LaunchSingleBomb(bomb3, bomb3Rb, bomb3StartWorldPosition, bomb3StartWorldRotation);
        yield return new WaitForSeconds(timeBetweenBombs);

        LaunchSingleBomb(bomb4, bomb4Rb, bomb4StartWorldPosition, bomb4StartWorldRotation);
        yield return new WaitForSeconds(timeBetweenBombs);

        LaunchSingleBomb(bomb5, bomb5Rb, bomb5StartWorldPosition, bomb5StartWorldRotation);
    }

    private IEnumerator ShowCountdown()
    {
        if (bombCountdownText == null)
            yield break;

        RectTransform rect = bombCountdownText.rectTransform;
        bombCountdownText.gameObject.SetActive(true);

        // Normale Countdown-Zahlen: oben, kleiner
        rect.anchoredPosition = new Vector2(0f, 150f);
        bombCountdownText.fontSize = 80;
        bombCountdownText.alignment = TextAlignmentOptions.Center;

        for (int i = Mathf.CeilToInt(countdownDuration); i > 0; i--)
        {
            bombCountdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        // BOOM: groß, mittig
        rect.anchoredPosition = new Vector2(0f, 0f);
        bombCountdownText.fontSize = 180;
        bombCountdownText.text = "BOOM!";
        yield return new WaitForSeconds(0.5f);

        if (player != null && trapRespawnPoint != null)
        {
            CharacterController cc = player.GetComponent<CharacterController>();

            if (cc != null)
                cc.enabled = false;

            player.position = trapRespawnPoint.position;

            if (cc != null)
                cc.enabled = true;
        }

        bombCountdownText.gameObject.SetActive(false);
    }

    private void LaunchSingleBomb(GameObject bombObj, Rigidbody rb, Vector3 startPos, Quaternion startRot)
    {
        if (bombObj == null)
            return;

        bombObj.SetActive(true);

        bombObj.transform.SetParent(null, true);
        bombObj.transform.position = startPos;
        bombObj.transform.rotation = startRot;

        Collider bombCollider = bombObj.GetComponent<Collider>();

        if (bombCollider != null && chestColliders != null)
        {
            foreach (Collider c in chestColliders)
            {
                if (c != null)
                {
                    Physics.IgnoreCollision(bombCollider, c, true);
                }
            }
        }

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            Vector3 launchVelocity =
                (transform.forward * bombForwardForce) +
                (Vector3.up * bombUpForce);

            rb.linearVelocity = launchVelocity;
        }
        StartCoroutine(DisableBombAfterTime(bombObj,bombLifetime));
    }

    private IEnumerator DisableBombAfterTime(GameObject bombObj, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (bombObj != null)
        {
            bombObj.SetActive(false);
        }
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