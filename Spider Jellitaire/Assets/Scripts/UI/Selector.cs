using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selector<T> : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
	public List<T> itemlist;
	protected Deque<T> items;

	public virtual void Awake()
	{
		items = new Deque<T>();
		foreach (T item in itemlist)
		{
			items.AddLast(item);
		}
	}

	public virtual void OnBeginDrag(PointerEventData eventData)
	{
		
		if (eventData.delta.x > 0)
		{
			// ¿À¸¥ÂÊ
			items.SlideRight();
		}
		else if (eventData.delta.x < 0)
		{
			// ¿ÞÂÊ
			items.SlideLeft();
		}

		Debug.Log("Drag");
	}

	public void OnDrag(PointerEventData eventData)
	{

	}

	public void OnEndDrag(PointerEventData eventData)
	{

	}
}
