using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	[SerializeField]
	private GameObject Board; // 큐브를 올릴 받침대 오브젝트 그룹 (Root 접근에 필요)

	[SerializeField]
	public Color32[] CubeColors; // 큐브 타입에 따른 색상표

	// 게임 진행 State 패턴
	public enum GameState 
	{
		Init,
		Game,
		Exit,
		Hide
	}

	public GameState State;

	private int rootCount = 5; // 큐브를 올릴 수 있는 라인(Root)의 개수
	public List<CubeController>[] Cubes; // 모든 큐브의 정보를 담고있는 List의 배열

	protected override void Awake()
    {
		base.Awake();

		// Cubes 및 게임 상태 초기화
		Cubes = new List<CubeController>[rootCount];
		for (int i = 0; i < rootCount; i++)
		{
			Cubes[i] = new List<CubeController>();
		}

		State = GameState.Hide;

		Application.targetFrameRate = 120; // 게임의 프레임을 선언
	}

	private void Update()
	{
		switch (State)
		{
			case GameState.Init:
				StartCoroutine(InitBoard());
				AddNewCubes();
				State = GameState.Game;
				break;

			case GameState.Exit:
				StartCoroutine(ClearBoard());
				State = GameState.Hide;
				break;

			case GameState.Game:

				break;
		}
	}

	// 큐브 새 줄 추가 버튼 이벤트
	public void OnAddButtonClicked()
	{
		StartCoroutine(AddNewCubes());
	}

	private IEnumerator InitBoard()
	{
		// 게임 시작를 위한 애니메이션과 로직 실행
		Board.SetActive(true);
		Board.transform.GetComponent<Animator>().SetTrigger("Start");
		yield return AddNewCubes();
		yield return AddNewCubes();
	}

	private IEnumerator ClearBoard()
	{
		// 게임 종료를 위한 애니메이션과 로직 실행
		yield return RemoveAllCubes();
		yield return new WaitForSeconds(0.2f);
		Board.transform.GetComponent<Animator>().SetTrigger("Exit");
		yield return new WaitForSeconds(0.4f);
		Board.SetActive(false);
	}

	// 새 큐브 줄 추가 로직
	private IEnumerator AddNewCubes()
	{
		yield return new WaitForSeconds(0.5f);

		// 각 라인에 새로운 큐브들을 추가
		for (int i = 0; i < rootCount; i++)
		{
			GameObject newCube = ResourceManager.Instance.Instantiate("Prefabs/Cube");
			CubeController newCubeController = newCube.GetComponent<CubeController>();

			// 1번과 3번 라인은 z가 1 낮음
			float z = 50f;
			if (i == 1 || i == 3)
			{
				z = 49f;
			}

			// 새로 만든 큐브의 초기 위치를 지정
			newCube.transform.position = new Vector3(i - 2, -12.625f + 0.75f * (Cubes[i].Count), z);

			// TODO : 새로 만든 큐브의 값들을 지정(타입, 숫자 등)
			newCubeController.SetValue(Random.Range(1, 6), Random.Range(0, 3));

			// 라인의 Root 위치에 자식 오브젝트가 이미 있으면 제일 막내로 넣음
			int CubeCountInLine = Cubes[i].Count;
			if (CubeCountInLine > 0)
			{
				Cubes[i][CubeCountInLine - 1].Child = newCubeController;
				newCubeController.Parent = Cubes[i][CubeCountInLine - 1];
			}

			Cubes[i].Add(newCubeController);

			newCube.transform.SetParent(Board.transform);
		}
	}

	// 모든 큐브를 삭제
	private IEnumerator RemoveAllCubes()
	{
		yield return null;
		for (int i = 0; i < rootCount; i++)
		{
			if (Cubes[i].Count > 0)
			{
				Cubes[i][0].DestroyCube();
			}

			Cubes[i] = new List<CubeController>(); // Cubes도 비워줌
		}
	}

	// 해당 Root에 올려진 모든 큐브의 정보를 업데이트
	public void UpdateLine(int i)
	{
		// Board의 Root 오브젝트에서 레이캐스팅하여 최초 큐브를 찾음
		// Child로 링크되어 있으므로 링크드리스트처럼 순회 접근
		Transform root = Board.transform.GetChild(i);
		RaycastHit hit;
		CubeController current;
		Cubes[i] = new List<CubeController>();
		if (Physics.Raycast(root.transform.position, Vector3.up, out hit))
		{
			if (hit.collider.CompareTag("Cube") && hit.collider.gameObject.name.Equals("Cube"))
			{
				current = hit.collider.transform.GetComponent<CubeController>();

				while (current != null)
				{
					Cubes[i].Add(current);
					current = current.Child;
				}
			}
		}
	}
}
