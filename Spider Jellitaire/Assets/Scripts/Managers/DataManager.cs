using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DataManager : Singleton<DataManager>
{
	protected override void Awake()
	{
		base.Awake();
	}

	public List<T> LoadJsonToList<T>(string path) where T : class
	{
		string json = ResourceManager.Instance.Load<TextAsset>(path).text;
		List<T> list = JsonConvert.DeserializeObject<List<T>>(json);

		return list;
	}

	public Dictionary<int, T> LoadJsonToDict<T>(string path) where T : BaseEntity
	{
		List<T> list = LoadJsonToList<T>(path);
		Dictionary<int, T> dict = new Dictionary<int, T>();

		foreach (T item in list)
		{
			dict.Add(item.ID, item);
		}

		return dict;
	}
}
