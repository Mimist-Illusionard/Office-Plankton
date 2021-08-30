using System;
using System.Collections.Generic;


public class RandomTask
{
    public Task CreateTask()
    {
        var task = new Task();
        var values = Enum.GetValues(typeof(TaskType));
        task.Type = (TaskType)UnityEngine.Random.Range(1, values.Length);

        InitializeTask(task);

        return task;
    }

    private void InitializeTask(Task task)
    {
        task.Time = UnityEngine.Random.Range(TaskManager.Singleton.GetMinimalTime(), TaskManager.Singleton.GetMaximalTime()) + TaskManager.Singleton.GetBonusTime();
        switch (task.Type)
        {
            case TaskType.None:
                break;
            case TaskType.Carry:
                CarryTaskInitializer(task);
                break;
            case TaskType.Immediately:
                WifiTaskInitializer(task);
                break;
            case TaskType.Durable:
                DurableTaskInitializer(task);
                break;
            default:
                break;
        }
    }

    #region Carry Task
    private void CarryTaskInitializer(Task task)
    {
        task.Time = UnityEngine.Random.Range(10, 25);
        RandomCarryTaskInitializer(task);     
    }

    private void RandomCarryTaskInitializer(Task task)
    {
        var values = Enum.GetValues(typeof(CarryType));
        var taskType = (CarryType)UnityEngine.Random.Range(1, values.Length);
        var carryTaskData = GameManager.Singleton.AssetsContext.GetScriptableObject("CarryTaskData") as CarryTaskData;
        var taskInfo = new TaskInfo();
        var taskObject = new UnityEngine.Object();

        for (int i = 0; i < carryTaskData.CarryTasks.Count; i++)
        {
            var carryTask = carryTaskData.CarryTasks[i];
            if (carryTask.Type == taskType)
            {
                taskInfo = carryTask.CarryInfos[UnityEngine.Random.Range(0, carryTask.CarryInfos.Count)];
                taskObject = carryTask.TaskObjects[UnityEngine.Random.Range(0, carryTask.TaskObjects.Count)];
            }
        }

        task.Name = taskInfo.Name;
        task.Description = taskInfo.Description;
        task.Sprite = GameManager.Singleton.AssetsContext.GetSprite(taskType.ToString());

        CarryTriggerInitializer(task, taskType, taskObject);
    }

    private void CarryTriggerInitializer(Task task, CarryType carryType, UnityEngine.Object taskPrefab)
    {
        var triggers = UnityEngine.Object.FindObjectsOfType<CarryTrigger>();
        var taskTriggers = new List<CarryTrigger>();

        for (int i = 0; i < triggers.Length; i++)
        {
            var trigger = triggers[i];

            for (int j = 0; j < trigger.Types.Count; j++)
            {
                var type = trigger.Types[j];
                if (type == carryType)
                    taskTriggers.Add(trigger);
            }
        }
        var randomTrigger = taskTriggers[UnityEngine.Random.Range(0, taskTriggers.Count)];
        if (randomTrigger._hasTask == false)
        {
            randomTrigger.InitiazleTask(task, carryType, taskPrefab);
            task.OnTaskStateChange += randomTrigger.TaskEnd;
        }
        else
            task = null;
    }
    #endregion

    private void WifiTaskInitializer(Task task)
    {
        task.Sprite = GameManager.Singleton.AssetsContext.GetSprite("WiFi");
        task.Name = "Restart WiFi!";
        task.Description = "Please help me! Restart WiFi!";
        task.Time = UnityEngine.Random.Range(10, 25);

        var wifi = UnityEngine.Object.FindObjectsOfType<WiFi>();
        var randomWifi = wifi[UnityEngine.Random.Range(0, wifi.Length)];

        if (randomWifi.HasTask == false)
        {
            randomWifi.SetTask(task);
            task.OnTaskStateChange += randomWifi.TaskEnd;

            var waypoint = WaypointManager.Singleton.CreateWaypoint(randomWifi.transform);
            waypoint.ChangeInfo(task.Sprite, task.Type.ToString());

            task.OnTaskStateChange += waypoint.Destroy;
        }
        else
            task = null;
    }

    #region Durable Task
    private void DurableTaskInitializer(Task task)
    {
        task.Time = UnityEngine.Random.Range(10, 25);
        RandomDurableTaskInitializer(task);        
    }

    private void RandomDurableTaskInitializer(Task task)
    {
        var values = Enum.GetValues(typeof(DurableType));
        var taskType = (DurableType)UnityEngine.Random.Range(1, values.Length);
        var carryTaskData = GameManager.Singleton.AssetsContext.GetScriptableObject("DurableTaskData") as DurableTaskData;
        var taskInfo = new TaskInfo();

        for (int i = 0; i < carryTaskData.DurableTasks.Count; i++)
        {
            var durableTask = carryTaskData.DurableTasks[i];
            if (durableTask.Type == taskType)
            {
                taskInfo = durableTask.CarryInfos[UnityEngine.Random.Range(0, durableTask.CarryInfos.Count)];
            }
        }

        task.Name = taskInfo.Name;
        task.Description = taskInfo.Description;
        task.Sprite = GameManager.Singleton.AssetsContext.GetSprite(taskType.ToString());

        FindDurableTask(task, taskType);
    }

    private void FindDurableTask(Task task, DurableType durableType)
    {
        var tasks = UnityEngine.Object.FindObjectsOfType<Durable>();
        var durableTasks = new List<Durable>();

        var triggers = UnityEngine.Object.FindObjectsOfType<CarryTrigger>();
        var taskTriggers = new List<CarryTrigger>();

        for (int i = 0; i < tasks.Length; i++)
        {
            var durableTask = tasks[i];
            if (durableType == durableTask.Type)
            {
                durableTasks.Add(durableTask);
            }         
        }

        var randomDurable = durableTasks[UnityEngine.Random.Range(0, durableTasks.Count)];
        if (randomDurable.HasTask == false)
        {
            randomDurable.SetTask(task);
            task.OnTaskStateChange += randomDurable.TaskEnd;

            var waypoint = WaypointManager.Singleton.CreateWaypoint(randomDurable.transform);
            waypoint.ChangeInfo(task.Sprite, task.Type.ToString());

            task.OnTaskStateChange += waypoint.Destroy;
        }
        else
            task = null;
    }
    #endregion
}
