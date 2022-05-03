using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum BehaviourState { none, wander, patrol, pursue }

public class AIMovement : MonoBehaviour
{
    //Allow for an awake state to be set in inspector
    public BehaviourState awakeState;
    //Settings for the wander range
    [Header("Random Wander Settings")]
    public Bounds boundBox;
    //Settings for patrol state
    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    public bool randomPatrol = false;
    //Settings for pursue state
    [Header("Pursue Settings")]
    public float pursueDistance = 2.5f;
    public Transform prey;

    private NavMeshAgent agent;
    private BehaviourState currentState = BehaviourState.none;
    private Vector3 targetPos;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    
    void Start()
    {
        //Set Awake state as first state on start up
        SetState(awakeState);
    }

    
    void Update()
    {
        if (prey != null)
        {
            float targetDistance = Vector3.Distance(prey.position, transform.position);

            if (targetDistance <= pursueDistance)
            {
                if (currentState != BehaviourState.pursue)
                {
                    if (currentState != BehaviourState.pursue)
                    {
                        SetState(BehaviourState.pursue);
                    }
                    else
                    {
                        targetPos = prey.position;
                        agent.SetDestination(targetPos);
                    }
                }
                else
                {
                    SetState(awakeState);
                }
            }
        }

        //Stopping Distance
        float distance = Vector3.Distance(targetPos, transform.position);

        if (distance <= agent.stoppingDistance)
        {
            agent.isStopped = true;

            if (currentState == BehaviourState.wander)
            {

                FindWanderTarget();
            }
            else if (currentState == BehaviourState.patrol)
            {
                GoToNextWaypoint(randomPatrol);
            }
            else if (currentState == BehaviourState.pursue)
            {
                targetPos = prey.position;
                agent.SetDestination(targetPos);
            }
        }
        else if (agent.isStopped == true)
        {
            agent.isStopped = false;
        }
    }

    void SetState(BehaviourState state)
    {
        if (currentState != state)
        {
            currentState = state;
            if (currentState == BehaviourState.wander)
            {
                //Find new wander target
                FindWanderTarget();
            }
            else if (currentState == BehaviourState.patrol)
            {
                //Go to patrol waypoint
                GoToNextWaypoint(randomPatrol);
            }
        }
    }

    void FindWanderTarget()
    {
        //Set the target to a random point within boundary box
        targetPos = RandomPoint();
        //Set destination to the new target position
        agent.SetDestination(targetPos);
        //makes sure the AI keeps moving (not stopped)
        agent.isStopped = false;

    }

    void GoToNextWaypoint(bool random = false)
    {
        if (random == false)
        {
            //Patrol next waypoint
            targetPos = GetPatrolWaypoint();
        }
        else
        {
            //Patrol random point
            targetPos = GetPatrolWaypoint(true);
        }
        //Set AI destination to new waypoint
        agent.SetDestination(targetPos);
        //Make sure they can move
        agent.isStopped = false;
    }

    Vector3 GetPatrolWaypoint(bool random = false)
    {
        if (random == false)
        {
            if (targetPos == Vector3.zero)
            {
                return patrolPoints[0].position;
            }
            else
            {
                for (int i = 0; i < patrolPoints.Length; i++)
                {
                    if (patrolPoints[i].position == targetPos)
                    {
                        if (i + 1 >= patrolPoints.Length)
                        {
                            return patrolPoints[0].position;
                        }
                        else
                        {
                            return patrolPoints[i + 1].position;
                        }
                    }
                }
                //Get closest patrol point
                return ClosestPatrolPoint();
            }
        }
        else
        {
            return patrolPoints[Random.Range(0, patrolPoints.Length)].position;
        }        
    }

    Vector3 ClosestPatrolPoint()
    {
        Transform closest = null;
        foreach (Transform Tp in patrolPoints)
        {
            if (closest == null)
            {
                closest = Tp;
            }
            else if(Vector3.Distance(transform.position, Tp.position) < Vector3.Distance(transform.position, closest.position))
            {
                closest = Tp;
            }
        }
        return closest.position;
    }

    Vector3 RandomPoint()
    {
        float randomX = Random.Range(-boundBox.extents.x + agent.radius, boundBox.extents.x - agent.radius);
        float randomZ = Random.Range(-boundBox.extents.z + agent.radius, boundBox.extents.z - agent.radius);
        return new Vector3(randomX, transform.position.y, randomZ);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(boundBox.center, boundBox.size);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetPos, 2f);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, pursueDistance);
    }
}
