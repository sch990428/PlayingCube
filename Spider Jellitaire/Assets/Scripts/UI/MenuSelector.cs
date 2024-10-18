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

	public override void OnBeginDrag(PointerEventData eventData)
	{
		if (eventData.delta.x > 0)
		{
			// 오른쪽
			items.SlideRight();
		}
		else if (eventData.delta.x < 0)
		{
			// 왼쪽
			items.SlideLeft();
		}

		items.GetLast().SetActive(false);
		items.GetFront().SetActive(true);
	}
}
