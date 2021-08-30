using UnityEngine;
using System.Collections;


public class AnimatorBehaviour : MonoBehaviour
{
    [SerializeField] private float _durable = 2;

    private Animator _animator => GetComponent<Animator>();
    private bool _isDurableEnded;

    public void SetBool(string name)
    {
        _animator.SetBool(name, !_animator.GetBool(name));
    }

    public void SetBoolDurable(string name)
    {
        StartCoroutine(DurableBool(name));
    }

    public void SetDurable(float durable)
    {
        _durable = durable;
    }

    public bool IsDurableEnded()
    {
        return _isDurableEnded;
    }

    private IEnumerator DurableBool(string name)
    {
        _isDurableEnded = false;
        SetBool(name);
        yield return new WaitForSeconds(_durable);
        SetBool(name);
        _isDurableEnded = true;
        yield return null;
    }
}
