using NCC.Utility.Objects;
using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MoveableObject : MonoBehaviour
{
	private GridFloor _surface;
	NavMeshAgent _agent;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	private void Awake()
	{
		_agent = GetComponent<NavMeshAgent>();
		_surface = GameObject.FindGameObjectWithTag("FloorPlane").GetComponent<GridFloor>();
	}

	void Start()
	{
		//Get Random Point on surface 
		_agent.SetDestination(_surface.GetRandomGrid().center);
	}

	// Update is called once per frame
	void Update()
	{
		var direction = _agent.destination;
		direction.y = transform.position.y;
		if ((!_agent.pathPending && _agent.remainingDistance < 0.1f) || _agent.pathStatus == NavMeshPathStatus.PathInvalid)
		{
			var growingObjects = FindObjectsByType<GrowingObject>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
			bool foundWheat = false;

			// Find wheat
			foreach (var growingObject in growingObjects)
			{
				if (growingObject.Type == Utility.Objects.ObjectType.Wheat)
				{
					foundWheat = true;
					_agent.SetDestination(growingObject.transform.position);
					break;
				}
			}

			// Move randomly if there's no wheat
			if (!foundWheat)
				_agent.SetDestination(_surface.GetRandomGrid().center);
		}
	}

	private void OnEnable()
	{
		_agent.enabled = true;
	}

	private void OnDisable()
	{
		_agent.enabled = false;
	}
}
