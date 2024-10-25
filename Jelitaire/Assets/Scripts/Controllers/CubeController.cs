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

	public void SetValue(int number, int type)
	{
		Number = number;
		Type = type;

		NumberText.text = Number.ToString();
		render.material.color = GameManager.Instance.CubeColors[Type];
	}

	private void OnCollisionEnter(Collision collision)
	{
		Rigidbody rigidbody = GetComponent<Rigidbody>();
		
		// 충돌이 감지되면 큐브의 속도를 감소시켜 반동을 상쇄
		Vector3 newVelocity = rigidbody.linearVelocity * 0.15f;
		rigidbody.linearVelocity = newVelocity;
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
}
