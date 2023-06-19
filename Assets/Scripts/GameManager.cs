using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public EnemyParameter StartEnemyParameter = new EnemyParameter(1f, 100f, 10);
    public int StartEnemyPerMinute;

    public LayerMask ShootingLayerMask;


    public GameObject Camera;

    public float EnemySpawnDelayTime = 0.1f;

    public DifficultySetting CurrentDifficultySettings;

    public event Action OnGameStart;
    public event Action<DifficultySetting> OnDifficultyChange;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        SetStartValues();
        WaveManager.instance.WaveEnded(5);
        StartCoroutine(GameStartDelay());
    }

    private void SetStartValues()
    {
    }

    public void StartGame()
    {
        OnGameStart?.Invoke();
    }

    public void ChangeDifficulty(DifficultySetting diffícultySetting)
    {
        StartCoroutine(ChangeDifficultyDelay(diffícultySetting));
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
