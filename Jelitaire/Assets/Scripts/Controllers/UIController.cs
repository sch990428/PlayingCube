using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

	[SerializeField]
	public Button GameOverUI;

	[SerializeField]
	private TMP_Text MoneyText;

	[SerializeField]
	private TMP_Text ScoreText;

	private float switchTerm = 1f; // UI을 전환하는 간격
	private bool isLoading = false; // UI를 전환하는 도중인가?

	public int Money = 0;

	private void Awake()
	{
		GameManager.Instance.OnScoreChanged -= ScoreUpdate;
		GameManager.Instance.OnScoreChanged += ScoreUpdate;
		MoneyText.text = Money.ToString();
	}

	public void OnPlayButtonClicked()
	{
		if (!isLoading)
		{
			StartCoroutine(SwitchToGameUI());
		}
	}

	public void OnLobbyButtonClicked()
	{
		if (!GameManager.Instance.isGenerating && !isLoading)
		{
			GameOverUI.gameObject.SetActive(false);
			StartCoroutine(SwitchToLobbyUI());

			Money += GameManager.Instance.Score;
			MoneyText.text = Money.ToString();
		}
	}

	private IEnumerator SwitchToGameUI()
	{
		// 로비 UI에서 게임 UI로 전환
		isLoading = true;
		
		StartCoroutine(FadeOutUIGroup(StaticLobbyUI));
		StartCoroutine(FadeOutUIGroup(ModeSelectUI));
		StartCoroutine(SlideDownBackgrounds());

		GameManager.Instance.State = GameManager.GameState.Init;
		yield return new WaitForSeconds(switchTerm);

		yield return FadeInUIGroup(GameUI);
		isLoading = false;
	}

	private IEnumerator SwitchToLobbyUI()
	{
		// 게임 UI에서 로비 UI로 전환
		isLoading = true;
		StartCoroutine(FadeOutUIGroup(GameUI));
		GameManager.Instance.State = GameManager.GameState.Exit;
		yield return new WaitForSeconds(switchTerm);

		StartCoroutine(FadeInUIGroup(StaticLobbyUI));
		StartCoroutine(FadeInUIGroup(ModeSelectUI));
		StartCoroutine(SlideUpBackgrounds());

		yield return SlideUpBackgrounds();
		isLoading = false;
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

	private void ScoreUpdate()
	{
		ScoreText.text = GameManager.Instance.Score.ToString();
	}
}
