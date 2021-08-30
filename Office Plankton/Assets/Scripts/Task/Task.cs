using System;
using UnityEngine;


[System.Serializable]
public class Task
{
    public Sprite Sprite;
    public string Name;
    public string Description;
    public float Time;

    public TaskType Type;
    public bool _isComplete;

    public Action<float> OnTimeChange;
    public Action OnTaskStateChange;

    private bool _isTimeFreeze;

    public virtual void Logic()
    {
        if (_isTimeFreeze == true) return;

        Time -= UnityEngine.Time.deltaTime;
        OnTimeChange?.Invoke(Time);

        if (Time <= 0)
        {
            Failed();
        }
    }

    public virtual void Success()
    {
        TaskManager.Singleton.RemoveTask(this);
        RatingManager.Singleton.TaskSucceed();
        OnTaskStateChange?.Invoke();
    }

    public virtual void Failed()
    {
        TaskManager.Singleton.RemoveTask(this);
        RatingManager.Singleton.TaskFailed();
        OnTaskStateChange?.Invoke();
    }

    public void FreezeTime(bool value)
    {
        _isTimeFreeze = value;      
    }
}
