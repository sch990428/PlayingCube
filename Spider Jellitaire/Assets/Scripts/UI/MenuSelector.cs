using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuSelector : Selector<GameObject>
{
	public TMP_Text ModeName;
	public TMP_Text ModeDescription;

	[SerializeField]
	private Transform menuSlots;

	private Dictionary<int, Data.GameMode> modeDict;

	public override void Awake()
	{
		base.Awake();

		modeDict = LobbyManager.Instance.ModeDict;

		foreach (KeyValuePair<int, Data.GameMode> mode in modeDict)
		{
			GameObject go = ResourceManager.Instance.Instantiate($"Prefabs/UI/ModeThumbnails/{mode.Value.ModeThumbnailPath}", menuSlots);
			items.AddLast(new KeyValuePair<int, GameObject>(mode.Key, go));
			go.SetActive(false);
			itemCount++;
		}

		UpdateSlot();
	}

	public void UpdateSlot()
	{
		items.GetLast().Value.SetActive(false);
		items.GetFront().Value.SetActive(true);

		ModeName.text = modeDict[items.GetFront().Key].ModeName;
		ModeDescription.text = modeDict[items.GetFront().Key].ModeDescription;
	}

	public override void OnEndDrag(PointerEventData eventData)
	{
		if (!isDragging) { return; }

		switch (dragDicrection)
		{
			case DragDicrection.None:

			break;
			case DragDicrection.DragRight:
				items.SlideRight();
				break;
			case DragDicrection.DragLeft:
				items.SlideLeft();
				break;
		}

		UpdateSlot();

		isDragging = false;
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
		if (isDragging) { return; }
		Debug.Log("게임 진입");
	}
}
