using UnityEngine;
using TMPro;
using System.Collections;

public class VictoryTrigger : MonoBehaviour
{
    public Transform jewel;
    public TextMeshProUGUI youSurvivedText;

    public float riseHeight = 3f;
    public float riseDuration = 1.5f;

    private bool hasTriggered = false;

    private void Start()
    {
        if (youSurvivedText != null)
            youSurvivedText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            StartCoroutine(VictorySequence());
        }
    }

    IEnumerator VictorySequence()
    {
        Vector3 startPos = jewel.position;
        Vector3 endPos = startPos + new Vector3(0, riseHeight, 0);

        float elapsed = 0f;

        while (elapsed < riseDuration)
        {
            elapsed += Time.deltaTime;

            jewel.position = Vector3.Lerp(
                startPos,
                endPos,
                elapsed / riseDuration
            );

            yield return null;
        }

        jewel.position = endPos;

        if (youSurvivedText != null)
            youSurvivedText.gameObject.SetActive(true);
    }
}