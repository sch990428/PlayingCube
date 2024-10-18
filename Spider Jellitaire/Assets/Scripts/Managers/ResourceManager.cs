using Mono.Cecil;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
	public T Load<T>(string path) where T : Object
	{
		T resource = Resources.Load<T>(path);
		if (resource == null)
		{
			Debug.Log($"{path}에 해당 리소스가 존재하지 않습니다");
		}
		return resource;
	}

	public GameObject Instantiate(string path, Transform parent = null)
	{
		GameObject original = Load<GameObject>(path);

		if (original == null){ return null; }

		GameObject go = Instantiate(original, parent);
		go.name = original.name;

		return go;
	}

	public void Destroy(GameObject go, float t = 0f)
	{
		if (go == null){ return; }
		UnityEngine.Object.Destroy(go, t);
	}
}
