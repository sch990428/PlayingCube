using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : Singleton<LobbyManager>
{
	public Dictionary<int, Data.GameMode> ModeDict;
	public IDifficulty difficulty;

	protected override void Awake()
	{
		base.Awake();

		ModeDict = DataManager.Instance.LoadJsonToDict<Data.GameMode>("Datas/gamemodes");
	}
}
