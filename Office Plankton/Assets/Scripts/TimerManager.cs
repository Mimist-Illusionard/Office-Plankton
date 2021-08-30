using System;
using UnityEngine;


public class TimerManager : MonoBehaviour, IExecute
{
    [SerializeField] private Counter _timeCounter;

    private int _interval;
    private int _minutes;
    private int _step = 1;

    private float _passedTime;
    private int _intPassedTime;

    public Action<int> OnTimeChange;
    public Action OnIntervalChange;

    public static TimerManager Singleton { get; private set; }

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        GameManager.Singleton.SetNewExecuteObject(this);
        OnTimeChange += _timeCounter.OnValueChange;
    }

    public void Execute()
    {
        _passedTime += Time.deltaTime;
        if (_passedTime / 60f / _step >= 1)
        {
            _minutes += 1;
            _step += 1;

            OnIntervalChange?.Invoke();
        }

        _intPassedTime = (int)(_passedTime * 1) / 1;
        OnTimeChange?.Invoke(_intPassedTime);
    }

    public float GetTime()
    {
        return _passedTime;
    }
}
