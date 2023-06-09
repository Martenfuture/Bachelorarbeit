using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    public int WaveNumber = 0;
    public int EnemiesPerWave = 10;

    private void Awake()
    {
        instance = this;
    }

    public void StartWave()
    {
        WaveNumber++;
        EnemySpawning.instance.EnemyWaveRemaining = EnemiesPerWave;
        EnemySpawning.instance.StartSpawning();

        UIHandler.instance.UpdateUIWave(WaveNumber);
        UIHandler.instance.UpdateUIEnemy(EnemiesPerWave);
    }

    public void WaveEnded(int restartDelay)
    {
        UIHandler.instance.NextWaveCountdown(restartDelay);
    }
}
