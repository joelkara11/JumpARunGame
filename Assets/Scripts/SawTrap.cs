using UnityEngine;

public class SawTrap : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 3f;

    private Transform target;

    void Start()
    {
        if (pointA != null && pointB != null)
        {
            transform.position = pointA.position;
            target = pointB;
        }
    }

    void Update()
    {
        if (pointA == null || pointB == null || target == null) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            target = (target == pointA) ? pointB : pointA;
        }
    }
}