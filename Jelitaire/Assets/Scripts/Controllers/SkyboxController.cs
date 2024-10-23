using UnityEngine;

public class SkyboxController : MonoBehaviour
{
	Material material;
	private void Awake()
	{
		// 게임 시작 시 배경색 랜덤하게 변경
		material = ResourceManager.Instance.Load<Material>("Art/Materials/Skybox");

		byte r = (byte)Random.Range(125, 188);
		byte g = (byte)Random.Range(125, 188);
		byte b = (byte)Random.Range(125, 188);
		Color32 newColor = new Color32(r, g, b, 255);

		material.SetColor("_Tint", newColor);
	}
}
