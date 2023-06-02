using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRefrences : MonoBehaviour
{
    public NavMeshAgent navMeshagent;
    public Animator animator;

    [Header("Stats")]
    //lai netiktu computota navmesh route
    public float pathUpdateDelay = 0.2f;
    private void Awake()
    {
        navMeshagent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
}
