using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private static bool shouldMoving = false;

    [SerializeField]
    private float platformSpeed;

    [SerializeField]
    private Vector3 start;

    [SerializeField]
    private Vector3 end;

    private Vector3 lastPosition;

    private void Start()
    {
        lastPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (shouldMoving)
        {
            lastPosition = transform.position;

            float pingPong = Mathf.PingPong(Time.fixedTime * platformSpeed, 1.0f);
            Vector3 newPosition = Vector3.Lerp(start, end, pingPong);

            transform.position = newPosition;
        }
    }

    public Vector3 GetVelocity()
    {
        if (shouldMoving)
        {
            return (transform.position - lastPosition) / Time.fixedDeltaTime;
        }

        return Vector3.zero;
    }

    public void SetShouldMoving(bool shouldPlatformsMoving)
    {
        shouldMoving = shouldPlatformsMoving;
    }
}