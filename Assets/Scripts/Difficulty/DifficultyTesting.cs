using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyTesting : MonoBehaviour
{
    public float PlayerHealthMultiplier;
    public float EnemiesPerMinuteMultiplier;
    public float EnemiesPerWaveMultiplier;
    public float EnemySpeedMultiplier;
    public float EnemyHealthMultiplier;
    public float EnemyDamageMultiplier;
    public float WeaponDamageMultiplier;
    public float WeaponFirerateMultiplier;

    private void Update()
    {
        DifficultySetting difficultySetting = new DifficultySetting(PlayerHealthMultiplier, EnemiesPerMinuteMultiplier, EnemiesPerWaveMultiplier, EnemySpeedMultiplier, EnemyHealthMultiplier, EnemyDamageMultiplier, WeaponDamageMultiplier, WeaponFirerateMultiplier);
        GameManager.instance.ChangeDifficulty(difficultySetting);
    }
}
