using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopController : MonoBehaviour
{
	[SerializeField]
	TMP_Text SkinName;

	[SerializeField]
	Renderer Preview;

	public Dictionary<int, Data.SkinData> SkinDict;
    public int index;
    public int maxIndex;

    private void Awake()
    {
        SkinDict = DataManager.Instance.LoadJsonToDict<Data.SkinData>("Datas/skins");
		foreach (var skin in SkinDict)
		{
			Debug.Log(skin.Value.SkinMaterialPath);
		}
		maxIndex = SkinDict.Count - 1;
    }

	private void Update()
    {
        SkinName.text = SkinDict[index].SkinName;
		Preview.material = ResourceManager.Instance.Load<Material>($"Art/Materials/Cube/{SkinDict[index].SkinMaterialPath}");
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

	public void Purchase()
	{

	}

	public void Select()
	{
		GameManager.Instance.material = ResourceManager.Instance.Load<Material>($"Art/Materials/Cube/{SkinDict[index].SkinMaterialPath}");
	}
}
