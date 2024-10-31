using UnityEngine;

public class SkyboxController : MonoBehaviour
{
	Material material;
	
	private void Awake()
	{
		// 게임 시작 시 배경색 랜덤하게 변경
		material = ResourceManager.Instance.Load<Material>("Art/Materials/Skybox");

		int min = 81;
		int max = 177;

		byte r = (byte)Random.Range(min, max);
		byte g = (byte)Random.Range(min, max);
		byte b = (byte)Random.Range(min, max);
		Color32 newColor = new Color32(r, g, b, 255);

		material.SetColor("_Tint", newColor);
	}
}
