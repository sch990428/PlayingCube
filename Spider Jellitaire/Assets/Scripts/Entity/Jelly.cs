using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.DualShock.LowLevel;

public class Jelly : MonoBehaviour
{
	public bool isMoving;

	public Define.JellyType type;

	Renderer jellyRenderer;

	private void Awake()
	{
		jellyRenderer = GetComponent<Renderer>();
	}

	private void OnEnable()
	{
		InputManager.Instance.OnTouchPressed += TouchPressed;
		InputManager.Instance.OnTouching += Touching;
		InputManager.Instance.OnTouchReleased += TouchReleased;
	}

	private void OnDisable()
	{
		InputManager.Instance.OnTouchPressed -= TouchPressed;
		InputManager.Instance.OnTouching -= Touching;
		InputManager.Instance.OnTouchReleased -= TouchReleased;
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

	private void TouchPressed(Jelly selectedJelly)
	{
		if (selectedJelly == this)
		{
			isMoving = true;
			GameManager.Instance.anyJellyMoving = true;
		}
	}

	private void Touching(Vector3 pos)
	{
		if (isMoving)
		{
			transform.position = pos;
		}
	}

	private void TouchReleased(Vector3 pos)
	{
		if (isMoving)
		{
			isMoving = false;
			GameManager.Instance.anyJellyMoving = false;
			UpdatePos();

			RaycastHit hit;
			Debug.DrawRay(transform.position, Vector3.forward, Color.red, 1f);
			this.gameObject.GetComponent<Collider>().enabled = false;
			if (Physics.Raycast(transform.position, Vector3.forward, out hit, 1f))
			{
				if (hit.collider.CompareTag("JellyEntity"))
				{
					Debug.Log("상단에 젤리가 있어요");
				}
			}
			this.gameObject.GetComponent<Collider>().enabled = true;
		}
	}
}
