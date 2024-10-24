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

	private float switchTerm = 0.4f;

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
		StartCoroutine(FadeOutUIGroup(StaticLobbyUI));
		StartCoroutine(FadeOutUIGroup(ModeSelectUI));
		StartCoroutine(SlideDownBackgrounds());

		yield return new WaitForSeconds(1f);

		StartCoroutine(FadeInUIGroup(GameUI));
	}

	private IEnumerator SwitchToLobbyUI()
	{
		StartCoroutine(FadeOutUIGroup(GameUI));

		yield return new WaitForSeconds(1f);

		StartCoroutine(FadeInUIGroup(StaticLobbyUI));
		StartCoroutine(FadeInUIGroup(ModeSelectUI));
		StartCoroutine(SlideUpBackgrounds());
	}

	private IEnumerator FadeOutUIGroup(Transform t)
	{
		Animator animator = t.GetComponent<Animator>();
		animator.SetTrigger("FadeOut");
		yield return new WaitForSeconds(switchTerm);
		t.gameObject.SetActive(false);
		animator.ResetTrigger("FadeOut");
	}

	private IEnumerator FadeInUIGroup(Transform t)
	{
		t.gameObject.SetActive(true);
		Animator animator = t.GetComponent<Animator>();
		animator.SetTrigger("FadeIn");
		yield return new WaitForSeconds(switchTerm);
		animator.ResetTrigger("FadeIn");
	}

	private IEnumerator SlideDownBackgrounds()
	{
		Animator animator = Backgrounds.GetComponent<Animator>();
		animator.SetTrigger("SlideDown");
		yield return new WaitForSeconds(0.5f);
		Backgrounds.gameObject.SetActive(false);
		animator.ResetTrigger("SlideDown");
	}

	private IEnumerator SlideUpBackgrounds()
	{
		Backgrounds.gameObject.SetActive(true);
		Animator animator = Backgrounds.GetComponent<Animator>();
		animator.SetTrigger("SlideUp");
		yield return new WaitForSeconds(0.5f);	
		animator.ResetTrigger("SlideUp");
	}
}
