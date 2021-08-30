using UnityEngine;

public class TaskUiDictionary
{
    public Task Task;
    public GameObject Ui;

    public TaskUiDictionary(Task task, GameObject ui)
    {
        Task = task;
        Ui = ui;
    }
}
