using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager>
{
	public bool anyJellyMoving;

	private PlayerInput playerInput;

	private Jelly[,] stage = new Jelly[7, 16];

	public int gridX = 34;
	public int gridY = 34;

	private InputAction touchPositionAction;
	private InputAction touchPressedAction;

	private void Awake()
	{
		playerInput = GetComponent<PlayerInput>();

		for (int i = 0; i < 7; i++)
		{
			GameObject go = ResourceManager.Instance.Instantiate("Prefabs/GameEntity/Jelly");
			stage[i, 0] = go.AddComponent<Jelly>();
			go.transform.position = new Vector3(i - 3, 0, 6);
			go.GetComponent<Jelly>().ChangeType(Define.JellyType.Green);
		}
	}
}
