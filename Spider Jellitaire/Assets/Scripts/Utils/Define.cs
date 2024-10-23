using System.Collections.Generic;
using UnityEngine;

public static class Define
{
	public static Dictionary<JellyType, Color32> JellyColor;

	static Define()
	{
		JellyColor = new Dictionary<JellyType, Color32>();
		InitJellyColor();
	}

	public static void InitJellyColor()
	{
		JellyColor.Add(JellyType.Unknown, new Color32(46, 46, 46, 230));
		JellyColor.Add(JellyType.Blue, new Color32(61, 102, 149, 230));
		JellyColor.Add(JellyType.Red, new Color32(173, 69, 61, 230));
		JellyColor.Add(JellyType.Green, new Color32(61, 149, 62, 230));
		JellyColor.Add(JellyType.Yellow, new Color32(158, 159, 97, 230));
	}

	public enum JellyType
	{
		Unknown,
		Blue,
		Red,
		Green,
		Yellow
	}
}
