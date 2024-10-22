using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selector<T> : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
	public enum DragDicrection
	{
		None,
		DragToLeft,
		DragToRight
	}

	[SerializeField]
	protected List<T> itemlist;

	protected Deque<KeyValuePair<int, T>> items;
	protected int itemCount;

	protected DragDicrection dragDicrection;
	protected bool isDragging = false;

	private float draggedTime;

	public virtual void Awake()
	{
		itemCount = 0;
		items = new Deque<KeyValuePair<int, T>>();
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
			dragDicrection = DragDicrection.DragToRight;
		}
		else if (eventData.delta.x < 0)
		{
			// ¿ÞÂÊ
			dragDicrection = DragDicrection.DragToLeft;
		}
	}

	public virtual void OnEndDrag(PointerEventData eventData)
	{

	}

	public virtual void OnPointerClick(PointerEventData eventData)
	{
		
	}
}
