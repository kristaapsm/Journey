using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyReferences : MonoBehaviour
{
    public float rotationSpeed = 5f; // Adjust the rotation speed as needed

    public NavMeshAgent navMeshAgent;
    public Animator animator;

    [Header("Stats")]
    public float pathUpdateDelay = 0.2f;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
}
