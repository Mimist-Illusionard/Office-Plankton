using UnityEngine;
using TMPro;


public class TaskUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private TextMeshProUGUI Description;
    [SerializeField] private TextMeshProUGUI Time;

    public void SetText(string name, string description, float time)
    {
        Name.text = name;
        Description.text = description;
        Time.text = time.ToString();
    }

    public void OnTimeChange(float value)
    {
        Time.text = ((float)(int)(value * 1) / 1).ToString();
    }
}
