using System.Collections.Generic;
using UnityEngine;


public class TaskUiManager : MonoBehaviour
{
    [SerializeField] private GameObject TaskPanel;
    [SerializeField] private GameObject TaskPrefab;

    private List<TaskUiDictionary> _tasksUi = new List<TaskUiDictionary>();

    public static TaskUiManager Singleton { get; private set; }

    private void Awake()
    {
        Singleton = this;
    }

    public void CreateTaskUi(Task task)
    {
        var createdTask = Instantiate(TaskPrefab);
        createdTask.transform.SetParent(TaskPanel.transform);

        var taskUi = createdTask.GetComponent<TaskUi>();
        taskUi.SetText(task.Name, task.Description, task.Time);
        task.OnTimeChange += taskUi.OnTimeChange;

        _tasksUi.Add(new TaskUiDictionary(task, createdTask));
    }

    public void RemoveTaskUi(Task task)
    {
        for (int i = 0; i < _tasksUi.Count; i++)
        {
            var taskUi = _tasksUi[i];
            if (taskUi.Task == task)
            {
                Destroy(taskUi.Ui);
            }
        }
    }
}