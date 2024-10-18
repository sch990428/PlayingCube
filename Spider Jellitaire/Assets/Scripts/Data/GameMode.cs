using UnityEngine;

namespace Data
{
	[System.Serializable]
	public class GameMode
	{
		public int ModeId { get; set; } // 모드 ID
		public string ModeName { get; set; }  // 모드 이름
		public string ModeDescription { get; set; }  // 모드 설명
		public string ModeThumbnailPath { get; set; }  // 모드 썸네일 경로
	}
}