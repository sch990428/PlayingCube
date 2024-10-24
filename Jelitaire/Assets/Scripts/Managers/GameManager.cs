using System.Collections;
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

	public GameState State = GameState.Exit;
	public bool isGameStart = false;

	protected override void Awake()
    {
		base.Awake();
		Application.targetFrameRate = 120; // 게임의 프레임을 선언
	}

	private void Update()
	{
		switch (State)
		{
			case GameState.Init:
				StartCoroutine(Init());
				AddNewCubes();
				State = GameState.Game;
				break;

			case GameState.Exit:
				StartCoroutine(Clear());
				State = GameState.Hide;
				break;
			case GameState.Game:

				break;
		}
	}

	private IEnumerator Init()
	{
		// 게임 시작를 위한 애니메이션과 로직 실행
		Board.SetActive(true);
		Board.transform.GetComponent<Animator>().SetTrigger("Start");
		yield return AddNewCubes();
		yield return AddNewCubes();
	}

	private IEnumerator Clear()
	{
		// 게임 종료를 위한 애니메이션과 로직 실행
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

			// 1번과 3번 라인은 z가 1 낮음
			float z = 50f;
			if (i == 1 || i == 3)
			{
				z = 49f;
			}

			// 새로 만든 큐브의 초기 위치를 지정
			newCube.transform.position = new Vector3(i - 2, -5, z);

			// 라인의 Root 위치에 자식 오브젝트가 이미 있으면 제일 막내로 넣어야함
			Transform lineRoot = Board.transform.GetChild(i);
			if (lineRoot.childCount != 0)
			{
				// TODO : LinkedList와 유사하게 Child와 Parent 형태로 연결되어있는 Cube 객체를 순회
				Debug.Log("Already has Child");
			}
			else
			{
				newCube.transform.SetParent(lineRoot);
			}
			
		}
	}
}
