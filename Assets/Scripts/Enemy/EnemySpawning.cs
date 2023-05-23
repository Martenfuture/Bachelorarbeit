using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.Switch;

public class EnemySpawning : MonoBehaviour
{
    public GameObject Enemy;
    public int EnemyPerMinute = 10;

    public EnemyParameter EnemyParameter;
    public GameObject EnemyPostion;
    public LayerMask RayCastLayerMask;

    public int EnemyCount { get { return gameObject.transform.GetChild(0).childCount; } }

    private List<Vector3> _enemyPositions;

    private bool _parameterChanged = false;

    private void Start()
    {
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
        StartCoroutine(SpawnerLoop());
    }

    public void ChangeParameters(EnemyParameter enemyParameter, int enemyPerMinute)
    {
        if (!_parameterChanged)
        {
            StopCoroutine(ChangeParameterDelay(enemyParameter, enemyPerMinute));
        } else
        {
            Debug.Log("Parameter allready changed");
        }
    }
    private void SpawnMinute()
    {
        StartCoroutine(SpawnLoopCoroutine());
    }
    private void SpawnEnemy()
    {
        Vector3 position = _enemyPositions[Random.Range(0, _enemyPositions.Count)];
        GameObject newEnemy = Instantiate(Enemy, position, Quaternion.identity, gameObject.transform.GetChild(0));
        newEnemy.GetComponent<EnemyController>().EnemyParameter = EnemyParameter;
        Debug.Log(EnemyCount);
    }

    IEnumerator SpawnerLoop()
    {
        while (true)
        {
            if (_parameterChanged)
            {
                _parameterChanged = false;
            }
            else
            {
                SpawnMinute();
            }
            yield return new WaitForSeconds(60);
        }
    }

    IEnumerator SpawnLoopCoroutine()
    {
        int spawnCount = EnemyPerMinute;
        while (spawnCount > 0)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(60f / EnemyPerMinute);
            spawnCount--;
        }
    }
    
    IEnumerator ChangeParameterDelay(EnemyParameter enemyParameter, int enemyPerMinute)
    {
        _parameterChanged = true;
        yield return new WaitUntil(() => !_parameterChanged);

        EnemyParameter = enemyParameter;
        EnemyPerMinute = enemyPerMinute;
        SpawnMinute();
    }
}
