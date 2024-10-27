using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class CubeController : MonoBehaviour
{
	// 큐브 개체 정보
	[SerializeField]
	private TMP_Text NumberText; // 숫자를 표시할 TextMeshPro 오브젝트

    public int Number; // 숫자
    public int Type; // 타입

	private Animator animator;
	private Renderer render;

	// 큐브 계층 정보
	public CubeController Parent; // 상위의 큐브 혹은 null(루트)
	public CubeController Child; // 하위의 큐브 혹은 null

	// 큐브 이동 관련 정보
	public bool isMoving; 
	private Vector3 prevPos;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		render = GetComponent<Renderer>();

		isMoving = false;
	}

	private void OnEnable()
	{
		// InputManager 구독
		InputManager.Instance.OnTouchStart += TouchStart;
		InputManager.Instance.OnTouching += Touching;
		InputManager.Instance.OnTouchEnd += TouchReleased;
	}

	private void OnDisable()
	{
		// InputManager 구독 해제
		if (InputManager.hasInstance())
		{
			InputManager.Instance.OnTouchStart -= TouchStart;
			InputManager.Instance.OnTouching -= Touching;
			InputManager.Instance.OnTouchEnd -= TouchReleased;
		}
	}

	// 큐브 생성 혹은 아이템 사용으로 인한 큐브 정보 변경
	public void SetValue(int number, int type)
	{
		Number = number;
		Type = type;

		NumberText.text = Number.ToString();
		render.material.color = GameManager.Instance.CubeColors[Type]; // 타입에 따른 색상 변경
	}

	// 큐브 삭제 로직
	public void DestroyCube()
	{
		if (Child != null)
		{
			Child.DestroyCube(); // Child 재귀
		}

		animator.SetTrigger("Destroy");

		ResourceManager.Instance.Destroy(gameObject, 0.35f);
	}

	// 해당 큐브가 선택되면 수행할 동작
	private void TouchStart(CubeController target)
	{
		if (target == this)
		{
			// TODO : 현재 뜯긴 큐브들의 숫자가 정렬되어있는가?

			prevPos = transform.position; // 이전 좌표 저장

			// 기존 부모와의 연결 끊기
			if (Parent != null)
			{
				Parent.Child = null;
				Parent = null;
			}

			int targetX = Mathf.RoundToInt(transform.position.x);
			isMoving = true;
			GameManager.Instance.UpdateLine(targetX + 2); // 현재 큐브 배치 업데이트
		}
	}

	// 해당 큐브가 선택되는 도중에 수행할 동작
	private void Touching(Vector2 pos)
	{
		if (isMoving)
		{
			pos.x = Mathf.RoundToInt(pos.x); // 소수점 반올림을 통해 좌표 변경이 1단위로 이루어지도록 함
			pos.x = Mathf.Clamp(pos.x, -2, 2); // x 축은 [-2, 2] 범위에서 움직임

			// y 축은 [현재 라인 최상단 큐브 혹은 Root 위, 2] 범위에서 움직임
			CubeController topCube = GameManager.Instance.GetTopCube((int)(pos.x) + 2);

			float yLimit = -12.675f;

			if (topCube != null)
			{
				yLimit = topCube.transform.position.y + 0.75f;
			}
			pos.y = Mathf.Clamp(pos.y, yLimit, 2);

			// x좌표 1씩 마다 지그재그로 z좌표가 변경
			float z;
			if (pos.x == -1 || pos.x == 1)
			{
				z = 49f;
			}
			else
			{
				z = 50f;
			}

			UpdatePositionWithChild(new Vector3(pos.x, pos.y, z)); 
		}
	}

	// 모든 Child와 함께 이동
	private void UpdatePositionWithChild(Vector3 pos)
	{
        if (Child != null)
        {
			Child.UpdatePositionWithChild(pos + new Vector3(0, 0.75f, 0)); // Child 재귀 (큐브의 높이인 0.75씩 올려서 함께 이동)
        }
		transform.position = pos;
    }

	// 해당 큐브가 놓아질 때 수행할 동작
	private void TouchReleased()
	{
		if (isMoving)
		{
			isMoving = false;

			int prevX = Mathf.RoundToInt(prevPos.x); // 이전 x좌표
			int currentX = Mathf.RoundToInt(transform.position.x); // 현재 x좌표

			// GameManager에서 탑 큐브를 찾아옴
			CubeController topCube = GameManager.Instance.GetTopCube(currentX + 2);

			// TODO : 현재 붙이려는 큐브와 붙혀지는 큐브의 숫자가 정렬되어있는가?

			if (topCube != null)
			{
				//Debug.Log($"큐브{topCube.Number}");
				UpdatePositionWithChild(topCube.transform.position + new Vector3(0, 0.75f, 0));
				topCube.Child = this;
				Parent = topCube;
			}
			else
			{
				//Debug.Log("루트");
				UpdatePositionWithChild(new Vector3(transform.position.x, -12.625f, transform.position.z));
				Parent = null;
			}

			GameManager.Instance.UpdateLine(currentX + 2);

			// 잘못된 위치에 둔 경우 다시 원위치
			//UpdatePositionWithChild(prevPos);
			//GameManager.Instance.UpdateLine(prevX + 2);
		}
	}
}
