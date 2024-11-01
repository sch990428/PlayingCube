using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
	public class UserData
	{
		public int Money;
		public List<bool> Skins;
		public List<int> HighScores;

		public UserData()
		{
			Money = 0;
			Skins = new List<bool>();
			HighScores = new List<int>();
		}
	}
}
