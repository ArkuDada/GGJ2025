using System;
using UnityEngine;

public class CaptureBubble : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
 
    public float Speed = 1.0f;
    public float _lifeTime = 5.0f;

    private Rigidbody _rigidbody;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Destroy(gameObject, _lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        _rigidbody.linearVelocity = Vector3.down * Speed;
    }
}
