using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Agent : MonoBehaviour, IExecute
{
    [SerializeField] private List<Transform> _waypoints = new List<Transform>();
    [SerializeField] private int _currentWaypointIndex;

    private NavMeshAgent _navMeshAgent => GetComponent<NavMeshAgent>();

    private void Start()
    {
        _navMeshAgent.SetDestination(_waypoints[0].position);
        GameManager.Singleton.SetNewExecuteObject(this);
    }

    public void Execute()
    {
        AgentLogic();
    }

    private void AgentLogic()
    {
        if (_navMeshAgent.remainingDistance == _navMeshAgent.stoppingDistance)
        {
            _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Count;
            _navMeshAgent.SetDestination(_waypoints[_currentWaypointIndex].position);
        }
    }

    public void AddWaypoint(Transform waypoint)
    {
        _waypoints.Add(waypoint);
    }

    public void ClearWaypoints()
    {
        _waypoints.Clear();
    }

    public void SetDestination(Vector3 point)
    {
        _navMeshAgent.SetDestination(point);
    }

    public bool IsLastPoint()
    {
        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance && _currentWaypointIndex == _waypoints.Count - 1)
        {
            return true;
        }
        else
            return false;
    }
}