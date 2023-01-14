using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Sprite toggleFalse;
    [SerializeField] private Sprite toggleTrue;

    bool isFullScreen = false;

    public void StartLevel(GameObject scene)
    {
        SceneManager.LoadScene(scene.name);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Continue()
    {
        Manager.link.isItOver = false;
        Manager.link.isMenuOn = false;
        this.gameObject.SetActive(false);
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
