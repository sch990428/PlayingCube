using System.IO;
using UnityEngine;

public class OptionManager : Singleton<OptionManager>
{
	private string OptionDataPath;
	public Data.OptionData OptionData;

	protected override void Awake()
	{
		base.Awake();
		OptionDataPath = Path.Combine(Application.persistentDataPath, "OptionData.json");
		OptionData = DataManager.Instance.LoadJsonToClass<Data.OptionData>(OptionDataPath);

		if (OptionData == null)
		{
			OptionData = new Data.OptionData();
			DataManager.Instance.SaveClassToJson<Data.OptionData>(OptionDataPath, OptionData);
		}
	}

	public void SaveOptionData()
	{
		DataManager.Instance.SaveClassToJson<Data.OptionData>(OptionDataPath, OptionData);
	}
}
