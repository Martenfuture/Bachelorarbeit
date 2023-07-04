using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DifficultyTesting : MonoBehaviour
{
    public int Wave;

    [Header("Exponential Difficulty")]
    public int ExponentialBaseDifficulty;

    ExponentialDifficulty exponentialDifficulty;

    private void Start()
    {
        exponentialDifficulty = gameObject.GetComponent<ExponentialDifficulty>();
    }

    private void Update()
    {
        GameManager.instance.ChangeDifficulty(GetComponent<HamletSystem>().GetHamletDifficultySetting(Wave));
    }


}
