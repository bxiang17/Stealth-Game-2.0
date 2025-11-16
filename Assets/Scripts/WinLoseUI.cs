using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public GameObject gameLoseUI;
    public GameObject gameWinUI;
    bool gameIsOver;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Guard.OnGuardHasSpottedPlayer += ShowGameLoseUI;
        Object.FindAnyObjectByType<Player>().OnPlayerReachedFinish += ShowGameWinUI;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameIsOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    public void ShowGameLoseUI()
    {
        OnGameOver(gameLoseUI);
    }

    public void ShowGameWinUI()
    {
        OnGameOver(gameWinUI);
    }

    void OnGameOver(GameObject gameOverUI)
    {
        gameOverUI.SetActive(true);
        gameIsOver = true;
        Guard.OnGuardHasSpottedPlayer -= ShowGameLoseUI; 
        Object.FindAnyObjectByType<Player>().OnPlayerReachedFinish -= ShowGameWinUI;

    }

}