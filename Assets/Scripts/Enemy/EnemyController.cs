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
        Debug.Log("Enemy Health: " + _enemyParameter.Health);
        if (_enemyParameter.Health <= 0)
        {
            GameObject destoyEffect = Instantiate(DestroyEffect, transform.position + new Vector3(0,1,0), Quaternion.identity);
            Destroy(gameObject);
            StartCoroutine(DeleteDestoyEffct(destoyEffect));
        }
    }

    private void OnGameStart()
    {
        if (_enemyParameter == null)
        {
            _enemyParameter = GameManager.instance.StartEnemyParameter;
        }
    }

    public void SetParameter(EnemyParameter parameter)
    {
        _enemyParameter = new EnemyParameter(parameter.Speed, parameter.Health,parameter.Damage);
    }

    IEnumerator DeleteDestoyEffct(GameObject destoyEffect)
    {
        yield return new WaitForSeconds(3f);
        Destroy(destoyEffect);
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
