using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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

    [Header("Exponential Difficulty")]
    public int ExponentialBaseDifficulty;

    ExponentialDifficulty exponentialDifficulty;

    private void Start()
    {
        exponentialDifficulty = gameObject.GetComponent<ExponentialDifficulty>();
    }

    private void Update()
    {
        float difficulltyMultiplier = exponentialDifficulty.GetDifficultyMultiplier(ExponentialBaseDifficulty);
        DifficultySetting difficultySetting = new DifficultySetting(difficulltyMultiplier, difficulltyMultiplier, difficulltyMultiplier, difficulltyMultiplier, difficulltyMultiplier, difficulltyMultiplier, difficulltyMultiplier, difficulltyMultiplier);
        GameManager.instance.ChangeDifficulty(difficultySetting);
    }


}
