using System.Collections;
using UnityEngine;

public class SkyfallTile : MonoBehaviour
{
    [Header("Tile Type")]
    public bool isSafeTile = false;

    [Header("Timing")]
    public float shakeDuration = 0.2f;
    public float shakeAmount = 0.04f;
    public float delayBeforeFall = 0.1f;
    public float destroyAfter = 3f;

    private bool hasTriggered = false;
    private Vector3 originalPosition;
    private Collider mainCollider;
    private Rigidbody rb;

    private void Start()
    {
        originalPosition = transform.position;
        mainCollider = GetComponent<Collider>();

        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.isKinematic = true;
        rb.useGravity = false;
    }

    public void TriggerTile()
    {
        if (hasTriggered) return;
        if (isSafeTile) return;

        hasTriggered = true;
        StartCoroutine(ShakeAndFall());
    }

    private IEnumerator ShakeAndFall()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-shakeAmount, shakeAmount),
                0f,
                Random.Range(-shakeAmount, shakeAmount)
            );

            transform.position = originalPosition + randomOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;

        yield return new WaitForSeconds(delayBeforeFall);

        if (mainCollider != null)
        {
            mainCollider.enabled = false;
        }

        rb.isKinematic = false;
        rb.useGravity = true;

        Destroy(gameObject, destroyAfter);
    }
}