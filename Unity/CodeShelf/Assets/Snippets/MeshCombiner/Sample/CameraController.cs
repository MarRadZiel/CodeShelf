using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform[] waypoints;
    private const float waypointDetectionDistance = 0.25f;
    private const float speed = 2.5f;

    private int currentWaypoint;
    private bool backwards = false;


    private void Update()
    {
        Vector3 toWaypoint = waypoints[currentWaypoint].position - transform.position;
        float distance = toWaypoint.magnitude;
        if (distance < waypointDetectionDistance)
        {
            if (backwards)
            {
                --currentWaypoint;
            }
            else
            {
                ++currentWaypoint;
            }

            if (currentWaypoint >= waypoints.Length || currentWaypoint < 0)
            {
                backwards = !backwards;
                if (backwards)
                {
                    --currentWaypoint;
                }
                else
                {
                    ++currentWaypoint;
                }
            }
        }
        else
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.LookRotation(toWaypoint.normalized, Vector3.up), Time.deltaTime);
            transform.Translate(Vector3.forward * Mathf.Min(distance, Time.deltaTime * speed), Space.Self);
        }
    }
}
