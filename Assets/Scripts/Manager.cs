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
    public static List<Turret> turrets;

    private void Awake()
    {
        Messenger.AddListener(GameEvent.DANGEROUS_TILES_UPDATE, EmptyVoid);
        turrets = new List<Turret>();
    }

    private void Start()
    {
        completeText.enabled = false;
        gameOverText.enabled = false;
        foreach (Turret turret in turrets)
        {
          turret.DangerousTilesSpawn();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public IEnumerator GameOver()
    {
        gameOverText.enabled = true;

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

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

    private void EmptyVoid()
    {

    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.DANGEROUS_TILES_UPDATE, EmptyVoid);
    }
}
