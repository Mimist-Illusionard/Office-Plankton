﻿using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    private List<IExecute> _executes = new List<IExecute>();
    public static GameManager Singleton { get; private set; }
    public AssetsContext AssetsContext;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        Application.targetFrameRate = 100;
    }

    private void Update()
    {
        for (var i = 0; i < _executes.Count; i++)
        {
            var updatable = _executes[i];
            updatable.Execute();
        }
    }

    public void SetNewExecuteObject(IExecute executeGameObject)
    {
        _executes.Add(executeGameObject);
    }

    public void RemoveExecuteObject(IExecute executeGameObject)
    {
        _executes.Remove(executeGameObject);
    } 

    public void ClearAllExeciteObjects()
    {
        _executes.Clear();
    }
}
