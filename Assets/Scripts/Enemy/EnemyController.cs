using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public EnemyParameter EnemyParameter;

    private Animator _animator;
    private NavMeshAgent _navMeshAgent;

    private void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
        _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        StartCoroutine(MoveToPlayer());
    }

    private void Update()
    {
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        _animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude);
    }

    IEnumerator MoveToPlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            gameObject.GetComponent<NavMeshAgent>().SetDestination(PlayerController.instance.transform.position);
        }
    }
}
