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
	private bool isMoving; 
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
		InputManager.Instance.OnTouchStart -= TouchStart;
		InputManager.Instance.OnTouching -= Touching;
		InputManager.Instance.OnTouchEnd -= TouchReleased;
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
			prevPos = transform.position; // 이전 좌표 저장

			// 기존 부모와의 연결 끊기
			if (Parent != null)
			{
				Parent.Child = null;
				Parent = null;
			}

			isMoving = true;
		}
	}

	// 해당 큐브가 선택되는 도중에 수행할 동작
	private void Touching(Vector2 pos)
	{
		if (isMoving)
		{
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
			// 레이캐스팅을 통해 현재 큐브의 바닥에 다른 큐브나 루트가 있는지 확인
			RaycastHit hit;
			isMoving = false;

			int prevX = Mathf.RoundToInt(prevPos.x); // 이전 x좌표
			int currentX = Mathf.RoundToInt(transform.position.x); // 현재 x좌표

			Debug.DrawRay(transform.position, Vector3.down * 10f, Color.red, 4f);
			if (Physics.Raycast(transform.position, Vector3.down, out hit))
			{
				if (hit.collider.CompareTag("Cube")) // 큐브면 
				{
					if (hit.collider.gameObject.name.Equals("Cube"))
					{
						UpdatePositionWithChild(hit.collider.transform.position + new Vector3(0, 0.75f, 0));
						CubeController newParent = hit.collider.transform.GetComponent<CubeController>();
						newParent.Child = this;
						Parent = newParent;
					}
					else // 큐브가 아니면 (Root)
					{				
						UpdatePositionWithChild(new Vector3(transform.position.x, -12.625f ,transform.position.z));
						Parent = null;
					}
					
					// GameManager Cubes 갱신
					GameManager.Instance.UpdateLine(prevX + 2);
					GameManager.Instance.UpdateLine(currentX + 2);

					return;
				}
			}

			// 잘못된 위치에 둔 경우 다시 원위치
			UpdatePositionWithChild(prevPos);
			GameManager.Instance.UpdateLine(prevX + 2);
		}
	}
}
