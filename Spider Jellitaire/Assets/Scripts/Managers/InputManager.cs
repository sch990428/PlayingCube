using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
	[SerializeField]
	private GameObject JellyObject;

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
				targetJelly = hit.collider.GetComponent<Jelly>();
				targetJelly.isMoving = true;
				GameManager.Instance.anyJellyMoving = true;
			}
		}
	}

	private void TouchReleased(InputAction.CallbackContext context)
	{
		if (targetJelly != null)
		{
			targetJelly.isMoving = false;
			GameManager.Instance.anyJellyMoving = false;
			targetJelly.UpdatePos();
			targetJelly = null;
		}
	}

	private void Touching(InputAction.CallbackContext context)
	{
		Vector3 touchPos = Camera.main.ScreenToWorldPoint(touchPositionAction.ReadValue<Vector2>());
		touchPos.y = 0;

		if (targetJelly != null)
		{
			targetJelly.transform.position = touchPos;
		}
	}
}
