using JetBrains.Annotations;
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

	private Jelly[] bottomJellies;

	protected override void Awake()
	{
		base.Awake();
		bottomJellies = new Jelly[Column];
		AddJellyLine();
		AddJellyLine();
	}

	private void AddJellyLine()
	{
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

		UpdateLastJelly();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.W))
		{
			AddJellyLine();
		}

		if (Input.GetKeyDown(KeyCode.S))
		{
			foreach (Jelly jelly in bottomJellies)
			{
				if (jelly != null)
				{
					Debug.Log(jelly.Number);
				}
			}
		}
	}

	public void UpdateLastJelly()
	{
		for (int i = 0; i < Column; i++)
		{
			RaycastHit hit;

			Debug.DrawRay(new Vector3(i - 3, 0.1f, BottomCastOriginZ), Vector3.forward * Mathf.Infinity, Color.red, 4f);
			if (Physics.Raycast(new Vector3(i - 3, 0.1f, BottomCastOriginZ), Vector3.forward, out hit, Mathf.Infinity))
			{
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
		foreach (Jelly jelly in bottomJellies)
		{
			if (jelly != null && jelly.Number == 5)
			{
				if (jelly.IsHierarchyReverse())
				{
					jelly.Pop();
				}
			}
		}
	}
}
