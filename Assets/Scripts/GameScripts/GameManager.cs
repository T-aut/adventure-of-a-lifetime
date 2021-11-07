using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool isGameEnded = false;
    public GameObject DeathMenu;

    void Awake()
    {
        DeathMenu.SetActive(isGameEnded);
    }

    public void EndGame()
    {
        if (!isGameEnded)
        {
            isGameEnded = true;
            Invoke("ShowDeathMenu", 2f);
            // Additional logic here in the future
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void ShowDeathMenu()
    {
        DeathMenu.SetActive(true);
    }
}
