using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    AudioSource source;

    public static bool isMusicOn = true;

    private void Awake()
    {
        Messenger.AddListener(GameEvent.OFF_MUSIC, TurnOffMusic);
        Messenger.AddListener(GameEvent.ON_MUSIC, TurnOnMusic);

        GameObject[] objs = GameObject.FindGameObjectsWithTag("SoundManager");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void TurnOffMusic()
    {
        source.volume = 0;
        isMusicOn = false;
    }

    private void TurnOnMusic()
    {
        source.volume = 0.2f;
        isMusicOn = true;
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.OFF_MUSIC, TurnOffMusic);
        Messenger.RemoveListener(GameEvent.ON_MUSIC, TurnOnMusic);
    }
}
