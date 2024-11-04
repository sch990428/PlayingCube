using UnityEngine;

namespace Data
{
	// 게임 모드 정보
	[System.Serializable]
	public class GameMode : BaseDataEntity
	{
		public string ModeName { get; set; }  // 모드 이름
		public string ModeDescription { get; set; }  // 모드 설명
		public float RewardRatio { get; set; } // 보상 배율 (총 점수에서 이 값을 곱하여 보상)
		public int TimerRatio { get; set; } // 난이도 상승 배율 (이 점수마다 시간이 1초 줄어듬)
		public string ModeThumbnailPath { get; set; }  // 모드 썸네일 프리팹 경로
	}
}