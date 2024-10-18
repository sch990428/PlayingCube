using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selector<T> : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
	public enum DragDicrection
	{
		None,
		DragLeft,
		DragRight
	}

	[SerializeField]
	protected List<T> itemlist;

	protected Deque<T> items;

	protected DragDicrection dragDicrection;
	protected bool isDragging = false;

	private float draggedTime;

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
		isDragging = false;
		draggedTime = Time.time;
		dragDicrection = DragDicrection.None;
	}

	public virtual void OnDrag(PointerEventData eventData)
	{
        if (Time.time - draggedTime > 0.08f)
        {
			isDragging = true;
		}
        
		if (eventData.delta.x > 0)
		{
			// ¿À¸¥ÂÊ
			dragDicrection = DragDicrection.DragRight;
		}
		else if (eventData.delta.x < 0)
		{
			// ¿ÞÂÊ
			dragDicrection = DragDicrection.DragLeft;
		}
	}

	public virtual void OnEndDrag(PointerEventData eventData)
	{

	}

	public virtual void OnPointerClick(PointerEventData eventData)
	{
		
	}
}
