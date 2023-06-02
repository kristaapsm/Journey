using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemyBrain_Stupid enemyBrain;

    private bool isDead = false; // Add a flag to track the AI's death

    // Start is called before the first frame update
    void Start()
    {
        setRigidbodyState(true);
        setColliderState(false);
        setNavAgentState(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Die()
    {
        if (isDead)
            return; // Skip if the AI is already dead

        isDead = true; // Set the flag to indicate the AI's death

        //Destroy(gameObject, 5f);
        GetComponent<Animator>().enabled = false;
        setRigidbodyState(false);
        setColliderState(true);
        setNavAgentState(true);
        if (enemyBrain != null)
        {
            enemyBrain.Die();
        }
    }

    void setRigidbodyState(bool state)
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = state;
        }
        GetComponent<Rigidbody>().isKinematic = !state;
    }

    void setColliderState(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            collider.enabled = state;
        }

        GetComponent<Collider>().enabled = !state;
    }

    void setNavAgentState(bool state)
    {
        UnityEngine.AI.NavMeshAgent[] agents = GetComponentsInChildren<UnityEngine.AI.NavMeshAgent>();

        foreach (UnityEngine.AI.NavMeshAgent agent in agents)
        {
            agent.enabled = state;
        }

        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = !state;
    }

    public void SetBrain(EnemyBrain_Stupid brain)
    {
        enemyBrain = brain;
    }
}
