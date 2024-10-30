using UnityEngine;

namespace Data
{
	// 게임 모드 정보
	[System.Serializable]
	public class GameMode : BaseDataEntity
	{
		public string ModeName { get; set; }  // 모드 이름
		public string ModeDescription { get; set; }  // 모드 설명
		public string ModeThumbnailPath { get; set; }  // 모드 썸네일 프리팹 경로
	}
}