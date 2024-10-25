using NUnit.Framework;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	[SerializeField]
	private GameObject Board;

	[SerializeField]
	public Color32[] CubeColors;

	public enum GameState
	{
		Init,
		Game,
		Exit,
		Hide
	}

	private int rootCount = 5;
	private CubeController[] roots;

	public GameState State;

	protected override void Awake()
    {
		base.Awake();
		roots = new CubeController[rootCount];
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
			newCube.transform.position = new Vector3(i - 2, -5, z);

			// TODO : 새로 만든 큐브의 값들을 지정(타입, 숫자 등)
			newCubeController.SetValue(Random.Range(1, 6), Random.Range(0, 3));

			// 라인의 Root 위치에 자식 오브젝트가 이미 있으면 제일 막내로 넣음
			if (roots[i] != null)
			{
				// LinkedList와 유사하게 Child와 Parent 형태로 연결되어있는 Cube 객체를 순회
				CubeController current = roots[i].GetComponent<CubeController>(); // Root의 자식 큐브
				while (current.Child != null)
				{
					current = current.Child;
				}

				// current가 계층 구조의 막내 큐브를 가리키면 여기에 Cube를 연결
				current.Child = newCubeController;
				newCubeController.Parent = current;
			}
			else
			{
				roots[i] = newCubeController;
			}

			newCube.transform.SetParent(Board.transform);
		}
	}

	private IEnumerator RemoveAllCubes()
	{
		yield return null;
		for (int i = 0; i < rootCount; i++)
		{
			roots[i].DestroyCube();
		}
	}
}
