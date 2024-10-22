using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
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
		Debug.Log(dragDicrection);
		switch (dragDicrection)
		{
			case DragDicrection.None:

			break;
			case DragDicrection.DragToRight:
				// 이전으로
				items.GetFront().Value.SetActive(false);
				items.SlideRight();
				break;
			case DragDicrection.DragToLeft:
				// 다음으로
				items.SlideLeft();			
				break;
		}

		UpdateSlot();

		isDragging = false;
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
		if (isDragging) { return; }
		SceneManager.LoadScene("SpiderScene");
	}
}
