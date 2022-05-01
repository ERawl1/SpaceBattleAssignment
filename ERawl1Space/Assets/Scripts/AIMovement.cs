using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum BehaviourState { none, wander }

public class AIMovement : MonoBehaviour
{
    //Allow for an awake state to be set in inspector
    public BehaviourState awakeState;
    //Settings for the wander range
    [Header("Random Wander Settings")]
    public Bounds boundBox;

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
        if (currentState == BehaviourState.wander)
        {
            float targetDistance = Vector3.Distance(targetPos, transform.position);
            if (targetDistance <= agent.stoppingDistance)
            {
                FindWanderTarget();
            }
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
    }
}
