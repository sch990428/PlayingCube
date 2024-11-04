using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
	public class UserData
	{
		public int Money;
		public bool[] Skins;
		public int[] HighScores;

		public UserData()
		{
			Money = 0;
			Skins = new bool[6];
			HighScores = new int[6];
		}
	}
}
