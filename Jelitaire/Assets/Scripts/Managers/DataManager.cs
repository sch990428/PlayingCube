using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
	protected override void Awake()
	{
		base.Awake();
	}
	
	// Json 데이터를 단일 클래스로 불러온다
	public T LoadJsonToClass<T>(string path) where T : class
	{
        if (File.Exists(path))
        {
			string json = File.ReadAllText(path);
			return JsonUtility.FromJson<T>(json);
		}
        
		return null;
	}

	// 단일 클래스를 Json 데이터로 저장한다
	public void SaveClassToJson<T>(string path, T data) where T : class
	{
		string json = JsonUtility.ToJson(data, true);
		File.WriteAllText(path, json);
	}

	// Json 데이터를 리스트로 불러온다
	public List<T> LoadJsonToList<T>(string path) where T : class
	{
		string json = ResourceManager.Instance.Load<TextAsset>(path).text;
		List<T> list = JsonConvert.DeserializeObject<List<T>>(json);

		return list;
	}

	// Json 데이터를 딕셔너리로 불러온다
	public Dictionary<int, T> LoadJsonToDict<T>(string path) where T : Data.BaseDataEntity
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
