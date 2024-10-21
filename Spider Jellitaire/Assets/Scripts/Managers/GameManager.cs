using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager>
{
	public bool anyJellyMoving;

	protected override void Awake()
	{
		base.Awake();

		for (int i = 0; i < 7; i++)
		{
			GameObject go = ResourceManager.Instance.Instantiate("Prefabs/GameEntity/Jelly");
			go.transform.position = new Vector3(i - 3, 0, 7.5f);
			Jelly j = go.GetComponent<Jelly>();
			j.Number = Random.Range(1, 6);
			j.ChangeType(Define.JellyType.Green);
			go.name += i;
		}
	}
}
