using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimation : MonoBehaviour
{
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;

    private void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
        _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        UpdateAnimation();
    }



    private void UpdateAnimation()
    {
        _animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude);
    }
}
