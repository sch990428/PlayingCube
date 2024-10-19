using UnityEngine;

namespace Data
{
	[System.Serializable]
	public class GameMode : BaseEntity
	{
		public string ModeName { get; set; }  // ��� �̸�
		public string ModeDescription { get; set; }  // ��� ����
		public string ModeThumbnailPath { get; set; }  // ��� ����� ���
	}
}