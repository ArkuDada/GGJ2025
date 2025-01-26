using NCC.Utility.Objects;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class MoveableObject : MonoBehaviour
{
	public GridFloor _surface;
	NavMeshAgent _agent;
	public BaseObject _base;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	private void Awake()
	{
		_agent = GetComponent<NavMeshAgent>();
		_surface = GameObject.FindGameObjectWithTag("FloorPlane").GetComponent<GridFloor>();
	}

	void Start()
	{
		_base = GetComponent<BaseObject>();
		ChooseNewDestination();
	}

	private Vector3 RandomDestination()
	{
		return _surface.GetRandomGrid().center;
	}

	public void ChooseNewDestination()
	{
		if (!_agent.isOnNavMesh || _base.State != BaseObject.ObjectState.Grounded)
			return;

		Vector3 destination;

		switch (_base.Type)
		{
			case Utility.Objects.ObjectType.Cow:
				// Find wheat
				var allWheat = FindObjectsByType<WheatData>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
				bool foundWheat = false;
				WheatData closestWheatData = null;
				Vector3 closestWheatPos = Vector3.zero;
				float closestWheat = float.MaxValue;

				foreach (var wheat in allWheat)
				{
					if (!wheat.beingEaten)
					{
						Vector3 wheatPos = wheat.transform.position;
						float dist = Vector3.Distance(wheatPos, transform.position);
						if (dist < closestWheat)
						{
							foundWheat = true;
							closestWheatPos = wheatPos;
							closestWheatData = wheat.GetComponent<WheatData>();
							closestWheat = dist;
						}
					}
				}

				CowData cowData = GetComponent<CowData>();

				if (foundWheat)
				{
					cowData.targetWheat = closestWheatData;
					destination = closestWheatPos;
				}
				else
				{
					cowData.targetWheat = null;
					destination = RandomDestination();
				}
				break;
			default:
				destination = RandomDestination();
				break;
		}

		_agent.SetDestination(destination);
	}

	void Update()
	{
		var direction = _agent.destination;
		direction.y = transform.position.y;
		Vector3 destination = _agent.destination;
		destination.y = transform.position.y;
		float remainingDistance = Vector3.Distance(destination, transform.position);
		if ((!_agent.pathPending && remainingDistance < 0.3f) || _agent.pathStatus == NavMeshPathStatus.PathInvalid)
		{
			bool chooseNewDestination = true;

			if (_base.Type == Utility.Objects.ObjectType.Cow && GetComponent<CowData>().isEating)
				chooseNewDestination = false;

			if (chooseNewDestination)
				ChooseNewDestination();
		}
	}

	private void OnEnable()
	{
		GetComponent<Rigidbody>().isKinematic = true;
		_agent.enabled = true;
	}

	private void OnDisable()
	{
		GetComponent<Rigidbody>().isKinematic = false;
		_agent.enabled = false;
	}
}
