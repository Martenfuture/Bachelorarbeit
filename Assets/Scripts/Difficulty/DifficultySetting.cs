using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultySetting
{
    public float PlayerHealthMultiplier;
    public float EnemiesPerMinuteMultiplier;
    public float EnemiesPerWaveMultiplier;
    public float EnemySpeedMultiplier;
    public float EnemyHealthMultiplier;
    public float EnemyDamageMultiplier;
    public float WeaponDamageMultiplier;
    public float WeaponFirerateMultiplier;

    public DifficultySetting(float playerHealthMultiplier, float enemiesPerMinuteMultiplier, float enemiesPerWaveMultiplier, float enemySpeedMultiplier, float ememyHealthMultiplier, float enemyDamageMultiplier, float weaponDamageMultiplier, float weaponFirerateMultiplier)
    {
        PlayerHealthMultiplier = playerHealthMultiplier;
        EnemiesPerMinuteMultiplier = enemiesPerMinuteMultiplier;
        EnemiesPerWaveMultiplier = enemiesPerWaveMultiplier;
        EnemySpeedMultiplier = enemySpeedMultiplier;
        EnemyHealthMultiplier = ememyHealthMultiplier;
        EnemyDamageMultiplier = enemyDamageMultiplier;
        WeaponDamageMultiplier = weaponDamageMultiplier;
        WeaponFirerateMultiplier = weaponFirerateMultiplier;
    }
}
