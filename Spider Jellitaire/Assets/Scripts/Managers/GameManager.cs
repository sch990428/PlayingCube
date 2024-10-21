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

	protected override void Awake()
	{
		base.Awake();
		AddJellyLine();
		AddJellyLine();
	}

	private void AddJellyLine()
	{
		for (int i = 0; i < 7; i++)
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
		}
	}
}
