using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
	[SerializeField]
	private GameObject JellyObject;

	public event Action<Jelly> OnTouchPressed;
	public event Action<Vector3> OnTouching;
	public event Action<Vector3> OnTouchReleased;

	private PlayerInput playerInput;
	private InputAction touchPositionAction;
	private InputAction touchPressAction;
	private InputAction touchingAction;

	Jelly targetJelly;

	private void Awake()
	{
		playerInput = GetComponent<PlayerInput>();
		touchPressAction = playerInput.actions["TouchPressed"];
		touchPositionAction = playerInput.actions["TouchPosition"];
		touchingAction = playerInput.actions["Touching"];
	}

	private void OnEnable()
	{
		touchingAction.performed += Touching;
		touchPressAction.performed += TouchPressed;
		touchPressAction.canceled += TouchReleased;
	}

	private void OnDisable()
	{
		touchingAction.performed -= Touching;
		touchPressAction.performed -= TouchPressed;
		touchPressAction.canceled -= TouchReleased;
	}

	private void TouchPressed(InputAction.CallbackContext context)
	{
		Vector3 touchPos = Camera.main.ScreenToWorldPoint(touchPositionAction.ReadValue<Vector2>());
		touchPos.y = 0;

		// Debug.Log(touchPos);

		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(touchPositionAction.ReadValue<Vector2>());
		Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2f);

		if (Physics.Raycast(ray, out hit))
		{
			if (hit.collider.CompareTag("JellyEntity"))
			{
				OnTouchPressed.Invoke(hit.collider.gameObject.GetComponent<Jelly>());
			}
		}
	}

	private void TouchReleased(InputAction.CallbackContext context)
	{
		Vector3 touchPos = Camera.main.ScreenToWorldPoint(touchPositionAction.ReadValue<Vector2>());
		touchPos.y = 0;

		OnTouchReleased.Invoke(touchPos);
	}

	private void Touching(InputAction.CallbackContext context)
	{
		Vector3 touchPos = Camera.main.ScreenToWorldPoint(touchPositionAction.ReadValue<Vector2>());
		touchPos.y = 0;
		OnTouching.Invoke(touchPos);
	}
}
