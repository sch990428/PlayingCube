using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	private float shakeTime;
	private float shakeIntensity;

	Vector3 defaultPos;
	Vector3 defaultRot;

	Material material;
	
	private void Awake()
	{
		defaultPos = transform.position; // 카메라 초기위치 저장
		defaultRot = transform.eulerAngles; // 카메라 초기방향 저장

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

	// 카메라 흔들림 효과
	public void OnShakeCamera(float shakeTime = 0.2f, float shakeIntensity = 0.1f)
	{
		this.shakeTime = shakeTime;
		this.shakeIntensity = shakeIntensity;

		StopCoroutine(ShakeByPosition());
		StartCoroutine(ShakeByPosition());
	}

	// 위치기반
	private IEnumerator ShakeByPosition()
	{
		Debug.Log("흔들림");
		while (shakeTime > 0f)
		{
			float z = Random.Range(-1f, 1f);
			transform.position = defaultPos + Random.insideUnitSphere * shakeIntensity;

			shakeTime -= Time.deltaTime;

			yield return null;
		}

		transform.position = defaultPos;
	}

	// 각도기반
	private IEnumerator ShakeByRotation()
	{
		Vector3 defaultRot = transform.eulerAngles;

		while (shakeTime > 0f)
		{
			float x = 0;
			float y = 0;
			float z = Random.Range(-1f, 1f);
			transform.rotation = Quaternion.Euler(defaultRot + new Vector3(x, y, z) * shakeIntensity * 10f);

			shakeTime -= Time.deltaTime;

			yield return null;
		}
	}
}
