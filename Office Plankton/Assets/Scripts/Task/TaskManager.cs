using System.Collections.Generic;
using UnityEngine;


public class TaskManager : MonoBehaviour, IExecute
{
    [Header("Random Task Settings")]
    [SerializeField] private float _randomTaskTime;
    [SerializeField] private float _randomTaskIntervalModifier;
    [SerializeField] private float _randomTaskTimeConstant;

    [Header("Task Modifier Settings")]
    [SerializeField] private float _addTaskTimeModifier;
    [SerializeField] private float _minimalTaskTime;
    [SerializeField] private float _maximalTaskTime;
    [SerializeField] private float _taskTimeConstant;

    private RandomTask _randomTask = new RandomTask();
    private List<Task> _tasks;
    private float _currentNewTaskTime;
    private float _bonusTaskTime;

    public static TaskManager Singleton { get; private set; }

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        _currentNewTaskTime = _randomTaskTime;
        _tasks = new List<Task>();

        GameManager.Singleton.SetNewExecuteObject(this);
        TimerManager.Singleton.OnIntervalChange += ChangeTaskAppearTime;
        TimerManager.Singleton.OnIntervalChange += ChangeTaskTime;
    }

    public void Execute()
    {
        NewTaskTimeLogic();
        TasksLogic();
    }

    public void AddTask(Task task)
    {
        _tasks.Add(task);
        TaskUiManager.Singleton.CreateTaskUi(task);
    }

    public void RemoveTask(Task task)
    {
        _tasks.Remove(task);
        TaskUiManager.Singleton.RemoveTaskUi(task);
    }

    private void NewTaskTimeLogic()
    {
        _currentNewTaskTime -= Time.deltaTime;
        if(_currentNewTaskTime <= 0)
        {
            _currentNewTaskTime = _randomTaskTime;

            var randomTask = _randomTask.CreateTask();

            if (randomTask != null)
                AddTask(randomTask);
        }
    }

    private void TasksLogic()
    {
        for (int i = 0; i < _tasks.Count; i++)
        {
            var task = _tasks[i];
            task.Logic();
        }
    }

    public void ChangeTaskAppearTime()
    {
        _randomTaskTime -= _randomTaskIntervalModifier;
        if (_randomTaskTime <= _randomTaskTimeConstant)
        {
            _randomTaskTime += _randomTaskIntervalModifier;
            TimerManager.Singleton.OnIntervalChange -= ChangeTaskAppearTime;
        }
    }

    public void ChangeTaskTime()
    {
        _maximalTaskTime += _addTaskTimeModifier;
        _minimalTaskTime += _addTaskTimeModifier;

        if (_maximalTaskTime >= _randomTaskTimeConstant)
        {
            TimerManager.Singleton.OnIntervalChange -= ChangeTaskTime;
        }
    }

    public void AddBonusTime(float bonusTime)
    {
        _bonusTaskTime += bonusTime;
    }

    public float GetBonusTime()
    {
        return _bonusTaskTime;
    }

    public float GetMinimalTime()
    {
        return _minimalTaskTime;
    }
    public float GetMaximalTime()
    {
        return _maximalTaskTime;
    }
}
