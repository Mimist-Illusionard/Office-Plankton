using UnityEngine;
using UnityEngine.Events;

public class Durable : MonoBehaviour, IInteractable
{
    private Task _currentTask;

    [SerializeField] private float _useTime;
    private float _currentUseTime;

    public UnityEvent OnTaskInitialize;
    public UnityEvent OnInteractStart;
    public UnityEvent OnInteract;
    public UnityEvent OnInteractEnd;
    public UnityEvent OnTaskEnd;

    public DurableType Type;
    [HideInInspector] public bool HasTask;

    public void StartLogic()
    {
        if (_currentTask == null) return;

        _currentTask.FreezeTime(true);
        OnInteractStart?.Invoke();
    }

    public void Logic()
    {
        if (_currentTask == null) return;

        _currentUseTime -= Time.deltaTime;
        if (_currentUseTime <= 0)
        {
            _currentTask.Success();
            _currentTask = null;
            HasTask = false;
        }

        OnInteract?.Invoke();
    }

    public void ReleaseLogic()
    {
        if (_currentTask == null) return;

        _currentTask.FreezeTime(false);
        OnInteractEnd?.Invoke();
    }

    public void SetTask(Task task)
    {
        OnTaskInitialize?.Invoke();

        _currentTask = task;
        _currentUseTime = _useTime;
        HasTask = true;
    }  

    public void TaskEnd()
    {
        OnTaskEnd?.Invoke();
        _currentTask = null;
        HasTask = false;
    }
}