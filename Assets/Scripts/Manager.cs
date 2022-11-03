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
    // здесь хранятся спрайты для всех тайлов
    public Sprite[] tileSprites;

    private void Start()
    {
        completeText.enabled = false;
        gameOverText.enabled = false;
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

}
