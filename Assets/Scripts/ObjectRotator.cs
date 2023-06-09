using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    public float Speed = 10f;
    public float SpeedHorizontal = 1f;
    public float Height = 0.2f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, 0, Speed * Time.deltaTime));

        Vector3 newPosition = startPosition + Vector3.up * Mathf.Sin(Time.time * SpeedHorizontal) * Height;

        transform.position = newPosition;
    }
}
