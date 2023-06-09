using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private EnemyParameter _enemyParameter;

    private Animator _animator;
    private NavMeshAgent _navMeshAgent;

    public GameObject DestroyEffect;

    public bool Hunting = true;

    private void Start()
    {
        GameManager.instance.OnGameStart += OnGameStart;
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

    public void TakeDamage(float damage)
    {
        _enemyParameter.Health -= damage;
        //Debug.Log("Enemy Health: " + _enemyParameter.Health);
        if (_enemyParameter.Health <= 0)
        {
            GameObject destoyEffect = Instantiate(DestroyEffect, transform.position + new Vector3(0,1,0), Quaternion.identity);
            EnemySpawning.instance.EnemyDied(destoyEffect);

            Destroy(gameObject);
        }
    }

    private void OnGameStart()
    {
        if (_enemyParameter == null)
        {
            EnemyParameter gameEnemyParameter = GameManager.instance.StartEnemyParameter;
            _enemyParameter = new EnemyParameter(gameEnemyParameter.Speed, gameEnemyParameter.Health, gameEnemyParameter.Damage);
        }
    }

    public void SetParameter(EnemyParameter parameter)
    {
        _enemyParameter = parameter;
        Debug.Log("Enemy Health: " + _enemyParameter.Health);
    }

    IEnumerator MoveToPlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            if (Hunting) gameObject.GetComponent<NavMeshAgent>().SetDestination(PlayerController.instance.transform.position);
        }
    }
}
