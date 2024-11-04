using Data;
using UnityEngine;

namespace Data
{
	// 스킨 정보
	[System.Serializable]
	public class SkinData : BaseDataEntity
	{
		public string SkinName { get; set; }  // 스킨 이름
		public int SkinPrice { get; set; } // 스킨 가격
		public string SkinMaterialPath { get; set; }  // 스킨 텍스쳐 경로
	}
}

