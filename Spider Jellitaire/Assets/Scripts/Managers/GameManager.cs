using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
	public enum JellyType
	{
		Unknown,
		Blue,
		Green,
		Red,
		Yellow
	}

	public Dictionary<JellyType, Color32> JellyColor;

	private PlayerInput playerInput;

	private InputAction touchPositionAction;
	private InputAction touchPressedAction;

	private void Awake()
	{
		playerInput = GetComponent<PlayerInput>();

		for (int i = 0; i < 7; i++)
		{
			GameObject go = ResourceManager.Instance.Instantiate("Prefabs/GameEntity/Jelly");
			go.transform.position = new Vector3(i - 3, 0, 6);
		}
	}
}
