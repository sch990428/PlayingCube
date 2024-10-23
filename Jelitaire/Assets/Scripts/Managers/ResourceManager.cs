using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
	// 해당 경로에 있는 특정 타입의 리소스를 반환
	public T Load<T>(string path) where T : Object
	{
		T resource = Resources.Load<T>(path);
		if (resource == null)
		{
			Debug.Log($"{path}에 해당 리소스가 존재하지 않습니다");
		}
		return resource;
	}

	// 해당 경로에 있는 리소스를 불러와 게임오브젝트로 만듬
	public GameObject Instantiate(string path, Transform parent = null)
	{
		GameObject original = Load<GameObject>(path);

		if (original == null){ return null; }

		GameObject go = Instantiate(original, parent);
		go.name = original.name;

		return go;
	}

	// 게임오브젝트를 파괴
	public void Destroy(GameObject go, float t = 0f)
	{
		if (go == null){ return; }
		UnityEngine.Object.Destroy(go, t);
	}
}
