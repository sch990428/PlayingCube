using Data;
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

	[SerializeField]
	TMP_Text ApplyText;

	[SerializeField]
	TMP_Text PriceText;

	[SerializeField]
	TMP_Text PurchaseText;

	public Dictionary<int, Data.SkinData> SkinDict;
    public int index;
    public int maxIndex;

    private void Awake()
    {
		SkinDict = GameManager.Instance.SkinDict;
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
			ApplyText.text = "장착";
			if (UIController.UserData.CurrentSkins == index)
			{
				ApplyText.text = "장착중";
			}
			ApplyButton.gameObject.SetActive(true);
			PurchaseButton.gameObject.SetActive(false);
		}
		else
		{
			PurchaseText.text = "구매불가";
			if (UIController.UserData.Money >= SkinDict[index].SkinPrice)
			{
				PurchaseText.text = "구매";
			}
			PriceText.text = SkinDict[index].SkinPrice.ToString();
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
		if (!UIController.UserData.Skins[index] && UIController.UserData.Money >= SkinDict[index].SkinPrice)
		{
			// 구매 로직
			ConfirmMessageController msg = ResourceManager.Instance.Instantiate("Prefabs/UI/ConfirmMessage", transform).GetComponent<ConfirmMessageController>();
			msg.Init("정말 구매하시겠습니까?", () =>
			{
				Time.timeScale = 1.0f;

				UIController.UserData.Money -= SkinDict[index].SkinPrice;
				UIController.UserData.Skins[index] = true;
				UIController.UpdateMoney();
				UIController.SaveUserData();
				ChangeIndex();
				ResourceManager.Instance.Destroy(msg.gameObject);
			});
		}
	}

	public void Select()
	{
		UIController.UserData.CurrentSkins = index;
		UIController.SaveUserData();
		ChangeIndex();
	}
}
