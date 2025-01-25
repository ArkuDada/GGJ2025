using System;
using Unity.AI.Navigation;
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
        _agent.SetDestination( _surface.GetRandomGrid().center);
    }

    // Update is called once per frame
    void Update()
    {
        var direction = _agent.destination;
        direction.y = transform.position.y;
        transform.LookAt(direction);
        if((!_agent.pathPending && _agent.remainingDistance < 0.1f) || _agent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            _agent.SetDestination( _surface.GetRandomGrid().center);
        }
    }
}
