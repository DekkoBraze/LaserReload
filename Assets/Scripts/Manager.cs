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

    public static EmptyState emptyState;
    public static PortalState portalState;
    public static TurretState turretState;
    public static MovableTurretState movableTurretState;
    public static RailState railState;

    public static Angle0 angle0;
    public static Angle90 angle90;
    public static Angle180 angle180;
    public static Angle270 angle270;

    public static Manager link;
    public static Player playerLink;

    // здесь хранятся спрайты для всех тайлов
    public Sprite[] tileSprites;
    public Sprite[] dangerTileSprites;

    public IState[] states;

    public Tile clickedTile { get; set; }
    public static int stepCount { get; set; } = 0;
    public bool isItOver { get; set; }

    private void Awake()
    {
        link = this;

        states = new IState[5];
        emptyState = new EmptyState();
        states[0] = emptyState;
        portalState = new PortalState();
        states[1] = portalState;
        turretState = new TurretState();
        states[2] = turretState;
        movableTurretState = new MovableTurretState();
        states[3] = movableTurretState;
        railState = new RailState();
        states[4] = railState;

        angle0 = new Angle0();
        angle90 = new Angle90();
        angle180 = new Angle180();
        angle270 = new Angle270();

        playerLink = FindObjectOfType<Player>();

        Messenger.Broadcast(GameEvent.SET_STATE);
    }

    private void Start()
    {
        isItOver = false;
        completeText.enabled = false;
        gameOverText.enabled = false;
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
        energyText.text = Manager.playerLink.energy.ToString();
    }

    public void CompleteTextAppear()
    {
        completeText.enabled = true;
    }

    public void OnPlayerDestroy()
    {
        StartCoroutine(GameOver());
    }
}
