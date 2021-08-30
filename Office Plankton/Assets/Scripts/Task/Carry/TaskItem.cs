using System;
using System.Collections.Generic;
using UnityEngine;


public class TaskItem : MonoBehaviour, IInteractable
{
    [SerializeField] private List<Waypoint> _waypoints = new List<Waypoint>();

    private CarryTrigger _taskTrigger;
    private Task _task;
    private bool _isItemPickedFirstTime;

    public Action OnItemTake;
    public Action OnItemRelease;

    public void Logic()
    {
    }

    public void ReleaseLogic()
    {
        CreateWaypoint();
        OnItemRelease?.Invoke();

        _taskTrigger.DestroyWaypoints();
    }

    public void StartLogic()
    {
        OnItemTake?.Invoke();

        if (!_isItemPickedFirstTime)
        {
            _task.Time += 5f;
            _isItemPickedFirstTime = true;
        }

        for (int i = 0; i < _waypoints.Count; i++)
        {
            var waypoint = _waypoints[i];
            OnItemTake -= waypoint.Destroy;
        }

        _taskTrigger.CreateWaypoint();
    }

    public void InitializeItem(Task task, CarryTrigger taskTrigger)
    {
        _task = task;
        _taskTrigger = taskTrigger;
        _isItemPickedFirstTime = false;
        CreateWaypoint();
    }

    private void CreateWaypoint()
    {
        var waypoint = WaypointManager.Singleton.CreateWaypoint(this.transform);
        waypoint.ChangeInfo(_task.Sprite, _task.Type.ToString());
        _waypoints.Add(waypoint);

        OnItemTake += waypoint.Destroy;
    }

    public void DestroyTaskItem()
    {
        for (int i = 0; i < _waypoints.Count; i++)
        {
            var waypoint = _waypoints[i];
            if (waypoint == null) continue;

            waypoint.Destroy();
        }

        Destroy(this.gameObject);
    }
}
