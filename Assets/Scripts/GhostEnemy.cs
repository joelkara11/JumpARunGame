using UnityEngine;

public class GhostEnemy : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float speed = 2f;
    [SerializeField] private AudioSource squashAudio;

    private int currentPointIndex = 0;
    private bool movingForward = true;
    private bool isDead = false;

    void Update()
    {
        if (isDead || patrolPoints.Length == 0)
            return;

        Transform target = patrolPoints[currentPointIndex];

        // Move ghost
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        // Rotate ghost toward movement direction
        Vector3 direction = target.position - transform.position;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Euler(
                -90f,
                lookRotation.eulerAngles.y,
                0f
            );
        }

        // Patrol logic
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            if (movingForward)
            {
                currentPointIndex++;

                if (currentPointIndex >= patrolPoints.Length - 1)
                    movingForward = false;
            }
            else
            {
                currentPointIndex--;

                if (currentPointIndex <= 0)
                    movingForward = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead)
            return;

        if (other.CompareTag("Player"))
        {
            Collider ghostCollider = GetComponent<Collider>();

            float playerFeetY = other.bounds.min.y;
            float ghostCenterY = ghostCollider.bounds.center.y;

            // More forgiving stomp detection
            if (playerFeetY > ghostCenterY - 0.2f)
            {
                Squash();
            }
        }
    }

    private void Squash()
    {
        isDead = true;

        if (squashAudio != null)
            squashAudio.Play();

        // Squash effect
        transform.localScale = new Vector3(
            transform.localScale.x,
            transform.localScale.y,
            transform.localScale.z * 0.1f
        );

        // Push slightly downward
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y - 0.45f,
            transform.position.z
        );

        Destroy(gameObject, 1f);
    }
}