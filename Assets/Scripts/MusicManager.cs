using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    AudioSource source;
    [SerializeField] private AudioClip step;
    [SerializeField] private AudioClip playerShoot;
    [SerializeField] private AudioClip enemyShoot;
    [SerializeField] private AudioClip button;
    [SerializeField] private AudioClip teleport;

    private float soundVol = 1;

    private void Awake()
    {
        Messenger.AddListener(GameEvent.STEP_SOUND, StepSound);
        Messenger.AddListener(GameEvent.PLAYER_SHOOT_SOUND, PlayerShootSound);
        Messenger.AddListener(GameEvent.ENEMY_SHOOT_SOUND, EnemyShootSound);
        Messenger.AddListener(GameEvent.BUTTON_SOUND, ButtonSound);
        Messenger.AddListener(GameEvent.TELEPORT_SOUND, TeleportSound);
        Messenger.AddListener(GameEvent.OFF_SOUND, TurnOffSound);
        Messenger.AddListener(GameEvent.ON_SOUND, TurnOnSound);
        Messenger.AddListener(GameEvent.OFF_MUSIC, TurnOffMusic);
        Messenger.AddListener(GameEvent.ON_MUSIC, TurnOnMusic);

        GameObject[] objs = GameObject.FindGameObjectsWithTag("MusicManager");

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

    private void StepSound()
    {
        source.PlayOneShot(step, soundVol);
    }

    private void PlayerShootSound()
    {
        source.PlayOneShot(playerShoot, soundVol);
    }

    private void EnemyShootSound()
    {
        source.PlayOneShot(enemyShoot, soundVol);
    }

    private void ButtonSound()
    {
        source.PlayOneShot(button, soundVol);
    }

    private void TeleportSound()
    {
        source.PlayOneShot(teleport, soundVol);
    }

    private void TurnOffSound()
    {
        soundVol = 0;
    }

    private void TurnOnSound()
    {
        soundVol = 1;
    }

    private void TurnOffMusic()
    {
        source.volume = 0;
    }

    private void TurnOnMusic()
    {
        source.volume = 0.2f;
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.STEP_SOUND, StepSound);
        Messenger.RemoveListener(GameEvent.PLAYER_SHOOT_SOUND, PlayerShootSound);
        Messenger.RemoveListener(GameEvent.ENEMY_SHOOT_SOUND, EnemyShootSound);
        Messenger.RemoveListener(GameEvent.BUTTON_SOUND, ButtonSound);
        Messenger.RemoveListener(GameEvent.TELEPORT_SOUND, TeleportSound);
        Messenger.RemoveListener(GameEvent.OFF_SOUND, TurnOffSound);
        Messenger.RemoveListener(GameEvent.ON_SOUND, TurnOnSound);
        Messenger.RemoveListener(GameEvent.OFF_MUSIC, TurnOffMusic);
        Messenger.RemoveListener(GameEvent.ON_MUSIC, TurnOnMusic);
    }
}
