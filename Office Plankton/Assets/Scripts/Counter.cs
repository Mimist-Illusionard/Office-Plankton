using TMPro;
using UnityEngine;


public class Counter : MonoBehaviour
{
    public TextMeshProUGUI Text;
    public string TextMessage;

    public void OnValueChange(int value)
    {
        Text.text = $"{TextMessage} {value}";
    }
}

