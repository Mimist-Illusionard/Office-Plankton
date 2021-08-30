using UnityEngine;

//Again one of bad scripts
public class RotateToPointBehaviour : MonoBehaviour, IExecute
{
    public Transform Target;

    public bool _isRotateToTarget = false;
    [HideInInspector] public bool DisableRotation;

    private void Start()
    {
        if (_isRotateToTarget)
            GameManager.Singleton.SetNewExecuteObject(this);
    }

    public void Execute()
    {
        if (_isRotateToTarget)
        {
            transform.LookAt(Target);
        }
    }

    public void RotateToPoint(Vector3 point)
    {
        if (DisableRotation != true)
        {
            transform.LookAt(point);
        }
    }
}
