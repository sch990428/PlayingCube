using UnityEngine;

public class MenuAnimation : MonoBehaviour
{
	private Vector3 DefaultPos;

	private void Awake()
	{
		DefaultPos = transform.position;
	}

	private void OnDisable()
	{
		transform.position = DefaultPos;
	}
}
