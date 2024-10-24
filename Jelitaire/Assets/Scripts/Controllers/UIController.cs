using System.Collections;
using UnityEngine;

public class UIController : MonoBehaviour
{
	[SerializeField]
	private Transform Backgrounds;

	[SerializeField]
	private Transform StaticLobbyUI;

	[SerializeField]
	private Transform ModeSelectUI;

	[SerializeField]
	private Transform GameUI;

	private float switchTerm = 0.5f; // UI을 전환하는 간격

	public void OnPlayButtonClicked()
	{
		StartCoroutine(SwitchToGameUI());
	}

	public void OnPauseButtonClicked()
	{
		StartCoroutine(SwitchToLobbyUI());
	}

	private IEnumerator SwitchToGameUI()
	{
		// 로비 UI에서 게임 UI로 전환
		StartCoroutine(FadeOutUIGroup(StaticLobbyUI));
		StartCoroutine(FadeOutUIGroup(ModeSelectUI));
		StartCoroutine(SlideDownBackgrounds());

		yield return new WaitForSeconds(switchTerm);

		StartCoroutine(FadeInUIGroup(GameUI));
	}

	private IEnumerator SwitchToLobbyUI()
	{
		// 게임 UI에서 로비 UI로 전환
		StartCoroutine(FadeOutUIGroup(GameUI));

		yield return new WaitForSeconds(switchTerm);

		StartCoroutine(FadeInUIGroup(StaticLobbyUI));
		StartCoroutine(FadeInUIGroup(ModeSelectUI));
		StartCoroutine(SlideUpBackgrounds());
	}

	private IEnumerator FadeOutUIGroup(Transform t)
	{
		// 서서히 사라지는 Fade Out효과
		Animator animator = t.GetComponent<Animator>();
		animator.SetTrigger("FadeOut");
		yield return new WaitForSeconds(0.5f);
		t.gameObject.SetActive(false);
		animator.ResetTrigger("FadeOut");
	}

	private IEnumerator FadeInUIGroup(Transform t)
	{
		// 서서히 나타나는 Fade In효과
		t.gameObject.SetActive(true);
		Animator animator = t.GetComponent<Animator>();
		animator.SetTrigger("FadeIn");
		yield return new WaitForSeconds(0.5f);
		animator.ResetTrigger("FadeIn");
	}

	private IEnumerator SlideDownBackgrounds()
	{
		// 배경이 서서히 내려가는 Slide Down효과
		Animator animator = Backgrounds.GetComponent<Animator>();
		animator.SetTrigger("SlideDown");
		yield return new WaitForSeconds(0.5f);
		Backgrounds.gameObject.SetActive(false);
		animator.ResetTrigger("SlideDown");
	}

	private IEnumerator SlideUpBackgrounds()
	{
		// 배경이 서서히 올라오는 Slide Up효과
		Backgrounds.gameObject.SetActive(true);
		Animator animator = Backgrounds.GetComponent<Animator>();
		animator.SetTrigger("SlideUp");
		yield return new WaitForSeconds(0.5f);	
		animator.ResetTrigger("SlideUp");
	}
}
