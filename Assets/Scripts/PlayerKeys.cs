using UnityEngine;
using TMPro;

public class PlayerKeys : MonoBehaviour
{
    public int keyCount = 0;

    [Header("UI")]
    public TMP_Text keyCounterText;

    void Start()
    {
        UpdateKeyUI();

        if (keyCounterText != null)
            keyCounterText.gameObject.SetActive(false);
    }

    public void ShowKeyUI()
    {
        if (keyCounterText != null)
        {
            UpdateKeyUI();
            keyCounterText.gameObject.SetActive(true);
        }
    }

    public void AddKey(int amount = 1)
    {
        keyCount += amount;
        UpdateKeyUI();
    }

    public void RemoveKeys(int amount)
    {
        keyCount -= amount;

        if (keyCount < 0)
            keyCount = 0;

        UpdateKeyUI();
    }

    public void ResetKeys()
    {
        keyCount = 0;
        UpdateKeyUI();
    }

    public void UpdateKeyUI()
    {
        if (keyCounterText != null)
            keyCounterText.text = "Keys: " + keyCount;
    }
}