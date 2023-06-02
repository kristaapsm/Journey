using CodeMonkey.HealthSystemCM;
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

    // Shooting
    public float shootingInterval = 1f;
    private bool canShoot = true;
    private float shootingTimer = 0f;

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

                        if (canShoot && animator.GetCurrentAnimatorStateInfo(0).IsName("PistolAim"))
                        {
                            Shoot();
                            canShoot = false;
                            shootingTimer = 0f;
                        }
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

        // Update shooting timer
        if (!canShoot)
        {
            shootingTimer += Time.deltaTime;
            if (shootingTimer >= shootingInterval)
            {
                canShoot = true;
            }
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
        enemyReferences.navMeshAgent.isStopped = false;
    }

    private void SetRandomPatrolDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, patrolRadius, -1);
        patrolDestination = navHit.position;
    }

    private void SetRigidbodyState(bool state)
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = state;
        }
    }

    private void SetColliderState(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = state;
        }
    }

    private void SetNavAgentState(bool state)
    {
        enemyReferences.navMeshAgent.enabled = state;
    }

    private void ChaseTarget()
    {
        enemyReferences.navMeshAgent.SetDestination(target.position);
        enemyReferences.navMeshAgent.isStopped = false;
    }

    private void LookAtTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * enemyReferences.rotationSpeed);
    }

    private void Shoot()
    {
        // Reduce the target's health by 10 using the HealthSystem
        HealthSystemComponent targetHealthSystem;
        if (target.TryGetComponent(out targetHealthSystem))
        {
            HealthSystem healthSystem = targetHealthSystem.GetHealthSystem();
            healthSystem.Damage(10);
            if (healthSystem.GetHealth() <= 0)
            {
                Destroy(target.gameObject);
            }
        }
    }

}
