using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
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

	public GameObject _bubble;
	private Rigidbody _rigidbody;
	private MeshRenderer _meshRenderer;

	public float floatSpeed = 1.0f;

	[FormerlySerializedAs("_Score")]
	[SerializeField] private float _BaseScore = 1;

	public Material _upMaterial;
	public Material _downMaterial;

	int _score = 0;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		if (_bubble == null)
		{
			// Bubble hasn't been set, attempt to auto-find it
			Transform bubbleTransform = transform.Find("Bubble");
			if (bubbleTransform != null)
			{
				_bubble = bubbleTransform.gameObject;
			}
		}
		_bubble.SetActive(false);
		_rigidbody = GetComponent<Rigidbody>();
		_meshRenderer = GetComponentInChildren<MeshRenderer>();
	}

	// Update is called once per frame
	protected virtual void Update()
	{

	}

	private void FixedUpdate()
	{
		// Needs to be set every frame in case it bumps into something
		if (State == ObjectState.Up)
		{
			_rigidbody.linearVelocity = Vector3.up * floatSpeed;
		}
	}

	public virtual int GetScore()
	{
		return _score;
	}

	private void OnCollisionEnter(Collision other)
	{
		if (State != ObjectState.Grounded && other.gameObject.CompareTag("FloorPlane"))
		{
			_meshRenderer.material = _upMaterial;
			State = ObjectState.Grounded;

			if (TryGetComponent<MoveableObject>(out var moveableObject))
			{
				moveableObject.enabled = true;
			}
		}
	}

	// Encapsulate in a bubble
	public void BubbleLift()
	{
		Debug.Log(Type);
		switch (Type)
		{
			case Objects.ObjectType.Wheat:
				var wheatData = GetComponent<WheatData>();
				if (wheatData.beingEaten)
					wheatData.cowEating.StopEating();
				break;
			case Objects.ObjectType.Cow:
				var cowData = GetComponent<CowData>();
				Debug.Log(cowData.isEating);
				if (cowData.isEating)
					GetComponent<CowData>().StopEating();
				break;
			default:
				break;
		}

		_bubble.SetActive(true);
		_rigidbody.useGravity = false;
		_rigidbody.linearVelocity = Vector3.up * floatSpeed;
		State = ObjectState.Up;

		if (TryGetComponent<MoveableObject>(out var moveableObject))
		{
			moveableObject.enabled = false;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("FallPlane"))
		{
			_bubble.SetActive(false);
			_meshRenderer.material = _downMaterial;
			_rigidbody.useGravity = true;
			_rigidbody.linearVelocity = Vector3.zero;
			State = ObjectState.Down;
		}
	}

	// Destroy object in a destructive manner (i.e.: Explosive), if this object supports it
	public void DestroyObject()
	{
		switch (Type)
		{
			case Objects.ObjectType.Box:
				GameObject boxParticles = Instantiate(GameObject.FindGameObjectWithTag("FloorPlane").transform.Find("BoxExplosionParticle").gameObject);
				boxParticles.transform.position = transform.position;
				boxParticles.SetActive(true);
				Destroy(boxParticles, 5);
				break;
			default:
				break;
		}
		DespawnObject();
	}

	// Destroy the object in a quiet manner (disappears)
	public void DespawnObject()
	{
		Destroy(gameObject);
	}
}
