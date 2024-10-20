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
		JellyColor.Add(JellyType.Unknown, new Color32(46, 46, 46, 170));
		JellyColor.Add(JellyType.Blue, new Color32(61, 102, 149, 170));
		JellyColor.Add(JellyType.Green, new Color32(61, 149, 62, 170));
		JellyColor.Add(JellyType.Red, new Color32(173, 69, 61, 170));
		JellyColor.Add(JellyType.Yellow, new Color32(254, 255, 97, 170));
	}

	public enum JellyType
	{
		Unknown,
		Blue,
		Green,
		Red,
		Yellow
	}
}
