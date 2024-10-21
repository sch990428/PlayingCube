using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.DualShock.LowLevel;

public class Jelly : MonoBehaviour
{
	[SerializeField]
	TMP_Text NumberText;

	public bool isMoving;

	public Define.JellyType Type;
	public int Number;

	private Renderer jellyRenderer;
	private Vector3 prevPosition;

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
		if (InputManager.hasInstance())
		{
			InputManager.Instance.OnTouchPressed -= TouchPressed;
			InputManager.Instance.OnTouching -= Touching;
			InputManager.Instance.OnTouchReleased -= TouchReleased;
		}
	}

	public void UpdatePos(Vector3 pos)
	{
		transform.position = new Vector3(pos.x, pos.y, pos.z);
	}

	public void ChangeType(Define.JellyType t)
	{
		Type = t;
		jellyRenderer.material.color = Define.JellyColor[t];
		NumberText.text = $"{Number}";
	}

	private void TouchPressed(Jelly selectedJelly)
	{
		if (selectedJelly == this)
		{
			isMoving = true;
			GameManager.Instance.anyJellyMoving = true;
			prevPosition = transform.position;
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

			this.gameObject.GetComponent<Collider>().enabled = false;

			RaycastHit hit;
			// Debug.DrawRay(transform.position, Vector3.forward * 1f, Color.red, 1.5f);

			if (Physics.Raycast(transform.position, Vector3.forward, out hit, 1f))
			{
				if (hit.collider.CompareTag("JellyEntity"))
				{
					UpdatePos(hit.collider.transform.position + new Vector3(0, 0, -1f));
					Debug.Log($"상단에 젤리가 있어요 {hit.collider.name}");
					this.transform.SetParent(hit.collider.transform, true);
					this.gameObject.GetComponent<Collider>().enabled = true;
					return;
				}
			}

			this.gameObject.GetComponent<Collider>().enabled = true;
			UpdatePos(prevPosition);
		}
	}
}
