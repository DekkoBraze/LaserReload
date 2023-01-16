using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Sprite toggleFalse;
    [SerializeField] private Sprite toggleTrue;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject levelMenu;
    [SerializeField] private GameObject howToPlayMenu;

    bool isFullScreen = false;
    bool isSoundOn = true;
    bool isMusicOn = true;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (levelMenu.activeSelf)
            {
                Messenger.Broadcast(GameEvent.BUTTON_SOUND);
                mainMenu.SetActive(true);
                levelMenu.SetActive(false);
            }
            if (howToPlayMenu.activeSelf)
            {
                Messenger.Broadcast(GameEvent.BUTTON_SOUND);
                mainMenu.SetActive(true);
                howToPlayMenu.SetActive(false);
            }
        }
    }

    private void Awake()
    {
        isFullScreen = Screen.fullScreen;
    }

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

    public void SoundToggle(Button button)
    {
        isSoundOn = !isSoundOn;
        if (isSoundOn)
        {
            Messenger.Broadcast(GameEvent.ON_SOUND);
            button.image.sprite = toggleTrue;
        }
        else
        {
            Messenger.Broadcast(GameEvent.OFF_SOUND);
            button.image.sprite = toggleFalse;
        }
    }

    public void MusicToggle(Button button)
    {
        isMusicOn = !isMusicOn;
        if (isMusicOn)
        {
            Messenger.Broadcast(GameEvent.ON_MUSIC);
            button.image.sprite = toggleTrue;
        }
        else
        {
            Messenger.Broadcast(GameEvent.OFF_MUSIC);
            button.image.sprite = toggleFalse;
        }
    }

    public void ButtonSoundBroadcast()
    {
        Messenger.Broadcast(GameEvent.BUTTON_SOUND);
    }
}
