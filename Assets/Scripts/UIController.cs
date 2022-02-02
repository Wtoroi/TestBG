using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject PauseMenu;

    public void Pause()
    {
        Time.timeScale = 0;
        PauseMenu.active = true;
    }

    public void Continue()
    {
        PauseMenu.active = false;
        Time.timeScale = 1;
    }

    public void Exit()
    {
        Application.Quit();
    }
}
