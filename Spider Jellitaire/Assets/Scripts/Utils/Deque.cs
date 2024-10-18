using System;
using System.Collections.Generic;
using UnityEngine;

public class Deque<T>
{
    private LinkedList<T> list = new LinkedList<T>();

    public void AddFront(T item) { list.AddFirst(item); }
	public void AddLast(T item) { list.AddLast(item); }

	public T GetFront()
	{
		if (list.Count == 0) { throw new InvalidOperationException("Deque가 비어있습니다"); }
		return list.First.Value;
	}

	public T GetLast()
	{
		if (list.Count == 0) { throw new InvalidOperationException("Deque가 비어있습니다"); }
		return list.Last.Value;
	}

	public T PopFront()
	{
		if (list.Count == 0) { throw new InvalidOperationException("Deque가 비어있습니다"); }
		T item = list.First.Value;
		list.RemoveFirst();
		return item;
	}

	public T PopLast()
	{
		if (list.Count == 0) { throw new InvalidOperationException("Deque가 비어있습니다"); }
		T item = list.Last.Value;
		list.RemoveLast();
		return item;
	}

	public void SlideLeft()
	{
		AddLast(PopFront());
	}

	public void SlideRight()
	{
		AddFront(PopLast());
	}
}
