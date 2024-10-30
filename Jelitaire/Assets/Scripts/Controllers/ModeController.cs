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

	public int Selected;

	private void Awake()
	{
		modeDict = DataManager.Instance.LoadJsonToDict<Data.GameMode>("Datas/gamemodes");
		Selected = 2;
		UpdateModeInfo(Selected);
	}

	// 이전 버튼
	public void OnPrevButtonClick()
	{
		Selected--;
		if (Selected < 1) { Selected = modeDict.Count; }
		UpdateModeInfo(Selected);
	}

	// 다음 버튼
	public void OnNextButtonClick()
	{
		Selected++;
		if (Selected > modeDict.Count) { Selected = 1; }
		UpdateModeInfo(Selected);
	}

	// 모드 정보 업데이트시 호출
	private void UpdateModeInfo(int index)
	{
		ModeName.text = modeDict[index].ModeName;
		ModeDescription.text = modeDict[index].ModeDescription;
	}
}
