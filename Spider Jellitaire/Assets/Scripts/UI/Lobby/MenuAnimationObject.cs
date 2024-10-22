using UnityEngine;

public class MenuAnimation : MonoBehaviour
{
	private Vector3 defaultPos;

	private Material material;

	private void Awake()
	{
		defaultPos = transform.position;
	}

	private void OnDisable()
	{
		transform.position = defaultPos;
	}
}
