using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool isGameEnded = false;

    public void EndGame()
    {
        if (!isGameEnded)
        {
            isGameEnded = true;
            Debug.Log("Game Over");
            RestartGame();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
