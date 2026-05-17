using UnityEngine;

public class SawDamage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit by saw");

            HealthManager.Instance.TakeDamage(20);
        }
    }
}