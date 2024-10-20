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

	public void UpdatePos()
	{
		Vector3 pos = transform.position;
		transform.position = new Vector3(Mathf.RoundToInt(pos.x), 0, Mathf.RoundToInt(pos.z));
	}

	public void ChangeType(Define.JellyType t)
	{
		jellyRenderer.material.color = Define.JellyColor[t];
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!GameManager.Instance.anyJellyMoving)
		{
			Debug.Log("Æ®¸®°Å!");
		}
	}
}
