using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CubeController : MonoBehaviour
{
	[SerializeField]
	private TMP_Text NumberText;

    public int Number;
    public int Type;

    public CubeController Parent; // 상위의 큐브 혹은 null(루트)
	public CubeController Child; // 하위의 큐브 혹은 null

	private Animator animator;

	private Renderer render;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		render = GetComponent<Renderer>();
	}

	private void OnEnable()
	{
		InputManager.Instance.OnTouchStart += TouchStart;
	}

	public void SetValue(int number, int type)
	{
		Number = number;
		Type = type;

		NumberText.text = Number.ToString();
		render.material.color = GameManager.Instance.CubeColors[Type];
	}

	public void DestroyCube()
	{
		if (Child != null)
		{
			Child.DestroyCube();
		}

		animator.SetTrigger("Destroy");

		ResourceManager.Instance.Destroy(gameObject, 0.35f);
	}

	private void TouchStart(CubeController target)
	{
		if (target == this)
		{
			transform.GetComponent<Rigidbody>().isKinematic = true;
			InputManager.Instance.OnTouching += Touching;
			InputManager.Instance.OnTouchEnd += TouchReleased;
		}
	}

	private void Touching(Vector2 pos)
	{
		transform.position = new Vector3(pos.x, pos.y, transform.position.z);
	}

	private void TouchReleased(CubeController target)
	{
		if (target == this)
		{
			InputManager.Instance.OnTouching -= Touching;
			transform.GetComponent<Rigidbody>().isKinematic = false;
			InputManager.Instance.OnTouchEnd -= TouchReleased;
		}
	}
}
