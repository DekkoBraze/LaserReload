using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GUILevelManager : MonoBehaviour
{
    [SerializeField] private Sprite toggleFalse;
    [SerializeField] private Sprite toggleTrue;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject levelMenu;
    [SerializeField] private GameObject howToPlayMenu;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject locks;
    public GameObject pause;

    bool isMainMenuOn = true;


    private void Awake()
    {
        Messenger.AddListener(GameEvent.CHANGE_PAUSE_BUTTON_VISABILITY, ChangePauseButtonVisability);

        if (mainMenu == null)
        {
            isMainMenuOn = false;
        }
    }

    private void Start()
    {
        if (locks != null)
        {
            foreach (Transform g in locks.transform.GetComponentsInChildren<Transform>())
            {
                if (g.name == "Locks")
                {
                    continue;
                }
                int lockNum = int.Parse(g.name);
                int unlockedLevels = PlayerPrefs.GetInt("LevelCounter") + 1;
                if (lockNum <= unlockedLevels)
                {
                    g.gameObject.SetActive(false);
                }
            }
        }
    }

    private void Update()
    {
        if (isMainMenuOn)
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
    }

    public void StartLevel(GameObject scene)
    {
        Messenger.Broadcast(GameEvent.ON_MUSIC);
        SceneManager.LoadScene(scene.name);
    }

    public void StartLevelFromMenu(GameObject scene)
    {
        string nextName = scene.name.Substring(5);
        int sceneNum = int.Parse(nextName);
        int keyNum;
        keyNum = PlayerPrefs.GetInt("LevelCounter", 0) + 1;
        if (sceneNum <= keyNum)
        {
            SceneManager.LoadScene(scene.name);
        }
    }

    public void Continue()
    {
        Manager.link.isItOver = false;
        Manager.link.isMenuOn = false;
        menu.SetActive(false);
        pause.SetActive(!Manager.link.isMenuOn);
    }
    public void Exit()
    {
        Application.Quit();
    }

    /*
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
    */

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
    public void ReloadSceneButton()
    {
        Manager.link.ReloadScene();
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Messenger.Broadcast(GameEvent.ON_MUSIC);
    }

    public void PauseButton()
    {
        Manager.link.isItOver = true;
        Manager.link.isMenuOn = true;
        menu.SetActive(true);
        pause.SetActive(!Manager.link.isMenuOn);
    }

    public void StartSkipAds()
    {
        Manager.link.adsObject.GetComponent<AdsYandex>().Show2();
    }

    public void ChangePauseButtonVisability()
    {
        pause.SetActive(!Manager.link.isMenuOn);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.CHANGE_PAUSE_BUTTON_VISABILITY, ChangePauseButtonVisability);
    }
}
