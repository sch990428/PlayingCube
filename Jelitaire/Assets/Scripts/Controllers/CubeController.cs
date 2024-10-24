using UnityEngine;

public class CubeController : MonoBehaviour
{
    public int Number;
    public int Type;

    public CubeController Parent; // 상위의 큐브 혹은 null(루트)
	public CubeController Child; // 하위의 큐브 혹은 null

	private void OnCollisionEnter(Collision collision)
	{
		Rigidbody rigidbody = GetComponent<Rigidbody>();
		
		// 충돌이 감지되면 큐브의 속도를 감소시켜 반동을 상쇄
		Vector3 newVelocity = rigidbody.linearVelocity * 0.15f;
		rigidbody.linearVelocity = newVelocity;
	}
}
