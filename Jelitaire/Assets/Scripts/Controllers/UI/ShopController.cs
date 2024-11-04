using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
	[SerializeField]
	UIController UIController;

	[SerializeField]
	TMP_Text SkinName;

	[SerializeField]
	Renderer Preview;

	[SerializeField]
	Button ApplyButton;

	[SerializeField]
	Button PurchaseButton;

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

		ChangeIndex();
	}

	public void ChangeIndex()
    {
        SkinName.text = SkinDict[index].SkinName;
		Preview.material = ResourceManager.Instance.Load<Material>($"Art/Materials/Cube/{SkinDict[index].SkinMaterialPath}");

		if (UIController.UserData.Skins[index])
		{
			ApplyButton.gameObject.SetActive(true);
			PurchaseButton.gameObject.SetActive(false);
		}
		else
		{
			ApplyButton.gameObject.SetActive(false);
			PurchaseButton.gameObject.SetActive(true);
		}
    }

	public void Next()
	{
		if (index < maxIndex)
		{
			index++;
			ChangeIndex();
		}
	}

	public void Previous()
	{
		if (index > 0)
		{
			index--;
			ChangeIndex();
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
