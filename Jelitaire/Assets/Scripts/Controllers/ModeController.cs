using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ModeController : MonoBehaviour
{
	[SerializeField]
	private TMP_Text ModeName;

	[SerializeField]
	private TMP_Text ModeDescription;

	private Dictionary<int, Data.GameMode> modeDict;

	private int selected;

	private void Awake()
	{
		modeDict = DataManager.Instance.LoadJsonToDict<Data.GameMode>("Datas/gamemodes");
		selected = 2;
		UpdateModeInfo(selected);
	}

	public void OnPrevButtonClick()
	{
		selected--;
		if (selected < 1) { selected = modeDict.Count; }
		UpdateModeInfo(selected);
	}

	public void OnNextButtonClick()
	{
		selected++;
		if (selected > modeDict.Count) { selected = 1; }
		UpdateModeInfo(selected);
	}

	private void UpdateModeInfo(int index)
	{
		ModeName.text = modeDict[index].ModeName;
		ModeDescription.text = modeDict[index].ModeDescription;
	}
}
