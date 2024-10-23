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

	protected override void Awake()
	{
		base.Awake();

		playerInput = GetComponent<PlayerInput>();
		if (playerInput == null)
		{
			playerInput = gameObject.AddComponent<PlayerInput>();
			playerInput.actions = ResourceManager.Instance.Load<InputActionAsset>("PlayerActions");
		}

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
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(touchPositionAction.ReadValue<Vector2>());
		// Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2f);

		if (Physics.Raycast(ray, out hit))
		{
			if (hit.collider.CompareTag("JellyEntity"))
			{
				if (OnTouchPressed != null)
				{
					OnTouchPressed.Invoke(hit.collider.gameObject.GetComponent<Jelly>());
				}
			}
		}
	}

	private void TouchReleased(InputAction.CallbackContext context)
	{
		Vector3 touchPos = Camera.main.ScreenToWorldPoint(touchPositionAction.ReadValue<Vector2>());
		touchPos.y = 0;

		if (OnTouchReleased != null)
		{
			OnTouchReleased.Invoke(touchPos);
		}
	}

	private void Touching(InputAction.CallbackContext context)
	{
		Vector2 touchPosition = touchPositionAction.ReadValue<Vector2>();
		Ray ray = Camera.main.ScreenPointToRay(touchPosition);

		// y = 0.5 평면을 정의해 이곳으로 레이캐스팅을 한 좌표를 얻어온다
		Plane plane = new Plane(Vector3.up, new Vector3(0, 0, 0));

		if (plane.Raycast(ray, out float enter))
		{
			Vector3 hitPoint = ray.GetPoint(enter);

			if (OnTouching != null)
			{
				OnTouching.Invoke(hitPoint);
			}
		}
	}
}
