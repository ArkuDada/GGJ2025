using System;
using UnityEngine;

public class PriceObject : MonoBehaviour
{
    public enum ObjectState
    {
        Grounded,
        Up,
        Down
    }
    
    public ObjectState State = ObjectState.Grounded;
    
    public GameObject b;
    private Rigidbody _rigidbody;
    private MeshRenderer _meshRenderer;
    
    public float floatSpeed = 1.0f;
    
    public Material _upMaterial;
    public Material _downMaterial;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("FloorPlane"))
        {
            _meshRenderer.material = _upMaterial;
            State = ObjectState.Grounded;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Beam"))
        {
            b.SetActive(true);
            _rigidbody.useGravity = false;
            _rigidbody.linearVelocity = Vector3.up * floatSpeed;
            State = ObjectState.Up;
            
        }

        if(other.CompareTag("FallPlane"))
        {
            b.SetActive(false);
            _meshRenderer.material = _downMaterial;
            _rigidbody.useGravity = true;
            _rigidbody.linearVelocity = Vector3.zero;
            State = ObjectState.Down;
        }
    }
}
