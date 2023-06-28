using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRandomIdle : MonoBehaviour
{
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;

    private void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
        _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        StartCoroutine(RandomIdle());
    }

    IEnumerator RandomIdle()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(9, 15));
            int randomIdle = Random.Range(1, 5);
            _animator.SetTrigger("Idle" + randomIdle);
        }
    }
}
