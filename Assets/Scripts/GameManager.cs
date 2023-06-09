using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public EnemyParameter StartEnemyParameter;
    public int StartEnemyPerMinute;

    public LayerMask ShootingLayerMask;


    public GameObject Camera;

    public float EnemySpawnDelayTime = 0.1f;

    public event Action OnGameStart;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        SetStartValues();
        EnemySpawning.instance.ChangeParameters(StartEnemyParameter, StartEnemyPerMinute);
        WaveManager.instance.WaveEnded(5);
        StartCoroutine(GameStartDelay());
    }

    private void SetStartValues()
    {
        StartEnemyParameter = new EnemyParameter(1f, 100f, 10);
    }

    public void StartGame()
    {
        OnGameStart?.Invoke();
    }

    IEnumerator GameStartDelay()
    {
        yield return new WaitForSeconds(0.1f);
        StartGame();
    }
}
