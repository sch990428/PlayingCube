using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
	[SerializeField]
	private GameObject Board; // 큐브를 올릴 받침대 오브젝트 그룹 (Root 접근에 필요)

	[SerializeField]
	public List<Color32> CubeColors; // 큐브 타입에 따른 색상표

	[SerializeField]
	private UIController uiController; // UI컨트롤러

	[SerializeField]
	private ModeController modeController; // 모드 컨트롤러

	[SerializeField]
	private GameObject TimerUI; // 타이머 UI

	[SerializeField]
	private GameObject GameOverUI; // 게임오버 창

	[SerializeField]
	private TMP_Text RewardText; // 보상값 텍스트

	// 게임 진행 State 패턴
	public enum GameState
	{
		Init,
		Game,
		Exit,
		Wait,
		GameOver,
	}

	public GameState State;
	public Difficulty GameDifficulty; // 게임 난이도 전략 (생성로직)

	private int rootCount = 5; // 큐브를 올릴 수 있는 라인(Root)의 개수
	public List<CubeController>[] Cubes; // 모든 큐브의 정보를 담고있는 List의 배열

	public bool isGenerating; // 큐브를 새로 만들고 있는 중인가?

	public int Score; // 게임 점수
	public int Combo;
	public event Action OnScoreChanged;

	public float rootY;

	// 타임어택 관련
	public bool isTimeAttack; // 타임어택 모드인가?
	public float timer; // 현재 타이머
	public float timerInterval; // 타이머 만료 시간

	// 장착한 스킨 마테리얼
	public Material material;

	protected override void Awake()
    {
		base.Awake();

		// Cubes 및 게임 상태 초기화
		Cubes = new List<CubeController>[rootCount];
		for (int i = 0; i < rootCount; i++)
		{
			Cubes[i] = new List<CubeController>();
		}

		State = GameState.Wait;

		Application.targetFrameRate = 120; // 게임의 프레임을 선언

		isGenerating = false;
	}

	public void ShuffleColor()
	{
		System.Random rng = new System.Random();

		for (int n = CubeColors.Count - 1; n > 0; n--)
		{
			int k = rng.Next(n + 1);
			(CubeColors[n], CubeColors[k]) = (CubeColors[k], CubeColors[n]);
		}
	}

	// ModeController의 선택값에 따라 난이도 전략 설정
	private Difficulty SetGameDifficulty()
	{
		switch (modeController.Selected)
		{
			case 1:
				return new StarfishEasy();
			case 2:
				return new StarfishNormal();
			case 3:
				return new StarfishHard();
			default:
				return null;
		}
	}

	private void Update()
	{
		switch (State)
		{
			case GameState.Init:
				ShuffleColor();
				GameDifficulty = SetGameDifficulty();
				GameDifficulty.InitQueue();
				StartCoroutine(InitBoard());
				State = GameState.Game;
				break;
			case GameState.Exit:
				StartCoroutine(ClearBoard());
				State = GameState.Wait;
				break;
			case GameState.GameOver:
				GameOverUI.SetActive(true);
				float rewardRatio = modeController.GetMode().RewardRatio;
				if (!isTimeAttack) { rewardRatio = 0; }
				RewardText.text = ((int)(Score * rewardRatio)).ToString();
				StartCoroutine(GameOver());
				State = GameState.Wait;
				break;
			case GameState.Game:
				if (isTimeAttack) // 타임어택이면 타이머관련 로직 처리
				{
					timer += Time.deltaTime;

					if (timer > timerInterval)
					{
						OnAddButtonClicked();
					}
				}
				break;
		}
	}

	// 큐브 새 줄 추가 버튼 이벤트
	public void OnAddButtonClicked()
	{
		if (!isGenerating && State == GameState.Game)
		{
			InputManager.Instance.TouchCancel();
			ResetTimer();
			StartCoroutine(AddNewCubes());
		}
	}

	private IEnumerator InitBoard()
	{
		// 게임 시작를 위한 애니메이션과 로직 실행
		Score = 0;
		uiController.ComboUpdate(1);
		Combo = 1;
		timer = 0f;
		timerInterval = 20f;
		OnScoreChanged.Invoke();
		Board.SetActive(true);
		TimerUI.SetActive(isTimeAttack);
		Board.transform.GetComponent<Animator>().SetTrigger("Start");
		yield return new WaitForSeconds(0.2f);
		SoundManager.Instance.PlaySound(SoundManager.GameSound.Init);
		yield return new WaitForSeconds(0.3f);
		rootY = Board.transform.GetChild(0).position.y;
		yield return AddNewCubes();
		yield return AddNewCubes();
	}

	private IEnumerator ClearBoard()
	{
		// 게임 종료를 위한 애니메이션과 로직 실행
		Board.transform.GetComponent<Animator>().SetTrigger("Exit");
		yield return new WaitForSeconds(0.2f);
		yield return RemoveAllCubes();
		yield return new WaitForSeconds(0.4f);
		Board.SetActive(false);
	}

	// 새 큐브 줄 추가 로직
	private IEnumerator AddNewCubes()
	{
		isGenerating = true;
		yield return new WaitForSeconds(0.3f);
		SoundManager.Instance.PlaySound(SoundManager.GameSound.AddLine);
		if (GameDifficulty.CubeQueue.Count <= 0)
		{
			// Debug.Log(GameDifficulty.CubeQueue.Count);
			GameDifficulty.InitQueue(); // 새로운 큐브 대기열 추가
			// Debug.Log(GameDifficulty.CubeQueue.Count);
		}

		// 각 라인에 새로운 큐브들을 추가
		for (int i = 0; i < rootCount; i++)
		{
			yield return new WaitForSeconds(0.03f);
			GameObject newCube = ResourceManager.Instance.Instantiate("Prefabs/Cube");
			CubeController newCubeController = newCube.GetComponent<CubeController>();

			// 1번과 3번 라인은 z가 1 낮음
			float z = 50f;
			if (i == 1 || i == 3)
			{
				z = 49f;
			}
			
			// 새로 만든 큐브의 초기 위치를 지정
			newCube.transform.position = new Vector3(i - 2, rootY + 2.375f + 0.75f * (Cubes[i].Count), z);
			Physics.SyncTransforms(); // 물리엔진에 즉시 동기화

			// TODO : 새로 만든 큐브의 값들을 지정(타입, 숫자 등)
			Cube cube = GameDifficulty.CubeQueue.Dequeue();
			newCubeController.SetValue(cube.Number, cube.Type);

			// 라인의 Root 위치에 자식 오브젝트가 이미 있으면 제일 막내로 넣음
			int CubeCountInLine = Cubes[i].Count;
			if (CubeCountInLine > 0)
			{
				Cubes[i][CubeCountInLine - 1].Child = newCubeController;
				newCubeController.Parent = Cubes[i][CubeCountInLine - 1];
			}

			Cubes[i].Add(newCubeController);

			newCube.transform.SetParent(Board.transform);

			UpdateLine(i);
		}

		isGenerating = false;
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
				
				if (!current.isMoving) // 이동중인 큐브는 레이캐스팅에 감지되면 안됨 (들어올리는 순간)
				{
					while (current != null)
					{
						if (!current.isDestroying) //삭제중인 큐브는 레이캐스팅에 감지되면 안됨
						{
							Cubes[i].Add(current);
							current = current.Child;
						}
					}
				}
			}
		}

		// 업데이트 된 라인의 최상단이 1이라면 Pop을 시도
		CubeController topCube = GetTopCube(i);
		if (topCube != null && topCube.Number == 1)
		{
			TryPop(topCube, i);
		}

		// 업데이트 된 라인에 쌓인 큐브의 갯수에 따라 위험도 결정
		int count = Cubes[i].Count;

		if (Cubes[i].Count > 7)
		{
			// 만일 해당 라인에 쌓인 큐브가 7개를 넘으면 게임오버 상태로 전환
			if (State == GameState.Game)
			{
				State = GameState.GameOver;
			}
		}
		else if (Cubes[i].Count == 7)
		{
			// 만일 해당 라인에 쌓인 큐브가 7개면 과적 상태로 전환
			foreach (CubeController c in Cubes[i])
			{
				c.isOverweight = true;
			}
		}
		else
		{
			// 평상시엔 과적 상태 해제
			foreach (CubeController c in Cubes[i])
			{
				c.isOverweight = false;
			}
		}

		//디버깅용 코드
		//string str = "";
		//foreach (CubeController c in Cubes[i])
		//{ str += c.Number.ToString() + " "; }
		//Debug.Log($"{i}번 루트 : {str}");
	}

	public bool CheckAllEmpty()
	{
		bool result = true;
		for (int i = 0; i < rootCount; i++)
		{
			if (Cubes[i].Count != 0)
			{
				result = false;
			}
		}

		return result;
	}

	public void DropRightCube()
	{
		Camera.main.GetComponent<CameraController>().BackgroundEffect(Color.white);
	}

	public void DropWrongCube()
	{
		Combo = 1;
		uiController.ComboUpdate(1);
		SoundManager.Instance.PlaySound(SoundManager.GameSound.Wrong);
		Camera.main.GetComponent<CameraController>().OnShakeCameraByRotation();
		Camera.main.GetComponent<CameraController>().BackgroundEffect(Color.red);
		AddScore(-1);
	}

	public IEnumerator GameOver()
	{
		yield return new WaitForSeconds(0.2f);
		SoundManager.Instance.PlaySound(SoundManager.GameSound.GameOver);

		// 순간적으로 RigidBody에 랜덤한 힘을 주어 큐브를 날려버림
		for (int k = 0; k < rootCount; k++)
		{
			foreach (CubeController c in Cubes[k])
			{
				Rigidbody rigidbody = c.transform.GetComponent<Rigidbody>();
				rigidbody.isKinematic = false;
				rigidbody.useGravity = true;

				Vector3 randomDirection = new Vector3(
					UnityEngine.Random.Range(-1f, 1f),
					UnityEngine.Random.Range(0f, 1f),
					UnityEngine.Random.Range(-1f, 1f)
				).normalized;

				rigidbody.AddForce(randomDirection * 50f, ForceMode.Impulse);
			}
		}
		yield return new WaitForSeconds(0.5f);
		yield return RemoveAllCubes(); // 모든 큐브 오브젝트 제거
		yield return new WaitForSeconds(1f);
		uiController.GameOverUI.onClick.AddListener(() => uiController.OnLobbyButtonClicked(true));
	}

	public void TryPop(CubeController topCube, int i)
	{
		if (topCube.IsSequentialReverse())
		{
			int topCubeIndex = Cubes[i].Count;
			Cubes[i].RemoveRange(topCubeIndex - 5, 5);

			AddScore(10 + 5 * (Combo - 1));
			SoundManager.Instance.PlaySound(SoundManager.GameSound.Pop);
			Combo = Mathf.Clamp(Combo + 1, 1, 5);
			uiController.ComboUpdate(Combo);
			Camera.main.GetComponent<CameraController>().OnShakeCameraByPosition();
			topCube.Pop();

			ResetTimer();

			if (CheckAllEmpty())
			{
				StartCoroutine(AddNewCubes());
			}
		}
	}

	private void ResetTimer()
	{
		// 타이머 초기화
		if (isTimeAttack)
		{
			int timerRatio = modeController.GetMode().TimerRatio;
			timer = 0f;
			timerInterval = Mathf.Clamp(20f - (Score / timerRatio), 5f, 20f); // 진행도에 맞춰 만료시간 설정
			// Debug.Log(timerInterval);
		}
	}

	// 해당 Root의 맨 윗 큐브 반환
	public CubeController GetTopCube(int i)
	{
		int count = Cubes[i].Count;
		if (count > 0)
		{
			return Cubes[i][count - 1];
		}

		return null;
	}

	public void AddScore(int amount)
	{
		Score = Mathf.Clamp(Score + amount, 0, int.MaxValue);
		if (OnScoreChanged != null)
		{
			OnScoreChanged.Invoke();
		}
	}
}
