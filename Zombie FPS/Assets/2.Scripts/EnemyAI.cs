using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Animator anim; 
    [SerializeField] float chaseRange = 5f;
    [SerializeField] float turnspeed = 5f;

    NavMeshAgent navMeshAgent;
    float distanceToTarget = Mathf.Infinity;
    bool isProvoked = false; 

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        distanceToTarget = Vector3.Distance(target.position, transform.position);
        
        if(isProvoked)
        {
            EngageTarget();
        }
        else if (distanceToTarget <= chaseRange)
        {
            isProvoked = true; 

            //navMeshAgent.SetDestination(target.position);
        }
    }

    private void EngageTarget()
    {
        FaceTarget();

        if(distanceToTarget > navMeshAgent.stoppingDistance)
        {
            ChaseTarget();
        }

        if(distanceToTarget <= navMeshAgent.stoppingDistance)
        {
            AttackTarget();
        }
    }

    private void ChaseTarget()
    {
        anim.SetBool("Attack", false);
        anim.SetTrigger("Move");
        navMeshAgent.SetDestination(target.position);
    }

    private void AttackTarget()
    {
        Debug.Log(name + "has seeked and is destroying " + target.name);

        anim.SetBool("Attack", true);
        //GetComponent<Animator>().SetBool("Attack", true);
    }

    private void FaceTarget()
    {
        //transform.rotation = where the target is, we need to rotate at a certain speed

        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnspeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
