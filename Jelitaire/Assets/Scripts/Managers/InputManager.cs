using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
	// InputSystem 액션
	private PlayerInput playerInput;
	private InputAction touchPressAction;
	private InputAction touchPositionAction;
	private InputAction touchingAction;

	private CubeController movingCube; // 현재 움직이고 있는 큐브

	// 큐브들이 구독할 Input 이벤트
	public event Action<CubeController> OnTouchStart;
	public event Action<Vector2> OnTouching;
	public event Action OnTouchEnd;

	protected override void Awake()
    {
		base.Awake();

		// 액션 불러오기
		playerInput = GetComponent<PlayerInput>();
		touchPressAction = playerInput.actions["TouchPress"];
		touchPositionAction = playerInput.actions["TouchPosition"];
		touchingAction = playerInput.actions["Touching"];
	}

	private void OnEnable()
	{
		// 각 액션에 클릭 이벤트 함수 구독
		touchPressAction.performed += TouchPressed;
		touchPressAction.canceled += TouchReleased;
		touchingAction.performed += Touching;
	}

	private void OnDisable()
	{
		// 각 액션에 클릭 이벤트 함수 해제
		touchPressAction.performed -= TouchPressed;
		touchPressAction.performed -= TouchReleased;
		touchingAction.performed -= Touching;
	}

	// 클릭 혹은 터치 최초 입력
	private void TouchPressed(InputAction.CallbackContext context)
	{
		// 레이캐스팅으로 큐브를 선택
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(touchPositionAction.ReadValue<Vector2>());
		Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2f);

		if (Physics.Raycast(ray, out hit))
		{
			if (hit.collider.CompareTag("Cube") && OnTouchStart != null)
			{
				movingCube = hit.collider.GetComponent<CubeController>();
				OnTouchStart.Invoke(movingCube); // 구독한 큐브 객체들에게 모두 Broadcasting (식별을 위한 movingCube 인자)
			}
		}
	}

	// 클릭 혹은 터치 진행중
	private void Touching(InputAction.CallbackContext context)
	{
		if (movingCube != null)
		{
			// xy평면에 레이캐스팅하여 정확한 포인터의 위치를 알아냄
			Vector2 touchPos = touchPositionAction.ReadValue<Vector2>();
			Ray ray = Camera.main.ScreenPointToRay(touchPos);

			Plane plane = new Plane(Vector3.forward, new Vector3(0, 0, 49.5f));

			if (plane.Raycast(ray, out float enter))
			{
				Vector3 hitPoint = ray.GetPoint(enter);

				if (OnTouching != null)
				{
					OnTouching.Invoke(hitPoint); // 구독한 큐브 객체들에게 모두 Broadcasting (포인터 좌표 전달)
				}
			}
		}
	}

	// 클릭 혹은 터치 종료
	private void TouchReleased(InputAction.CallbackContext context)
	{
		if (OnTouchEnd != null && movingCube != null)
		{
			OnTouchEnd.Invoke(); // 구독한 큐브 객체들에게 모두 Broadcasting (포인터 좌표 전달)
			movingCube = null;
		}
	}
}
