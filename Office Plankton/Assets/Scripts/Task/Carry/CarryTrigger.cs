using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CarryTrigger : MonoBehaviour, ICarryObject
{
    private Task _currentTask;
    private List<Waypoint> _waypoints = new List<Waypoint>();
    private GameObject _taskObject;
    private TaskItem _taskItem;

    public List<CarryType> Types;
    public List<CarryType> Type => Types;

    [HideInInspector] public bool _hasTask;

    public UnityEvent OnTaskInitialize;
    public UnityEvent OnTaskSuccess;
    public UnityEvent OnTaskEnd;

    private void OnTriggerEnter(Collider other)
    {
        if (_taskObject == null) return;

        if (other.gameObject == _taskObject)
        {
            _currentTask.Success();
            _hasTask = false;
            Destroy(_taskObject);
            OnTaskSuccess?.Invoke();
        }
    }

    public void InitiazleTask(Task task, CarryType carryType, UnityEngine.Object taskPrefab)
    {
        _hasTask = true;
        _currentTask = task;

        var spawnPoints = UnityEngine.Object.FindObjectsOfType<TaskItemSpawnPoint>();
        var taskSpawnPoints = new List<TaskItemSpawnPoint>();

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            var spawnPoint = spawnPoints[i];

            for (int j = 0; j < spawnPoint.PlaceTaskTypes.Count; j++)
            {
                var type = spawnPoint.PlaceTaskTypes[j];
                if (type == carryType)
                {
                    taskSpawnPoints.Add(spawnPoint);
                }
            }
        }
        var randomSpawnPoint = taskSpawnPoints[UnityEngine.Random.Range(0, taskSpawnPoints.Count)];

        _taskObject = GameObject.Instantiate(taskPrefab as GameObject, randomSpawnPoint.transform);
        _taskItem = _taskObject.GetComponent<TaskItem>();
        _taskItem.InitializeItem(task, this);

        task.OnTaskStateChange += _taskItem.DestroyTaskItem;
        task.OnTaskStateChange += DestroyWaypoints;

        OnTaskInitialize?.Invoke();
    }

    public void CreateWaypoint()
    {
        var waypoint = WaypointManager.Singleton.CreateWaypoint(this.transform);
        waypoint.ChangeInfo(GameManager.Singleton.AssetsContext.GetSprite($"{_currentTask.Sprite.name}_2"), _currentTask.Type.ToString());
        _waypoints.Add(waypoint);
    }

    public void DestroyWaypoints()
    {
        for (int i = 0; i < _waypoints.Count; i++)
        {
            var waypoint = _waypoints[i];
            if (waypoint == null) continue;

            waypoint.Destroy();
        }
    }

    public void TaskEnd()
    {
        OnTaskEnd?.Invoke();
        _hasTask = false;
        _currentTask = null;
    }
}