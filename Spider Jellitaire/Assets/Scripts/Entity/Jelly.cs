using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Jelly : MonoBehaviour
{
	public bool isMoving;

	public Define.JellyType type;

	Renderer jellyRenderer;

	private void Awake()
	{
		jellyRenderer = GetComponent<Renderer>();
	}

	public void ChangeType(Define.JellyType t)
	{
		jellyRenderer.material.color = Define.JellyColor[t];
	}
}
