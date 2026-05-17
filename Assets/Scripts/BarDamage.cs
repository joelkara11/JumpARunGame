using UnityEngine;

public class BarDamage : MonoBehaviour
{
    public int damage = 20;
    public float damageCooldown = 1.0f;

    private float lastDamageTime = -999f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time - lastDamageTime < damageCooldown)
                return;

            lastDamageTime = Time.time;

            Debug.Log("Player hit by rotating bar");
            HealthManager.Instance.TakeDamage(damage);
        }
    }
}
