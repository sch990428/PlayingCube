using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
	[SerializeField]
	private List<AudioClip> audioClips; // 오디오 클립

	private AudioSource audioSource;

	// 게임 사운드 요소 (순서 주의)
	public enum GameSound
	{
		Init,
		Click,
		Drop,
		AddLine,
		Pop,
		GameOver,
	}

	private Dictionary<GameSound, AudioClip> audioClipDict;

	protected override void Awake()
	{
		base.Awake();

		audioSource = GetComponent<AudioSource>();

		// 인스펙터로 할당된 사운드클립들을 audioClipDict에 Add
		audioClipDict = new Dictionary<GameSound, AudioClip>();

		for (int i = 0; i < audioClips.Count; i++)
		{
			audioClipDict.Add((GameSound)i, audioClips[i]);
		}
	}

	// 사운드 재생
	public void PlaySound(GameSound type)
	{
		if (!OptionManager.Instance.OptionData.SoundOff)
		{
			audioSource.PlayOneShot(audioClipDict[type]);
		}
	}
}
