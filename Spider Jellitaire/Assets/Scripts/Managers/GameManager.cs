using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	[SerializeField]
	private GameObject LineRoots;

	[SerializeField]
	private Score UI_Score;

	public int score = 0;
	public bool anyJellyMoving;

	public int Column = 7;
	public float BottomCastOriginZ = -20f;

	public Dictionary<int, Jelly> bottomJellies;

	public IDifficulty difficulty;

	protected override void Awake()
	{
		base.Awake();
		bottomJellies = new Dictionary<int, Jelly>();
		for (int i = 0; i < Column; i++)
		{
			bottomJellies[i] = null;
		}

		UI_Score.ScoreText.text = score.ToString();
	}

	private void Start()
	{
		Debug.Log(LobbyManager.Instance.difficulty);
		difficulty = LobbyManager.Instance.difficulty;
		AddJellyLine();
		AddJellyLine();
		Physics.SyncTransforms();
		GetBottomJellies();
	}

	public void AddJellyLine()
	{
		Debug.Log("젤리 추가");

		for (int i = 0; i < Column; i++)
		{
			Transform line = LineRoots.transform.GetChild(i);

			if (difficulty == null)
			{
				difficulty = new HardStrategy();
			}
			GameObject newJelly = difficulty.CreateNewJelly();
			Jelly j = newJelly.GetComponent<Jelly>();

			if (line.childCount == 0)
			{
				newJelly.transform.SetParent(line.transform);
				newJelly.transform.position = new Vector3(i - 3, 0, 7.5f);
			}
			else
			{
				// 이미 하위에 젤리가 있다는 뜻이므로 링크드리스트처럼 순회
				Jelly current = line.GetChild(0).GetComponent<Jelly>();
				int height = 1;
				while (current.Child != null)
				{
					current = current.Child;
					height++;
				}

				current.Child = j;
				j.Parent = current;
				newJelly.transform.SetParent(current.transform);
				newJelly.transform.position = new Vector3(i - 3, 0, 7.5f - height);
			}
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.W))
		{
			AddJellyLine();
			Physics.SyncTransforms();
			OnJellyChanged();
		}

		if (Input.GetKeyDown(KeyCode.S))
		{
			foreach (KeyValuePair<int, Jelly> jelly in bottomJellies)
			{
				if (jelly.Value != null)
				{
					Debug.Log($"{jelly.Key} : {jelly.Value.Number}");
				}
			}
		}
	}

	public void OnJellyChanged()
	{
		Debug.Log("젤리 놓임");
		GetBottomJellies();
		TryPop();
	}

	public void GetBottomJellies()
	{
		Debug.Log("최하단 젤리 목록 갱신");
		for (int i = 0; i < Column; i++)
		{
			RaycastHit hit;

			int jellyLayerMask = LayerMask.GetMask("JellyEntity");

			Vector3 origin = new Vector3(i - 3, 0, BottomCastOriginZ);
			Debug.DrawRay(origin, Vector3.forward * 100f, Color.red, 1f);
			if (Physics.Raycast(origin, Vector3.forward, out hit, 100f, jellyLayerMask))
			{
				Debug.Log(hit.collider.name);
				if (hit.collider.name == "Jelly")
				{
					bottomJellies[i] = hit.collider.transform.GetComponent<Jelly>();
				}
				else
				{
					bottomJellies[i] = null;
				}
			}
			else
			{
				bottomJellies[i] = null;
			}
		}
	}

	public void TryPop()
	{
		for (int i = 0; i < Column; i++)
		{
			if (bottomJellies[i] != null && bottomJellies[i].Number == 5)
			{
				Debug.Log($"{i}줄의 밑이 5다!");
				if (bottomJellies[i].IsHierarchyReverse())
				{
					Debug.Log($"{i}줄이 계층적이니 팝!");
					bottomJellies[i].Pop(i);
					bottomJellies[i] = null;
					score++;
					UI_Score.ScoreText.text = score.ToString();
				}
			}
		}
	}
}
