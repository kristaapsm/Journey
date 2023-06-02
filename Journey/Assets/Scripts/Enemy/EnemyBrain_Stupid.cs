using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBrain_Stupid : MonoBehaviour
{
    public Transform target;
    private EnemyReferences enemyReferences;
    private float pathUpdateDeadline;
    public float detectionRange = 10f;
    public float shootingRange = 5f;
    private bool isDead = false;
    private EnemyController enemyController;
    private Animator animator;

    // Animator Parameters
    private bool isWalking = false;
    private bool isRunning = false;
    private bool isAiming = false;

    // Patrolling
    public float patrolRadius = 10f;
    private Vector3 patrolDestination;
    private bool isPatrolling = false;

    private void Awake()
    {
        enemyReferences = GetComponent<EnemyReferences>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        SetRandomPatrolDestination();
        enemyController = GetComponentInParent<EnemyController>();
        if (enemyController != null)
        {
            enemyController.SetBrain(this);
        }
    }

    void Update()
    {
        if (!isDead)
        {
            // If there is a target
            if (target != null)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (distanceToTarget <= detectionRange)
                {
                    // Chase the target and look at it
                    ChaseTarget();
                    LookAtTarget();

                    if (distanceToTarget <= shootingRange)
                    {
                        // Set bool states for animations
                        isWalking = false;
                        isRunning = false;
                        isAiming = true;
                    }
                    else
                    {
                        // Set bool states for animations
                        isWalking = false;
                        isRunning = true;
                        isAiming = false;
                    }
                }
                else
                {
                    // Patrol if not already patrolling
                    if (!isPatrolling)
                    {
                        StartPatrolling();
                    }
                    else
                    {
                        // Check if the destination is reached
                        if (enemyReferences.navMeshAgent.remainingDistance <= enemyReferences.navMeshAgent.stoppingDistance)
                        {
                            SetRandomPatrolDestination();
                        }
                    }

                    // Set bool states for animations
                    isWalking = true;
                    isRunning = false;
                    isAiming = false;
                }

                // Update animator parameters
                animator.SetBool("isWalking", isWalking);
                animator.SetBool("isRunning", isRunning);
                animator.SetBool("isAiming", isAiming);
            }

            // Update speed animator parameter
            animator.SetFloat("speed", enemyReferences.navMeshAgent.desiredVelocity.sqrMagnitude);
        }
    }

    public void Die()
    {
        isDead = true;
        animator.enabled = false;
        SetRigidbodyState(false);
        SetColliderState(true);
        SetNavAgentState(false);
    }

    private void StartPatrolling()
    {
        isPatrolling = true;
        enemyReferences.navMeshAgent.SetDestination(patrolDestination);
    }

    private void SetRandomPatrolDestination()
    {
        isPatrolling = false;
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;
        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(randomDirection, out navMeshHit, patrolRadius, -1);
        patrolDestination = navMeshHit.position;
    }

    private void ChaseTarget()
    {
        enemyReferences.navMeshAgent.SetDestination(target.position);
    }

    private void LookAtTarget()
    {
        Vector3 lookDirection = target.position - transform.position;
        lookDirection.y = 0f;
        Quaternion rotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * enemyReferences.rotationSpeed);
    }

    private void SetRigidbodyState(bool state)
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = state;
        }
        GetComponent<Rigidbody>().isKinematic = !state;
    }

    private void SetColliderState(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = state;
        }
        GetComponent<Collider>().enabled = !state;
    }

    private void SetNavAgentState(bool state)
    {
        enemyReferences.navMeshAgent.enabled = state;
    }
}
