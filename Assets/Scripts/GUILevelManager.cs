using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GUILevelManager : MonoBehaviour
{
    [SerializeField] private Sprite toggleFalse;
    [SerializeField] private Sprite toggleTrue;
    [SerializeField] private GameObject musicToggle;
    [SerializeField] private GameObject soundToggle;
    [SerializeField] private GameObject menu;

    bool isFullScreen = false;

    private void Awake()
    {
        isFullScreen = Screen.fullScreen;
        if (MusicManager.isMusicOn)
        {
            musicToggle.GetComponent<Image>().sprite = toggleTrue;
        }
        else
        {
            musicToggle.GetComponent<Image>().sprite = toggleFalse;
        }
        if (SoundManager.isSoundOn)
        {
            soundToggle.GetComponent<Image>().sprite = toggleTrue;
        }
        else
        {
            soundToggle.GetComponent<Image>().sprite = toggleFalse;
        }
    }

    public void StartLevel(GameObject scene)
    {
        SceneManager.LoadScene(scene.name);
    }

    public void Continue()
    {
        Manager.link.isItOver = false;
        Manager.link.isMenuOn = false;
        menu.SetActive(false);
    }
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

    public void SoundToggle(Button button)
    {
        SoundManager.isSoundOn = !SoundManager.isSoundOn;
        if (SoundManager.isSoundOn)
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
        MusicManager.isMusicOn = !MusicManager.isMusicOn;
        if (MusicManager.isMusicOn)
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
