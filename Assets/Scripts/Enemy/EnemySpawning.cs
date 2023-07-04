using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.Switch;

public class EnemySpawning : MonoBehaviour
{
    public static EnemySpawning instance;
    public GameObject Enemy;
    public int EnemyPerMinute = 10;
    private int _startEnemyPerMinute;

    public int EnemyWaveRemaining;

    public EnemyParameter EnemyParameter;
    private EnemyParameter _startEnemyParameter;
    public GameObject EnemyPostion;
    public LayerMask RayCastLayerMask;

    public int EnemyCount { get { return gameObject.transform.GetChild(0).childCount; } }

    private List<Vector3> _enemyPositions;

    public int EnemysAlive;

    private bool _parameterChanged = false;

    //Kill Counter
    private int _killCount = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GameManager.instance.OnDifficultyChange += ChangeDifficulty;
        _enemyPositions = new List<Vector3>();
        foreach (Transform child in EnemyPostion.transform)
        {
            RaycastHit hit;
            if (Physics.Raycast(child.position, Vector3.down, out hit, 100f, RayCastLayerMask))
            {
                _enemyPositions.Add(hit.point);
            }
            else
            {
                Debug.LogError("Enemy Position not Hit: " + child.name);
            }
        }
        //StartCoroutine(SpawnerLoop());
        _startEnemyParameter = GameManager.instance.StartEnemyParameter;
        _startEnemyPerMinute = GameManager.instance.StartEnemyPerMinute;
    }

    public void StartSpawning()
    {
        EnemysAlive = EnemyWaveRemaining;
        StartCoroutine(SpawnerLoop());
    }

    public IEnumerator SpawnerLoop()
    {
        while (EnemyWaveRemaining > 0)
        {
            if (_parameterChanged)
            {
                _parameterChanged = false;
                yield return new WaitForSeconds(0.1f);
            }
            SpawnMinute();
            yield return new WaitForSeconds(60);
        }
    }
    private void SpawnMinute()
    {
        StartCoroutine(SpawnLoopCoroutine());
    }

    IEnumerator SpawnLoopCoroutine()
    {
        int spawnCount = EnemyPerMinute;
        while (spawnCount > 0 && EnemyWaveRemaining > 0)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(60f / EnemyPerMinute);
            spawnCount--;
        }
    }
    private void SpawnEnemy()
    {
        Vector3 position = _enemyPositions[Random.Range(0, _enemyPositions.Count)];
        GameObject newEnemy = Instantiate(Enemy, position, Quaternion.identity, gameObject.transform.GetChild(0));

        EnemyParameter newEnemyParameter = new EnemyParameter(EnemyParameter.Speed, EnemyParameter.Health, EnemyParameter.Damage);
        newEnemy.GetComponent<EnemyController>().SetParameter(newEnemyParameter);
        EnemyWaveRemaining--;
    }

    public void ChangeParameters(EnemyParameter enemyParameter, int enemyPerMinute)
    {
        if (!_parameterChanged)
        {
            StartCoroutine(ChangeParameterDelay(enemyParameter, enemyPerMinute));
        } else
        {
            Debug.Log("Parameter allready changed");
        }
    }

    public void EnemyDied(GameObject destroyEffect)
    {
        EnemysAlive--;
        UIHandler.instance.UpdateUIEnemy(EnemysAlive);
        StartCoroutine(DeleteDestoyEffctDelay(destroyEffect));
        _killCount++;
        if (EnemysAlive <= 0)
        {
            WaveManager.instance.WaveEnded(15);
        }
    }

    private void ChangeDifficulty(DifficultySetting diffícultySetting)
    {
        EnemyParameter newEnemyParameter = new EnemyParameter(
            diffícultySetting.EnemySpeedMultiplier * _startEnemyParameter.Speed,
            _startEnemyParameter.Health * (1.0f + (diffícultySetting.EnemyHealthMultiplier - 1) * 0.5f),
            Mathf.RoundToInt(diffícultySetting.EnemyDamageMultiplier * _startEnemyParameter.Damage)
            );
        StartCoroutine(ChangeParameterDelay(newEnemyParameter, Mathf.RoundToInt(diffícultySetting.EnemiesPerMinuteMultiplier * _startEnemyPerMinute * 1.25f)));
        //UIHandler.instance.UpdateUIVariable("EnemySpeed", newEnemyParameter.Speed.ToString());
        UIHandler.instance.UpdateUIVariable("EnemyHealth", newEnemyParameter.Health.ToString());
        UIHandler.instance.UpdateUIVariable("EnemyDamage", newEnemyParameter.Damage.ToString());
        UIHandler.instance.UpdateUIVariable("EnemiesPerMinute", Mathf.RoundToInt(diffícultySetting.EnemiesPerMinuteMultiplier * _startEnemyPerMinute).ToString());
    }
    
    IEnumerator ChangeParameterDelay(EnemyParameter enemyParameter, int enemyPerMinute)
    {
        _parameterChanged = true;
        yield return new WaitUntil(() => !_parameterChanged);

        EnemyParameter = enemyParameter;
        EnemyPerMinute = enemyPerMinute;
    }
    IEnumerator DeleteDestoyEffctDelay(GameObject destoyEffect)
    {
        yield return new WaitForSeconds(3f);
        Destroy(destoyEffect);
    }

    public float GetKillsPerMinute()
    {
        float killsPerMinute = _killCount / ((float)WaveManager.instance.WaveTime / 60);
        _killCount = 0;
        return killsPerMinute;
    }
}
