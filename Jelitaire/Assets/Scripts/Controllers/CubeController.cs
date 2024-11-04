using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Cube
{
	public int Number; // 숫자
	public int Type; // 타입

	public Cube(int number, int type)
	{
		Number = number;
		Type = type;
	}
}

public class CubeController : MonoBehaviour
{
	// 큐브 개체 정보
	[SerializeField]
	private TMP_Text NumberText; // 숫자를 표시할 TextMeshPro 오브젝트
	[SerializeField]
	private GameObject PopEffect; // 터지는 파티클 이펙트

	public int Number; // 숫자
    public int Type; // 타입

	private Animator animator;
	private Renderer render;

	// 큐브 계층 정보
	public CubeController Parent; // 상위의 큐브 혹은 null(루트)
	public CubeController Child; // 하위의 큐브 혹은 null
	public bool isOverweight = false; // 큐브가 과적된 상태인가?
	public int childCount = 0;

	// 큐브 이동 관련 정보
	public bool isMoving; 
	public bool isDestroying;
	private Vector3 prevPos;
	private CubeController prevParent;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		render = GetComponent<Renderer>();

		render.material = GameManager.Instance.material;

		isMoving = false;
		isDestroying = false;
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

	private void Update()
	{
		// 과적 상태에 따라 깜빡임 연출
		if (isOverweight)
		{
			float t = Mathf.PingPong(Time.time * 1f, 0.75f);
			render.material.color = Color.Lerp(GameManager.Instance.CubeColors[Type], Color.black, t);
		}
		else
		{
			render.material.color = GameManager.Instance.CubeColors[Type];
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

	// 큐브 강제 삭제 로직
	public void DestroyCube()
	{
		if (Child != null)
		{
			Child.DestroyCube(); // Child 재귀
		}

		ResourceManager.Instance.Destroy(gameObject);
	}

	// 해당 큐브가 선택되면 수행할 동작
	private void TouchStart(CubeController target)
	{
		if (target == this && !isDestroying && !GameManager.Instance.isGenerating)
		{
			// 현재 뜯길 큐브들의 숫자가 정렬되지않은 상태인가?
			if (!IsSequential())
			{
				return;
			}

			prevPos = transform.position; // 이전 좌표 저장
			prevParent = Parent; // 이전 부모 저장
			// 기존 부모와의 연결 끊기
			if (Parent != null)
			{
				Parent.Child = null;
				Parent = null;
			}

			// 현재 들려있는 자식의 수 세기
			childCount = 0;
			CubeController current = this;
			while (current.Child != null)
			{
				childCount++;
				current = current.Child;
			}
			// Debug.Log($"들고있는 자식 {childCount + 1}");

			int targetX = Mathf.RoundToInt(transform.position.x);
			isMoving = true;
			GameManager.Instance.UpdateLine(targetX + 2); // 현재 큐브 배치 업데이트
			SoundManager.Instance.PlaySound(SoundManager.GameSound.Click);
		}
	}

	// 자식 큐브들이 순차적으로 정렬되어 있는지 재귀 확인
	private bool IsSequential()
	{
		if (Child == null)
		{
			return true;
		}

		// 1씩 작은 순서로, 같은 타입끼리 정렬되어 있는가?
		return ( Child.IsSequential() && (Child.Number == Number - 1) && (Child.Type == Type) );
	}

	// 부모 큐브들이 순차적으로 정렬되어 있는지 재귀 확인
	public bool IsSequentialReverse()
	{
		if (Number == 5) // 현재 숫자가 5인 경우 true 반환
		{
			return true;
		}

		if (Parent == null) // 5가 아니었는데 부모가 없으면 순서가 끊긴 것으로 간주하고 false 반환
		{
			return false;
		}

		// 1씩 큰 순서로, 같은 타입끼리 정렬되어 있는가?
		return (Parent.IsSequentialReverse() && (Parent.Number == Number + 1) && (Parent.Type == Type));
	}

	// 올바른 순서로 연결된 큐브를 자식부터 순차적으로 삭제하는 로직
	public void Pop()
	{
		if (isDestroying)
		{
			return;
		}

		isDestroying = true;

		if (Number == 5) // 내 숫자가 5인 경우 
		{
			if (Parent != null) // 부모가 있을 때 부모와의 연결을 끊음
			{
				Parent.Child = null;
			}
		}
		else
		{
			Parent.Pop(); // Parent 재귀
		}

		DrawPopEffect();
		animator.SetTrigger("Destroy");
		ResourceManager.Instance.Destroy(gameObject, 0.35f);
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

			float yLimit = GameManager.Instance.rootY + 2.375f;

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
		if (Parent == null) // 자신이 최하단 부모라면
		{
			// Debug.Log($"{GameManager.Instance.Cubes[(int)(pos.x) + 2].Count} + {childCount}");
			// 들려있는 큐브의 수와 하단에 쌓인 큐브의 수가 합쳐졌을때 과적상태인가?
			if (GameManager.Instance.Cubes[(int)(pos.x) + 2].Count + childCount + 1 > 7) 
			{
				isOverweight = true;
			}
			else
			{
				isOverweight = false;
			}
		}
		else
		{
			isOverweight = Parent.isOverweight; // 들려있는 자식은 무조건 부모의 과적상태를 따름
		}
		
        if (Child != null)
        {
			Child.UpdatePositionWithChild(pos + new Vector3(0, 0.75f, 0)); // Child 재귀 (큐브의 높이인 0.75씩 올려서 함께 이동)
        }
		transform.position = pos;

		Physics.SyncTransforms(); // 물리엔진에 즉시 동기화
	}

	// 해당 큐브가 놓아질 때 수행할 동작
	private void TouchReleased()
	{
		if (isMoving)
		{
			isMoving = false;
			childCount = 0;

			int prevX = Mathf.RoundToInt(prevPos.x); // 이전 x좌표
			int currentX = Mathf.RoundToInt(transform.position.x); // 현재 x좌표

			// GameManager에서 탑 큐브를 찾아옴
			CubeController topCube = GameManager.Instance.GetTopCube(currentX + 2);

			if (topCube != null)
			{
				// 현재 붙이려는 큐브와 붙혀지는 부모 큐브의 숫자가 정렬되어있는가 ?
				if (topCube.Number == Number + 1)
				{
					//Debug.Log($"큐브{topCube.Number}");
					UpdatePositionWithChild(topCube.transform.position + new Vector3(0, 0.75f, 0));
					DrawDropEffect();
					topCube.Child = this;
					Parent = topCube;
					GameManager.Instance.UpdateLine(currentX + 2);
					SoundManager.Instance.PlaySound(SoundManager.GameSound.Drop);
					GameManager.Instance.DropRightCube();
					return;
				}
			}
			else
			{
				//Debug.Log("루트");
				UpdatePositionWithChild(new Vector3(transform.position.x, GameManager.Instance.rootY + 2.375f, transform.position.z));
				DrawDropEffect();
				Parent = null;
				GameManager.Instance.UpdateLine(currentX + 2);
				SoundManager.Instance.PlaySound(SoundManager.GameSound.Drop);
				GameManager.Instance.DropRightCube();
				return;
			}

			// 잘못된 위치에 둔 경우 다시 원위치
			UpdatePositionWithChild(prevPos);
			DrawDropEffect();

			// 원래 부모가 없는 Root 였으면 제외
			if (prevParent != null)
			{
				prevParent.Child = this;
				Parent = prevParent;
			}
			//Debug.Log(prevX + 2 + "번 루트 갱신");
			GameManager.Instance.UpdateLine(prevX + 2);
			GameManager.Instance.DropWrongCube();
		}
	}

	private void DrawDropEffect()
	{
		// 드래그 드랍 이펙트 표시
		GameObject go = ResourceManager.Instance.Instantiate("Prefabs/Effect");
		go.transform.position = transform.position + new Vector3(0, -0.375f, 0);
		go.transform.SetParent(transform);
		ResourceManager.Instance.Destroy(go, 0.2f);
	}

	private void DrawPopEffect()
	{
		// 팝 이펙트 표시
		GameObject go = Instantiate(PopEffect, transform.position, transform.rotation);
		var particleSystem = go.GetComponent<ParticleSystem>();

		Color32 defaultColor = GameManager.Instance.CubeColors[Type];

		var mainModule = particleSystem.main;
		mainModule.startColor = new ParticleSystem.MinMaxGradient(defaultColor);
		var colorOverLifetime = particleSystem.colorOverLifetime;
		Gradient gradient = new Gradient();
		
		gradient.SetKeys(
			new GradientColorKey[] { new GradientColorKey(defaultColor, 0f), new GradientColorKey(defaultColor, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, 1.0f) }
			);
		colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);

		ResourceManager.Instance.Destroy(go, 1f);
	}
}
