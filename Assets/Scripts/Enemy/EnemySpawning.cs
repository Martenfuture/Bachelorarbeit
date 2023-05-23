using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Switch;

public class EnemySpawning : MonoBehaviour
{
    public GameObject Enemy;
    public int EnemyPerMinute = 10;

    public EnemyParameter EnemyParameter;
    public GameObject EnemyPostion;
    public LayerMask RayCastLayerMask;

    private List<Vector3> _enemyPositions;

    private bool _isSpawning = false;

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
        if (_isSpawning)
        {
            StopCoroutine(ChangeParameterDelay(enemyParameter, enemyPerMinute));
        } else
        {
            EnemyParameter = enemyParameter;
            EnemyPerMinute = enemyPerMinute;
        }
    }
    private void SpawnMinute()
    {
        if (_isSpawning) { Debug.LogError("Spawning out of Sync"); }
        _isSpawning = true;
        StartCoroutine(SpawnLoopCoroutine());
    }
    private void SpawnEnemy()
    {
        Vector3 position = _enemyPositions[Random.Range(0, _enemyPositions.Count)];
        GameObject newEnemy = Instantiate(Enemy, position, Quaternion.identity);
        newEnemy.GetComponent<EnemyController>().EnemyParameter = EnemyParameter;
    }

    IEnumerator SpawnerLoop()
    {
        SpawnMinute();
        yield return new WaitForSeconds(60);
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

        _isSpawning = false;
    }
    
    IEnumerator ChangeParameterDelay(EnemyParameter enemyParameter, int enemyPerMinute)
    {
        yield return new WaitUntil(() => !_isSpawning);

        EnemyParameter = enemyParameter;
        EnemyPerMinute = enemyPerMinute;
    }
}
