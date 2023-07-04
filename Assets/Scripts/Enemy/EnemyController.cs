using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private EnemyParameter _enemyParameter;

    public GameObject DestroyEffect;

    public bool Hunting = true;
    private bool _firing = false;
    private LayerMask _rayCastLayerMask;

    private int _maxHealth;
    private Material _material;

    private void Start()
    {
        _rayCastLayerMask = GameManager.instance.ShootingLayerMask;
        GameManager.instance.OnGameStart += OnGameStart;

        _maxHealth = (int) _enemyParameter.Health;
        _material = transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material;
        StartCoroutine(MoveToPlayer());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_firing)
        {
            _firing = true;
            StartCoroutine(FiringLoop());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _firing = false;
        }
    }

    public void TakeDamage(float damage)
    {
        _enemyParameter.Health -= damage;
        float healthRatio = _enemyParameter.Health / _maxHealth;
        _material.color = new Color(1, healthRatio, healthRatio);
        //Debug.Log("Enemy Health: " + _enemyParameter.Health);
        if (_enemyParameter.Health <= 0)
        {
            GameObject destoyEffect = Instantiate(DestroyEffect, transform.position + new Vector3(0,1,0), Quaternion.identity);
            EnemySpawning.instance.EnemyDied(destoyEffect);

            Destroy(gameObject);
        }
    }

    private void FireOnPlayer()
    {
        float distance = Vector3.Distance(transform.position, PlayerController.instance.transform.position);
        float hitChance = 0.5f - (distance / GetComponent<SphereCollider>().radius * 0.3f);

        Vector3 direction = (PlayerController.instance.transform.position - transform.GetChild(2).position).normalized;
        Debug.DrawRay(transform.GetChild(2).position, direction * 10, Color.green, 1f);
        if (IsHit(hitChance))
        {

            RaycastHit hit;
            if (Physics.Raycast(transform.GetChild(2).position, (PlayerController.instance.transform.position - transform.GetChild(2).position).normalized, out hit, 100f, _rayCastLayerMask) && !hit.transform.CompareTag("Player"))
            {
                // Effect needed
            }
            else
            {
                PlayerController.instance.TakeDamage(_enemyParameter.Damage);
            }
        }

        GameObject muzzleEffect = Instantiate(WeaponHandler.instance.MuzzleEffect, transform.GetChild(2).GetChild(0).position, Quaternion.identity, transform.GetChild(2).GetChild(0));
        muzzleEffect.transform.LookAt(transform.GetChild(2).GetChild(0).forward);
    }

    private bool IsHit(float hitChance)
    {
        float randomValue = Random.Range(0f, 1f);
        return randomValue <= hitChance;
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
        _maxHealth = (int) _enemyParameter.Health;
    }

    IEnumerator FiringLoop()
    {
        while (_firing)
        {
            FireOnPlayer();
            yield return new WaitForSeconds(2f);
        }
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
