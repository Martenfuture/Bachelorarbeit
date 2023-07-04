using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamletSystem : MonoBehaviour
{
    public float OldHamletScore = 1;

    [Header("Player Performance")]
    public float HealthLostPerWave; // 30
    public float HitQuotesPerWave; // 0.5
    public float EnemiesKilledPerMinute; // 10

    [Header("Player Performance Goal")]
    public float HealthLostPerWaveGoal;
    public float HitQuotesPerWaveGoal;
    public float EnemiesKilledPerMinuteGoal; // maximum possible minus 20%


    public float CalculateHamletScore()
    {
        // over 1 = good, under 1 = bad
        float hamletScore = 0;

        HealthLostPerWave = PlayerController.instance.ReseavedDamge;
        PlayerController.instance.ReseavedDamge = 0;
        HitQuotesPerWave = WeaponHandler.instance.GetAccuracy();
        EnemiesKilledPerMinute = EnemySpawning.instance.GetKillsPerMinute();

        EnemiesKilledPerMinuteGoal = EnemySpawning.instance.EnemyPerMinute * 0.8f;

        float healthLostPerWaveScore = Mathf.Clamp(Mathf.Lerp(1.5f, 0.5f, (HealthLostPerWave / (2 * HealthLostPerWaveGoal))), 0.5f, 1.5f);
        float hitQuotesPerWaveScore = HitQuotesPerWave / HitQuotesPerWaveGoal;
        float enemiesKilledPerMinuteScore = EnemiesKilledPerMinute / EnemiesKilledPerMinuteGoal;

        hamletScore = (healthLostPerWaveScore + hitQuotesPerWaveScore + enemiesKilledPerMinuteScore) / 3;
        hamletScore = hamletScore * (2f/3f) + OldHamletScore * (1f/3f);

        Debug.Log("HealthLost: " + healthLostPerWaveScore + "   HitQuote: " + hitQuotesPerWaveScore + "    EnemiesPerMin: " + enemiesKilledPerMinuteScore);
        Debug.Log("HamletScore: " + hamletScore);
        OldHamletScore = hamletScore;

        return hamletScore;
    }

    public DifficultySetting GetHamletDifficultySetting(int wave)
    {
        if (wave == 0)
        {
            return new DifficultySetting(1, 1, 1, 1, 1, 1, 1, 1);
        }
        float hamletScore = CalculateHamletScore();
        float waveFloat = (float)wave / 10f + 1;
        float difficultyMultiplier = hamletScore * waveFloat;
        Debug.Log("DifficultyMultiplier: " + difficultyMultiplier);
        return new DifficultySetting(difficultyMultiplier, difficultyMultiplier, difficultyMultiplier, difficultyMultiplier, difficultyMultiplier, difficultyMultiplier, difficultyMultiplier, difficultyMultiplier);
    }
}
