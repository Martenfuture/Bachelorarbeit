using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public EnemyParameter StartEnemyParameter = new EnemyParameter(1f, 100f, 10);
    public int StartEnemyPerMinute;

    public PlayerInputActions PlayerControls;

    public LayerMask ShootingLayerMask;

    public int DifficultyType;


    public GameObject Camera;

    public float EnemySpawnDelayTime = 0.1f;

    public DifficultySetting CurrentDifficultySettings;

    private InputAction _restartGame;

    public event Action OnGameStart;
    public event Action<DifficultySetting> OnDifficultyChange;
    public event Action OnGameOver;

    private void Awake()
    {
        instance = this;
        PlayerControls = new PlayerInputActions();
        DifficultyType = PlayerPrefs.GetInt("DifficultyType");
    }
    private void Start()
    {
        SetStartValues();
        WaveManager.instance.WaveEnded(5);
        StartCoroutine(GameStartDelay());
    }

    private void OnDisable()
    {
        if (_restartGame != null)
        {
            _restartGame.Disable();
        }
    }

    private void SetStartValues()
    {
    }

    public void StartGame()
    {
        OnGameStart?.Invoke();
    }

    private void RestartGame(InputAction.CallbackContext context)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ChangeDifficulty(DifficultySetting diffícultySetting)
    {
        StartCoroutine(ChangeDifficultyDelay(diffícultySetting));
    }

    public void PlayerDied()
    {
        _restartGame = PlayerControls.Player.Space;
        _restartGame.Enable();
        _restartGame.performed += RestartGame;

        OnGameOver?.Invoke();
        Time.timeScale = 0;

        PlayerPrefs.SetInt("TotalDeaths", PlayerPrefs.GetInt("TotalDeaths", 0) + 1);
        PlayerPrefs.SetInt("MaxWave", Mathf.Max(PlayerPrefs.GetInt("MaxWave", 0), WaveManager.instance.WaveNumber));
    }

    IEnumerator GameStartDelay()
    {
        yield return new WaitForSeconds(0.1f);
        StartGame();
    }

    IEnumerator ChangeDifficultyDelay(DifficultySetting diffícultySetting)
    {
        yield return new WaitForEndOfFrame();
        OnDifficultyChange?.Invoke(diffícultySetting);
        CurrentDifficultySettings = diffícultySetting;
    }
}
