using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public EnemyParameter EnemyParameter;


    private void Start()
    {
        StartCoroutine(MoveToPlayer());
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
