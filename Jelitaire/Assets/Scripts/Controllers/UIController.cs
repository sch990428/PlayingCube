using System.Collections;
using TMPro;
using Unity.VisualScripting;
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
	private Transform BottomMenuUI;

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

		if (!PlayerPrefs.HasKey("Voxel"))
		{
			PlayerPrefs.SetInt("Voxel", 0);
		}

		Money = PlayerPrefs.GetInt("Voxel");
		MoneyText.text = Money.ToString();
	}

	public void OnPlayButtonClicked()
	{
		if (!isLoading)
		{
			StartCoroutine(SwitchToGameUI());
		}
	}

	public void OnLobbyButtonClicked(bool applyRecord)
	{
		if (!GameManager.Instance.isGenerating && !isLoading)
		{
			GameOverUI.gameObject.SetActive(false);
			StartCoroutine(SwitchToLobbyUI());

			if (applyRecord)
			{
				Data.GameMode mode = ModeSelectUI.GetComponent<ModeController>().GetMode();
				Money += GameManager.Instance.Score / mode.RewardRatio;
				MoneyText.text = Money.ToString();
				PlayerPrefs.SetInt("Voxel", Money);
			}
		}
	}

	private IEnumerator SwitchToGameUI()
	{
		// 로비 UI에서 게임 UI로 전환
		isLoading = true;
		
		StartCoroutine(FadeOutUIGroup(StaticLobbyUI));
		StartCoroutine(FadeOutUIGroup(ModeSelectUI));
		StartCoroutine(FadeOutUIGroup(BottomMenuUI));
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
		StartCoroutine(FadeInUIGroup(BottomMenuUI));
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

	// 게임 종료
	public void QuitGame()
	{
	#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
	#else
        Application.Quit();
	#endif
	}
}
