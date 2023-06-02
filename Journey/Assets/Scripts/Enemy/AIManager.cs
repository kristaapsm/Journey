using UnityEngine;
using UnityEngine.SceneManagement;

public class AIManager : MonoBehaviour
{
    public int aiCount;

    private void Start()
    {
        // Count the number of AI characters in the scene
        aiCount = GameObject.FindGameObjectsWithTag("AI").Length;
    }

    public void DecreaseAICount()
    {
        aiCount--;

        Debug.Log("AI Count: " + aiCount);

        if (aiCount <= 0)
        {
            Debug.Log("All AI enemies have been defeated!");
            SceneManager.LoadScene("YourSceneName");
        }
    }

}
