using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float Seconds = 3f;
    void Start()
    {
        StartCoroutine(DestroyAfterSeconds());
    }

    IEnumerator DestroyAfterSeconds()
    {
        yield return new WaitForSeconds(Seconds);
        Destroy(gameObject);
    }
}
