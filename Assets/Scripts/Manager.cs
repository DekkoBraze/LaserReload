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
    public PortalState portalState;
    public DangerPortalState dangerPortalState;
    public TurretState turretState;
    public MovableTurretState movableTurretState;
    public RailState railState;

    public Angle0 angle0;
    public Angle90 angle90;
    public Angle180 angle180;
    public Angle270 angle270;

    public static Manager link;
    public static Player playerLink;

    private GameObject _tilesFolder;

    // здесь хранятся спрайты для всех тайлов
    public Sprite[] tileSprites;
    public Sprite[] dangerTileSprites;
    public IState[] states;
    public static int stepCount = 0;
    public bool isItOver;

    private void Awake()
    {
        link = this;

        states = new IState[7];
        emptyState = new EmptyState();
        states[0] = emptyState;
        dangerState = new DangerState();
        states[1] = dangerState;
        portalState = new PortalState();
        states[2] = portalState;
        dangerPortalState = new DangerPortalState();
        states[3] = dangerPortalState;
        turretState = new TurretState();
        states[4] = turretState;
        movableTurretState = new MovableTurretState();
        states[5] = movableTurretState;
        railState = new RailState();
        states[6] = railState;

        angle0 = new Angle0();
        angle90 = new Angle90();
        angle180 = new Angle180();
        angle270 = new Angle270();

        _tilesFolder = GameObject.FindGameObjectWithTag("Folder");
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
