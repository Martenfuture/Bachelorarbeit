using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    public int WaveNumber = 0;
    public int StartEnemiesPerWave = 10;
    private int _enemiesPerWave = 10;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GameManager.instance.OnDifficultyChange += ChangeDifficulty;
    }
    public void StartWave()
    {
        WaveNumber++;
        EnemySpawning.instance.EnemyWaveRemaining = _enemiesPerWave;
        EnemySpawning.instance.StartSpawning();

        UIHandler.instance.UpdateUIWave(WaveNumber);
        UIHandler.instance.UpdateUIEnemy(_enemiesPerWave);
    }

    public void WaveEnded(int restartDelay)
    {
        UIHandler.instance.NextWaveCountdown(restartDelay);
    }

    private void ChangeDifficulty(DifficultySetting diffícultySetting)
    {
        _enemiesPerWave = Mathf.RoundToInt(StartEnemiesPerWave * diffícultySetting.EnemiesPerWaveMultiplier);
        UIHandler.instance.UpdateUIVariable("EnemiesPerWave", _enemiesPerWave.ToString());
    }
}
