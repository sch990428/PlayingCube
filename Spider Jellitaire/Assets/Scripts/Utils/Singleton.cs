using Unity.VisualScripting;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T instance;
	public static T Instance
	{
		get { Init(); return instance; }
	}

	private static void Init()
	{
		if (instance == null)
		{
			instance = FindAnyObjectByType<T>();

			if (instance == null)
			{
				GameObject go = new GameObject(typeof(T).Name);
				instance = go.AddComponent<T>();
			}
		}
	}

	protected virtual void Awake()
	{
		if (instance == null)
		{
			instance = this as T;
			DontDestroyOnLoad(gameObject);
		}
		else if(instance != this)
		{
			Destroy(gameObject);
		}
	}
}
