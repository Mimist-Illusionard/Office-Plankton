using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DurableTaskData", menuName = "Data/DurableTaskData")]
class DurableTaskData : ScriptableObject
{
    public List<DurableTask> DurableTasks;
}


[System.Serializable]
public class DurableTask
{
    public DurableType Type;
    public List<TaskInfo> CarryInfos;
}
