using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();
    private Transform targetWaypoint;
    private int targetWaypointIndex = 0;
    private float minDistance = 0.1f;
    private int lastwayPointIndex;

    private float movementSpeed = 4.0f;
    private float rotationSpeed = 2.0f;

    void Start()
    {
        lastwayPointIndex = waypoints.Count - 1;

        targetWaypoint = waypoints[targetWaypointIndex];

    }

    
    void Update()
    {
        float movementStep = movementSpeed * Time.deltaTime;
        float rotationStep = rotationSpeed * Time.deltaTime;

        Vector3 directionToTarget = targetWaypoint.position - transform.position;
        Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget);

        //transform.rotation = rotationToTarget;

        //Smoother rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationToTarget, rotationStep);

        Debug.DrawRay(transform.position, transform.forward * 50f, Color.green);
        Debug.DrawRay(transform.position, directionToTarget, Color.red, 0f);

        //Checks for distance 
        float distance = Vector3.Distance(transform.position, targetWaypoint.position);

        //Debug.Log("Distance: " + distance);
        CheckDistanceToWayPoint(distance);

        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, movementStep);

        

    }



    //Check based on current distance to waypoint, if closer than min distance, do something else
    void CheckDistanceToWayPoint(float currentDistance)
    {
        //Checks to see if distance is less than or equal to minimum distance and updates waypoint index
        if(currentDistance <= minDistance)
        {
            targetWaypointIndex++;
            UpdateTargetWaypoint();

        }

    }

    //Update the current waypoint
    void UpdateTargetWaypoint()
    {
        //Reset waypoint index when reaching last point
        if(targetWaypointIndex > lastwayPointIndex)
        {
            targetWaypointIndex = 0;

        }

        targetWaypoint = waypoints[targetWaypointIndex];
    }
}
