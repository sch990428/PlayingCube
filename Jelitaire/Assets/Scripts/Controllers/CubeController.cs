using UnityEngine;

public class CubeController : MonoBehaviour
{
    public int Number;
    public int Type;

    public CubeController Parent; // 상위의 큐브 혹은 null(루트)
	public CubeController Child; // 하위의 큐브 혹은 null
}
