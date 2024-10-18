using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuSelector : Selector<GameObject>
{
	public override void Awake()
	{
		base.Awake();
		items.GetFront().SetActive(true);
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

		items.GetLast().SetActive(false);
		items.GetFront().SetActive(true);

		isDragging = false;
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
		if (isDragging) { return; }
		Debug.Log("게임 진입");
	}
}
