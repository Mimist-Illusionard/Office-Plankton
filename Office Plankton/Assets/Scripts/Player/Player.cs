using UnityEngine;


public class Player : MonoBehaviour, IExecute
{
	[SerializeField] Transform _cameraHolder;
	[SerializeField] private float _mouseSensitivity, _walkSpeed, _sprintSpeed, _jumpForce, _smoothTime, _gravityForce;
	[SerializeField] private float _staminaRegen;
	[SerializeField] private float _staminaSpent;

	private CharacterController _characterController;

	private Vector3 velocity;
	private RaycastHit hit;

	private float _playerSpeed;

	private float verticalLookRotation;
	private float _previousWalkSpeed;
	private float _previousSprintSpeed;

	public bool _canRun = true;

	private void Awake()
	{
		_characterController = GetComponent<CharacterController>();
	}

	private void Start()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		SwitchCursorMode();

		_playerSpeed = _walkSpeed;

		GameManager.Singleton.SetNewExecuteObject(this);
		PlayerManager.Singleton.SetPlayer(this);
	}

	public void Execute()
	{
		if (gameObject.GetComponent<Player>().enabled != false)
		{
			RaycastLogic();
			Look();
			Sprint();
			Move();
			GameGravity();
		}
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		float pushPower = 2;
		Rigidbody body = hit.collider.attachedRigidbody;
		Vector3 force;

		if (body == null || body.isKinematic) { return; }
		force = hit.controller.velocity * pushPower;
		body.AddForceAtPosition(force, hit.point);

	}

	private void Look()
	{
		transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * _mouseSensitivity);

		verticalLookRotation += Input.GetAxisRaw("Mouse Y") * _mouseSensitivity;
		verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

		_cameraHolder.localEulerAngles = Vector3.left * verticalLookRotation;
	}

	private void Move()
	{
		float x = Input.GetAxis("Horizontal");
		float z = Input.GetAxis("Vertical");

		Vector3 move = gameObject.transform.right * x + gameObject.transform.forward * z;
		_characterController.Move(move * _playerSpeed * Time.deltaTime);
	}

	private void Sprint()
	{
		if (_canRun)
		{
			if (Input.GetKey(KeyCode.LeftShift))
			{
				PlayerManager.Singleton.StaminaManager(_staminaSpent);
				_playerSpeed = _sprintSpeed;
			}
			else
			{
				PlayerManager.Singleton.StaminaManager(_staminaRegen);
				_playerSpeed = _walkSpeed;
			}
		}
		else
		{
			PlayerManager.Singleton.StaminaManager(_staminaRegen);
			_playerSpeed = _walkSpeed;
		}
	}

	private void GameGravity()
	{
		velocity.y = _gravityForce;
		_characterController.Move(velocity * Time.deltaTime);
	}

	public void SwitchCursorMode()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}
	public void OpenCursor()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.Confined;
	}

	private void RaycastLogic()
    {
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 3);

			if (hit.collider == null) return;

			if (hit.collider.GetComponent<IInteractable>() != null)
			{
				var interactableObject = hit.collider.GetComponent<IInteractable>();
				interactableObject.StartLogic();
			}
		}

		if (Input.GetKey(KeyCode.Mouse0))
		{
			Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 3);

			if (hit.collider == null) return;

			if (hit.collider.GetComponent<IInteractable>() != null)
			{
				var interactableObject = hit.collider.GetComponent<IInteractable>();
				interactableObject.Logic();
			}
		}

		if (Input.GetKeyUp(KeyCode.Mouse0))
		{
			Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 3);

			if (hit.collider == null) return;

			if (hit.collider.GetComponent<IInteractable>() != null)
			{
				var interactableObject = hit.collider.GetComponent<IInteractable>();
				interactableObject.ReleaseLogic();
			}
		}
	}

	public void AddSpeedModifier(float modifier)
    {
			_previousWalkSpeed = _walkSpeed;
			_previousSprintSpeed = _sprintSpeed;

			_walkSpeed += _walkSpeed * modifier / 100;
			_sprintSpeed += _sprintSpeed * modifier / 100;
    }

	public void RemoveSpeedModifier(float modifier)
    {
		_walkSpeed = _walkSpeed - _previousWalkSpeed * modifier / 100;
		_sprintSpeed = _sprintSpeed - _previousSprintSpeed * modifier / 100;
	}
}