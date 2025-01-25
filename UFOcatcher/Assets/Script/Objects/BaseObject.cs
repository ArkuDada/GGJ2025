using System;
using UnityEngine;
using UnityEngine.Serialization;
using Utility;

public class BaseObject : MonoBehaviour
{
    public enum ObjectState
    {
        Grounded,
        Up,
        Down
    }
    
    public ObjectState State = ObjectState.Grounded;
    
    public Objects.ObjectType Type;
    
    public GameObject b;
    private Rigidbody _rigidbody;
    private MeshRenderer _meshRenderer;
    
    public float floatSpeed = 1.0f;

    [FormerlySerializedAs("_Score")]
    [SerializeField]private float _BaseScore = 1;
    
    public Material _upMaterial;
    public Material _downMaterial;
    
    int _score = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    public virtual int GetScore()
    {
        return _score;
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
        if(other.CompareTag("Bubble"))
        {
            b.SetActive(true);
            _rigidbody.useGravity = false;
            _rigidbody.linearVelocity = Vector3.up * floatSpeed;
            State = ObjectState.Up;
            
            Destroy(other.gameObject);
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
