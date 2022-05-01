using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongswordPath : MonoBehaviour
{
    public UNSCWaypoints waypoints;
    public float moveSpeed = 5f;
    public float rotSpeed = 2f;
    private Transform currentWaypoint;
    public float minDistance = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        transform.LookAt(currentWaypoint);
    }

    // Update is called once per frame
    void Update()
    {
        float movementStep = moveSpeed * Time.deltaTime;
        float rotationStep = rotSpeed * Time.deltaTime;

        Vector3 directionToTarget = currentWaypoint.position - transform.position;
        Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotationToTarget, rotationStep);

        Debug.DrawRay(transform.position, transform.forward * 20f, Color.green);
        Debug.DrawRay(transform.position, directionToTarget, Color.blue, 0f);

        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, currentWaypoint.position) < minDistance)
        {
            currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
            //transform.LookAt(currentWaypoint);
        }
    }
}
