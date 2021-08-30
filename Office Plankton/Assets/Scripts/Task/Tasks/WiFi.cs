using UnityEngine;
using UnityEngine.Events;

public class WiFi : MonoBehaviour, IInteractable
{
    private Task _currentTask;
    [HideInInspector] public bool HasTask;

    public UnityEvent OnTaskInitialize;
    public UnityEvent OnTaskSuccess;
    public UnityEvent OnTaskEnd;

    public UnityEvent OnInteractStart;
    public UnityEvent OnInteract;
    public UnityEvent OnInteractEnd;

    public void Logic()
    {
        OnInteract?.Invoke();
        if (HasTask)
        {
            _currentTask.Success();
            HasTask = false;
            OnTaskSuccess?.Invoke();
        }
    }

    public void ReleaseLogic()
    {
        OnInteractEnd?.Invoke();
    }

    public void SetTask(Task task)
    {
        _currentTask = task;
        HasTask = true;

        OnTaskInitialize?.Invoke();
    }

    public void StartLogic()
    {
        OnInteractStart?.Invoke();
    }

    public void TaskEnd()
    {
        OnTaskEnd?.Invoke();
        _currentTask = null;
        HasTask = false;
    }
}
