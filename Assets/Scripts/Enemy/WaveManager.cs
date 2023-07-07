using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    public int WaveNumber = 0;
    public int StartEnemiesPerWave = 10;
    private int _enemiesPerWave = 5;

    public int WaveTime;

    private bool _isWaveActive = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GameManager.instance.OnDifficultyChange += ChangeDifficulty;
        _enemiesPerWave = StartEnemiesPerWave;
    }
    public void StartWave()
    {
        WaveNumber++;
        StartCoroutine(WaveTimer());
        EnemySpawning.instance.EnemyWaveRemaining = _enemiesPerWave;
        EnemySpawning.instance.StartSpawning();
        UIHandler.instance.UpdateUIWave(WaveNumber);
        UIHandler.instance.UpdateUIEnemy(_enemiesPerWave);
    }

    public void WaveEnded(int restartDelay)
    {
        _isWaveActive = false;
        UIHandler.instance.NextWaveCountdown(restartDelay);
        float difficulltyMultiplier = GameManager.instance.gameObject.GetComponent<ExponentialDifficulty>().GetDifficultyMultiplier(WaveNumber);
        DifficultySetting difficultySetting = null;

        int difficultyType = GameManager.instance.DifficultyType;
        if (difficultyType == 0)
        {
            UIHandler.instance.UpdateUIVariable("HamletScore", "NA");
            difficulltyMultiplier = GameManager.instance.gameObject.GetComponent<ExponentialDifficulty>().GetDifficultyMultiplier(WaveNumber);
            difficultySetting = new DifficultySetting(difficulltyMultiplier, difficulltyMultiplier, difficulltyMultiplier, difficulltyMultiplier, difficulltyMultiplier, difficulltyMultiplier, difficulltyMultiplier, difficulltyMultiplier);
        }
        else if (difficultyType == 1)
        {
            difficultySetting = GameManager.instance.gameObject.GetComponent<HamletSystem>().GetHamletDifficultySetting(WaveNumber);
        }
        GameManager.instance.ChangeDifficulty(difficultySetting);
    }

    private void ChangeDifficulty(DifficultySetting diffícultySetting)
    {
        _enemiesPerWave = Mathf.RoundToInt(StartEnemiesPerWave * diffícultySetting.EnemiesPerWaveMultiplier * 1.5f);
        UIHandler.instance.UpdateUIVariable("EnemiesPerWave", (Mathf.Round(_enemiesPerWave * 100) / 100).ToString());
    }

    IEnumerator WaveTimer()
    {
        _isWaveActive = true;
        WaveTime = 0;
        while (_isWaveActive)
        {
            yield return new WaitForSeconds(1);
            WaveTime++;
        }
    }
}
