using TMPro;

using UnityEngine;
using UnityEngine.UI;


public class Waypoint : MonoBehaviour, IExecute
{
    [SerializeField] private GameObject _waypoint;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _taskName;
    [SerializeField] private TextMeshProUGUI _distanceText;
    [SerializeField] private float _closeDistance;

    private Player _player => PlayerManager.Singleton.GetPlayer();
    private Transform _target;
    private float _distance;

    private void Awake()
    {
        GameManager.Singleton.SetNewExecuteObject(this);
    }

    public void Execute()
    {
        if (_target == null) return;

        GetDistance();
        CheckOnScreen();
    }  

    private void GetDistance()
    {
        _distance = Vector3.Distance(_player.transform.position, _target.position);
        _distanceText.text = _distance.ToString("f1") + "m";

        if (_distance <= _closeDistance)
        {
            SetActive(false);
        }
    }

    private void CheckOnScreen()
    {
        var thing = Vector3.Dot((_target.position - Camera.main.transform.position).normalized, Camera.main.transform.forward);

        if (thing <= 0)
        {
            SetActive(false);
        }
        else if(_distance > _closeDistance)
        {
            SetActive(true);
            transform.position = Camera.main.WorldToScreenPoint(_target.position);
        }
    }

    private void SetActive(bool value)
    {
        _waypoint.SetActive(value);
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void Destroy()
    {
        if (gameObject == null) return;

        Destroy(this.gameObject);
        GameManager.Singleton.RemoveExecuteObject(this);
    }

    public void ChangeInfo(Sprite image, string taskName)
    {
        _taskName.text = taskName;
        _image.sprite = image;
    }
}
