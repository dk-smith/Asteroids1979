using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Controllers;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UI;
using Random = UnityEngine.Random;

// Общий контроль игрового процесса
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameSettings settings;
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private HUD Hud;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private PlayerControl player;

    private readonly List<AsteroidController> asteroidsList = new List<AsteroidController>();
    private int level = 0;
    private int highScore = 0;
    private int bigCount = 0;
    private bool gameOver = false;
    private bool newHighScore = false;

    private void Awake()
    {
        LoadScore();
        FillList();
        StartCoroutine(AddSaucerCoroutine());

        AsteroidController.OnAsteroidDestroy += RemoveAsteroid;
        
        if (playerData)
        {
            playerData.Lives = settings.startLives;
            playerData.Score = 0;
            playerData.OnChange += OnPlayerDataChange;
            playerData.OnHit += OnPlayerHit;
        }

        if (settings.WasInit)
        {
            player.gameObject.SetActive(true);
            pauseMenu.gameObject.SetActive(false);
            return;
        }
        
        player.gameObject.SetActive(false);
        StartCoroutine(WaitForRestart());
        pauseMenu.gameObject.SetActive(true);
        pauseMenu.StartState();
        settings.WasInit = true;
    }

    private void OnPlayerHit()
    {
        StartCoroutine(PlayerHit());
    }
    
    private IEnumerator PlayerHit()
    {
        yield return new WaitForSeconds(1f);
        
        if (playerData.Lives <= 0)
        {
            GameOver();
        }
        else
        {
            player.Reset();
        }
        
    }

    private void OnPlayerDataChange()
    {
        if (playerData.Score > highScore)
        {
            highScore = playerData.Score;
            pauseMenu.UpdateHighScoreText(highScore);
            
            if (!newHighScore)
            {
                SaveScore();
                newHighScore = true;
                Hud.OnNewHighScore();
            }
        }
    }

    void Update()
    {
        if (!gameOver && Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    void Pause()
    {
        Time.timeScale = Math.Abs(Time.timeScale - 1) < Mathf.Epsilon ? 0 : 1;
        pauseMenu.PauseState();
        pauseMenu.gameObject.SetActive(!pauseMenu.gameObject.activeSelf);
        player.enabled = !player.enabled;
    }

    void FillList()
    {
        for (int i = 0; i < settings.levels[level]; i++)
            asteroidsList.Add(Instantiate(settings.asteroid, GetRandomPosition(), Quaternion.identity));
        bigCount += settings.levels[level];
    }

    // Возвращает случайные коордианты игрового мира на границе экрана.
    Vector2 GetRandomPosition()
    {
        byte[] side = new byte[4];
        side[Random.Range(0, 3)] = 1;
        int x = Mathf.Max(Mathf.Max(0, Screen.width * side[2]), Random.Range(0, Screen.width) * Mathf.Max(side[1], side[3]));
        int y = Mathf.Max(Mathf.Max(0, Screen.height * side[3]), Random.Range(0, Screen.height) * Mathf.Max(side[0], side[2]));
        Vector2 screenPoint = new Vector2(x, y);
        return Camera.main.ScreenToWorldPoint(screenPoint);
    }

    private void RemoveAsteroid(AsteroidController asteroid)
    {
        if (!asteroid) return;
        
        if (asteroid.IsBig) bigCount--;
        
        if (asteroidsList.Contains(asteroid))
            asteroidsList.Remove(asteroid);
        
        InstantiateChilds(asteroid);
        
        if (!gameOver)
        {
            CheckForEmpty();
        }
    }
    
    private void InstantiateChilds(AsteroidController asteroid)
    {
        var asteroidData = asteroid.Data;
        if (asteroidData.child != null)
        {
            for (int i = 0; i < asteroidData.childCount; i++)
            {
                asteroidsList.Add(Instantiate(asteroidData.child, asteroid.transform.position, Quaternion.identity));
            }
        }
    }

    void CheckForEmpty()
    {
        if (asteroidsList.Count == 0)
        {
            StopCoroutine(AddSaucerCoroutine());
            level = Mathf.Min(settings.levels.Length - 1, level + 1);
            FillList();
        }
    }

    void AddSaucer()
    {
        int index = Random.Range(0, 100) % 2 == 0 ? 1 : 0;
        Instantiate(settings.saucers[index], GetRandomPosition(), Quaternion.identity);
    }

    IEnumerator AddSaucerCoroutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => bigCount < settings.levels[level] * settings.saucerRespCondition);
            do
            {
                AddSaucer();
                yield return new WaitForSeconds(Random.Range(settings.saucerRespTimeRange.x, settings.saucerRespTimeRange.y));
            } while ((bigCount < settings.levels[level] * settings.saucerRespCondition) && !gameOver);
        }
    }

    public void GameOver()
    {
        gameOver = true;
        
        if (newHighScore) SaveScore();
        
        pauseMenu.GameOverState(playerData.Score);
        pauseMenu.gameObject.SetActive(true);
        
        StartCoroutine(WaitForRestart());
    }

    IEnumerator WaitForRestart()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
        ResetGame();
    }

    void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Exit()
    {
        if (newHighScore) SaveScore();
        Application.Quit();
    }

    void SaveScore()
    {
        DataLoader.SaveScore(highScore);
    }

    void LoadScore()
    {
        highScore = DataLoader.LoadScore();
        pauseMenu.UpdateHighScoreText(highScore);
    }

    private void OnDestroy()
    {
        if (playerData)
        {
            playerData.OnChange -= OnPlayerDataChange;
            playerData.OnHit -= OnPlayerHit;
        }

        AsteroidController.OnAsteroidDestroy -= RemoveAsteroid;
    }
}
