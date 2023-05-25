using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public GameObject Mesh;

    private Animator _animator;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _animator = Mesh.GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        _animator.SetFloat("Speed", gameObject.GetComponent<CharacterController>().velocity.magnitude);
        Debug.Log(gameObject.GetComponent<CharacterController>().velocity.magnitude);
    }
}
