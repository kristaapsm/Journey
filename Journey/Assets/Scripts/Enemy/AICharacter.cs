using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacter : MonoBehaviour
{
    private AIManager aiManager;

    private void Start()
    {
        // Get reference to the AIManager script
        aiManager = GameObject.FindObjectOfType<AIManager>();
    }

    public void Die()
    {
        // Handle AI death logic

        // Call the DecreaseAICount() method in AIManager script
        aiManager.DecreaseAICount();
    }
}
