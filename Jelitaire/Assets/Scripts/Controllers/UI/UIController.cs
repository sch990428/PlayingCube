using System.Collections;
using System.IO;
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
	private Transform BottomMenuUI;

	[SerializeField]
	private Transform GameUI;

	[SerializeField]
	public Button GameOverUI;

	[SerializeField]
	private TMP_Text MoneyText;

	[SerializeField]
	private TMP_Text ScoreText;

	[SerializeField]
	private Transform ComboGroup;

	private float switchTerm = 1f; // UI을 전환하는 간격
	private bool isLoading = false; // UI를 전환하는 도중인가?

	public string UserDataPath;
	public Data.UserData UserData;

	private void Awake()
	{
		GameManager.Instance.OnScoreChanged -= ScoreUpdate;
		GameManager.Instance.OnScoreChanged += ScoreUpdate;

		UserDataPath = Path.Combine(Application.persistentDataPath, "UserData.json");
		UserData = DataManager.Instance.LoadJsonToClass<Data.UserData>(UserDataPath);

		if (UserData == null)
		{
			UserData = new Data.UserData();
			DataManager.Instance.SaveClassToJson<Data.UserData>(UserDataPath, UserData);
		}

		MoneyText.text = UserData.Money.ToString();
	}

	public void OnPlayButtonClicked(bool timeattack)
	{
		GameManager.Instance.isTimeAttack = timeattack;
		if (!isLoading)
		{
			StartCoroutine(SwitchToGameUI());
		}
	}

	public void OnLobbyButtonClicked(bool applyRecord)
	{
		if (!GameManager.Instance.isGenerating && !isLoading)
		{
			if (applyRecord)
			{
				int modeId = ModeSelectUI.GetComponent<ModeController>().GetMode().ID;
				if (GameManager.Instance.Score > UserData.HighScores[modeId])
				{
					UserData.HighScores[modeId] = GameManager.Instance.Score;
				}

				float rewardRatio = ModeSelectUI.GetComponent<ModeController>().GetMode().RewardRatio;
				if (!GameManager.Instance.isTimeAttack)
				{
					rewardRatio = 0;
				}
				UserData.Money += (int)(GameManager.Instance.Score * rewardRatio);
				MoneyText.text = UserData.Money.ToString();

				SaveUserData();
				GameOverUI.gameObject.SetActive(false);
				StartCoroutine(SwitchToLobbyUI());
			}
			else
			{
				ConfirmMessageController msg = ResourceManager.Instance.Instantiate("Prefabs/UI/ConfirmMessage", transform).GetComponent<ConfirmMessageController>();
				msg.Init("진행중인 게임은 저장되지 않습니다.", () =>
				{
					Time.timeScale = 1.0f;
					GameOverUI.gameObject.SetActive(false);
					StartCoroutine(SwitchToLobbyUI());
					ResourceManager.Instance.Destroy(msg.gameObject);
				});
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

	public void SaveUserData()
	{
		DataManager.Instance.SaveClassToJson<Data.UserData>(UserDataPath, UserData);
	}

	// 콤보에 따라서 별모양 활성화
	public void ComboUpdate(int combo)
	{
		// Debug.Log($"콤보 {combo}");

		for (int i = 0; i < ComboGroup.childCount; i++)
		{
			ComboGroup.GetChild(i).gameObject.SetActive(i < combo);
		}
	}

	// 게임 종료
	public void QuitGame()
	{
		SaveUserData();
	#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
	#else
        Application.Quit();
	#endif
	}
}
