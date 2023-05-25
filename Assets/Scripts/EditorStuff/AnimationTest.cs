using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    [Range(0.0f, 3.5f)]
    public float Speed;

    private void Update()
    {
        gameObject.GetComponent<Animator>().SetFloat("Speed", Speed);
    }
}
