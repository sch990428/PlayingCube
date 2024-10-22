using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager>
{
	[SerializeField]
	private GameObject LineRoots;

	public bool anyJellyMoving;

	public int Column = 7;
	public float BottomCastOriginZ = -20f;

	private Dictionary<int, Jelly> bottomJellies;

	protected override void Awake()
	{
		base.Awake();
		bottomJellies = new Dictionary<int, Jelly>();
		for (int i = 0; i < Column; i++)
		{
			bottomJellies[i] = null;
		}
	}

	private void Start()
	{
		AddJellyLine();
		AddJellyLine();
		Physics.SyncTransforms();
		GetBottomJellies();
	}

	private void AddJellyLine()
	{
		Debug.Log("젤리 추가");
		for (int i = 0; i < Column; i++)
		{
			Transform line = LineRoots.transform.GetChild(i);

			GameObject newJelly = ResourceManager.Instance.Instantiate("Prefabs/GameEntity/Jelly");
			Jelly j = newJelly.GetComponent<Jelly>();
			j.Number = Random.Range(1, 6);
			j.ChangeType(Define.JellyType.Green);

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
			GetBottomJellies();
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
		}

		TryPop();
	}

	public void TryPop()
	{
		foreach (KeyValuePair<int, Jelly> jelly in bottomJellies)
		{
			if (jelly.Value != null && jelly.Value.Number == 5)
			{
				if (jelly.Value.IsHierarchyReverse())
				{
					jelly.Value.Pop();
				}
			}
		}
	}
}
