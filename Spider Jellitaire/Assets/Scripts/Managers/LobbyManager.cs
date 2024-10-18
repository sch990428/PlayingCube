using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : Singleton<LobbyManager>
{
	public Dictionary<int, Data.GameMode> ModeDict;

	protected override void Awake()
	{
		base.Awake();

		ModeDict = new Dictionary<int, Data.GameMode>();

		string modeJson = ResourceManager.Instance.Load<TextAsset>("Datas/gamemodes").text;
		List<Data.GameMode> list = JsonConvert.DeserializeObject<List<Data.GameMode>>(modeJson);
		
		foreach (Data.GameMode mode in list)
		{
			ModeDict.Add(mode.ModeId, mode);
		}
	}
}
