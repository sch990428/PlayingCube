using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	[SerializeField]
	private GameObject Board;

	public enum GameState
	{
		Init,
		Game,
		Exit,
		Hide
	}

	private int lootCount = 5;

	public GameState State;

	protected override void Awake()
    {
		base.Awake();
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
		yield return new WaitForSeconds(0.5f);
		Board.transform.GetComponent<Animator>().SetTrigger("Exit");
		yield return new WaitForSeconds(0.5f);
		Board.SetActive(false);
	}

	private IEnumerator AddNewCubes()
	{
		yield return new WaitForSeconds(0.5f);

		// 각 라인에 새로운 큐브들을 추가
		for (int i = 0; i < lootCount; i++)
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
			

			// 라인의 Root 위치에 자식 오브젝트가 이미 있으면 제일 막내로 넣음
			Transform lineRoot = Board.transform.GetChild(i);
			if (lineRoot.childCount != 0)
			{
				// LinkedList와 유사하게 Child와 Parent 형태로 연결되어있는 Cube 객체를 순회
				CubeController current = lineRoot.GetChild(0).GetComponent<CubeController>(); // Root의 자식 큐브
				while (current.Child != null)
				{
					current = current.Child;
				}

				// current가 계층 구조의 막내 큐브를 가리키면 여기에 Cube를 연결
				newCube.transform.SetParent(current.transform);
				current.Child = newCubeController;
				newCubeController.Parent = current;
			}
			else
			{
				newCube.transform.SetParent(lineRoot);
			}
		}
	}

	private IEnumerator RemoveAllCubes()
	{
		// TODO : 삭제 로직 개선
		for (int i = 0; i < lootCount; i++)
		{
			Transform lineRoot = Board.transform.GetChild(i);
			lineRoot.gameObject.GetComponent<Collider>().enabled = false;
		}

		yield return new WaitForSeconds(1f);

		for (int i = 0; i < lootCount; i++)
		{
			Transform lineRoot = Board.transform.GetChild(i);
			lineRoot.gameObject.GetComponent<Collider>().enabled = true;
			ResourceManager.Instance.Destroy(lineRoot.GetChild(0).gameObject);
		}
	}
}
