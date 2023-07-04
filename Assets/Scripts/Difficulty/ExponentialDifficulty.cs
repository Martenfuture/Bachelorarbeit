using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExponentialDifficulty : MonoBehaviour
{
    [SerializeField] private float _baseDifficulty = 1f;
    [SerializeField] private float _difficultyIncreasePerWave = 1f;

    public float GetDifficultyMultiplier(int wave)
    {
        float difficultyMultiplier = _baseDifficulty * Mathf.Pow(_difficultyIncreasePerWave, wave);
        UIHandler.instance.UpdateUIVariable("DifficultyMultiplier", difficultyMultiplier.ToString());
        return difficultyMultiplier;
    }
}
