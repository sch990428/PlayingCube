using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public interface IDifficulty
{
	public void InitQueue();
}

public class Difficulty : IDifficulty
{
	public Queue<Cube> CubeQueue;

	public virtual void InitQueue()
	{
		CubeQueue = new Queue<Cube>();
	}

	public void Shuffle(List<Cube> list)
	{
		// Fisher-Yates 알고리즘으로 큐브를 셔플
		System.Random rng = new System.Random();

		for (int n = list.Count - 1; n > 0; n--)
		{
			int k = rng.Next(n + 1);
			(list[n], list[k]) = (list[k], list[n]);
		}
	}
}

public class StarfishEasy : Difficulty
{
	public override void InitQueue()
	{
		base.InitQueue();
		List<Cube> tempList = new List<Cube>();

		for (int i = 0; i < 2; i++)
		{
			for (int type = 0; type < 1; type++)
			{
				for (int num = 1; num <= 5; num++)
				{
					tempList.Add(new Cube(num, type));
				}
			}
		}

		Shuffle(tempList);

		// 대기열에 넣기
		foreach (var cube in tempList)
		{
			CubeQueue.Enqueue(cube);
		}
	}
}

public class StarfishNormal : Difficulty
{
	public override void InitQueue()
	{
		base.InitQueue();
		List<Cube> tempList = new List<Cube>();

		for (int i = 0; i < 1; i++)
		{
			for (int type = 0; type < 2; type++)
			{
				for (int num = 1; num <= 5; num++)
				{
					tempList.Add(new Cube(num, type));
				}
			}
		}

		Shuffle(tempList);

		// 대기열에 넣기
		foreach (var cube in tempList)
		{
			CubeQueue.Enqueue(cube);
		}
	}
}

public class StarfishHard : Difficulty
{
	public override void InitQueue()
	{
		base.InitQueue();
		List<Cube> tempList = new List<Cube>();

		for (int i = 0; i < 1; i++)
		{
			for (int type = 0; type < 3; type++)
			{
				for (int num = 1; num <= 5; num++)
				{
					tempList.Add(new Cube(num, type));
				}
			}
		}

		Shuffle(tempList);

		// 대기열에 넣기
		foreach (var cube in tempList)
		{
			CubeQueue.Enqueue(cube);
		}
	}
}
