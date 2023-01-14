using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Sprite toggleFalse;
    [SerializeField] private Sprite toggleTrue;

    bool isFullScreen = false;

    public void Exit()
    {
        Application.Quit();
    }

    public void FullScreenToggle(Button button)
    {
        isFullScreen = !isFullScreen;
        Screen.fullScreen = isFullScreen;
        if (isFullScreen)
        {
            button.image.sprite = toggleTrue;
        }
        else
        {
            button.image.sprite = toggleFalse;
        }
    }

    public void SoundToggle()
    {

    }

    public void MusicToggle()
    {

    }
}
