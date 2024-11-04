using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopController : MonoBehaviour
{
	[SerializeField]
	TMP_Text SkinName;

    public Dictionary<int, Data.SkinData> SkinDict;
    public int index;
    public int maxIndex;

    private void Awake()
    {
        SkinDict = DataManager.Instance.LoadJsonToDict<Data.SkinData>("Datas/skins");

		maxIndex = SkinDict.Count - 1;
    }

	private void Update()
    {
        SkinName.text = SkinDict[index].SkinName;
    }

	public void Next()
	{
		if (index < maxIndex)
		{
			index++;
		}
	}

	public void Previous()
	{
		if (index > 0)
		{
			index--;
		}
	}
}
