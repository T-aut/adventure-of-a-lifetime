using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GamePaused = false;
    public GameObject PauseMenuUI;
    public PlayerMovement PlayerMovement;

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (GamePaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        GamePaused = true;

        PlayerMovement.SetControlEnabled(false);
    }

    public void ResumeGame()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        GamePaused = false;

        PlayerMovement.SetControlEnabled(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
