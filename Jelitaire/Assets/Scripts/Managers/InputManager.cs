using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
	private PlayerInput playerInput;
	private InputAction touchPressAction;
	private InputAction touchPositionAction;
	private InputAction touchingAction;

	private CubeController movingCube;

	public event Action<CubeController> OnTouchStart;
	public event Action<Vector2> OnTouching;
	public event Action<CubeController> OnTouchEnd;

	protected override void Awake()
    {
		playerInput = GetComponent<PlayerInput>();
		touchPressAction = playerInput.actions["TouchPress"];
		touchPositionAction = playerInput.actions["TouchPosition"];
		touchingAction = playerInput.actions["Touching"];
	}

	private void OnEnable()
	{
		touchPressAction.performed += TouchPressed;
		touchPressAction.canceled += TouchReleased;
		touchingAction.performed += Touching;
	}

	private void OnDisable()
	{
		touchPressAction.performed -= TouchPressed;
		touchPressAction.performed -= TouchReleased;
		touchingAction.performed += Touching;
	}

	private void TouchPressed(InputAction.CallbackContext context)
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(touchPositionAction.ReadValue<Vector2>());
		Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2f);

		if (Physics.Raycast(ray, out hit))
		{
			if (hit.collider.CompareTag("Cube") && OnTouchStart != null)
			{
				movingCube = hit.collider.GetComponent<CubeController>();
				OnTouchStart.Invoke(movingCube);
			}
		}
	}

	private void Touching(InputAction.CallbackContext context)
	{
		Vector2 touchPos = touchPositionAction.ReadValue<Vector2>();
		Ray ray = Camera.main.ScreenPointToRay(touchPos);

		Plane plane = new Plane(Vector3.forward, new Vector3(0, 0, 49.5f));

		if (plane.Raycast(ray, out float enter))
		{
			Vector3 hitPoint = ray.GetPoint(enter);

			if (OnTouching != null)
			{
				movingCube.GetComponent<Collider>().enabled = false;

				hitPoint.x = Mathf.RoundToInt(hitPoint.x);
				hitPoint.x = Mathf.Clamp(hitPoint.x, -2, 2);
				
				OnTouching.Invoke(hitPoint);
			}
		}
	}

	private void TouchReleased(InputAction.CallbackContext context)
	{
		if (OnTouchEnd != null && movingCube != null)
		{
			movingCube.GetComponent<Collider>().enabled = true;
			OnTouchEnd.Invoke(movingCube);
			movingCube = null;
		}
	}
}
