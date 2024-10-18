using UnityEngine;

namespace Data
{
	[System.Serializable]
	public class GameMode
	{
		public int ModeId { get; set; } // ��� ID
		public string ModeName { get; set; }  // ��� �̸�
		public string ModeDescription { get; set; }  // ��� ����
		public string ModeThumbnailPath { get; set; }  // ��� ����� ���
	}
}