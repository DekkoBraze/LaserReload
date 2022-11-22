using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Manager : MonoBehaviour
{
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private TMP_Text completeText;
    [SerializeField] private TMP_Text gameOverText;

    public EmptyState emptyState;
    public DangerState dangerState;

    public static Manager link;
    public static Player playerLink;

    private GameObject _tilesFolder;

    // здесь хранятся спрайты для всех тайлов
    public Sprite[] tileSprites;
    public static int stepCount = 0;

    private void Awake()
    {
        link = this;
        emptyState = new EmptyState();
        dangerState = new DangerState();
    }

    private void Start()
    {
        completeText.enabled = false;
        gameOverText.enabled = false;
        _tilesFolder = GameObject.FindGameObjectWithTag("Folder");
        playerLink = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            stepCount = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void StepChange()
    {
        stepCount++;
        Messenger.Broadcast(GameEvent.NEXT_STEP);
    }

    public IEnumerator GameOver()
    {
        stepCount = 0;
        gameOverText.enabled = true;

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // метод для апдейта показателя энергии
    public void EnergyUpdate()
    {
        energyText.text = Player.energy.ToString();
    }

    public void CompleteTextAppear()
    {
        completeText.enabled = true;
    }

    public void OnPlayerDestroy()
    {
        StartCoroutine(GameOver());
    }

    public void StartCheckMovableTurret(Tile ClickedTile)
    {
        _tilesFolder.BroadcastMessage("CheckMovableTurretMove", ClickedTile);
    }

}
