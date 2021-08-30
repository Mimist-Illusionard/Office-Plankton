using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CarryTaskData", menuName = "Data/CarryTaskData")]
public class CarryTaskData : ScriptableObject
{
    public List<CarryTask> CarryTasks;
}

[System.Serializable]
public class CarryTask
{
    public CarryType Type;
    public List<GameObject> TaskObjects;
    public List<TaskInfo> CarryInfos;
}

[System.Serializable]
public class TaskInfo
{
    public string Name;
    [Multiline()]
    public string Description;
}
