using UnityEngine;


public class PlayerGrabbingLogic : MonoBehaviour, IExecute
{
	public Transform _offset;

	[SerializeField] private float _grabPower = 10f;
	[SerializeField] private float _throwPower = 1f;
	[SerializeField] private float _rayDistance = 3;

	private RaycastHit hit;

	private bool _isGrabbing = false;
	private bool _isThrowing = false;

	private void Start()
	{
		_offset = gameObject.GetComponentInChildren<HolderView>().transform;
		GameManager.Singleton.SetNewExecuteObject(this);
	}

	public void Execute()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Physics.Raycast(transform.position, transform.forward, out hit, _rayDistance);

			if (hit.rigidbody)
			{
				_isGrabbing = true;
			}
		}

		if (Input.GetMouseButtonUp(0))
		{
			if (_isGrabbing)
			{
				_isGrabbing = false;
			}
		}

		if (Input.GetMouseButtonUp(1))
		{
			if (_isGrabbing)
			{
				_isGrabbing = false;
				_isThrowing = true;
			}
		}

		if (_isGrabbing)
		{
			if (hit.rigidbody)
			{
				hit.rigidbody.velocity = (_offset.position - (hit.transform.position + hit.rigidbody.centerOfMass)) * _grabPower;
			}
		}

		if (_isThrowing)
		{
			if (hit.rigidbody)
			{
				hit.rigidbody.velocity = transform.forward * _throwPower;
				_isThrowing = false;
			}
		}
	}
}
