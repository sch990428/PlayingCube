using Data;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionController : MonoBehaviour
{
	[SerializeField]
	private Transform UICanvas;

	[SerializeField]
	private Toggle SFXOff;

	[SerializeField]
	private Toggle ScreenShakeOff;

	private Animator animator;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		
		// 기존 데이터 토글에 반영
		SFXOff.isOn = OptionManager.Instance.OptionData.SoundOff;
		ScreenShakeOff.isOn = OptionManager.Instance.OptionData.ScreenShakeOff;
	}

	private void OnEnable()
	{
		animator.SetTrigger("FadeIn");
	}

	public void ToggleSFX()
	{
		OptionManager.Instance.OptionData.SoundOff = SFXOff.isOn;
		OptionManager.Instance.SaveOptionData();
	}

	public void ToggleScreenShake()
	{
		OptionManager.Instance.OptionData.ScreenShakeOff = ScreenShakeOff.isOn;
		OptionManager.Instance.SaveOptionData();
	}

	public void CloseOption()
	{
		animator.SetTrigger("FadeOut");
		StartCoroutine(FadeOut());
	}

	public IEnumerator FadeOut()
	{
		yield return new WaitForSeconds(0.3f);
		gameObject.SetActive(false);
	}

	// 데이터 삭제 로직
	public void Reset()
	{
		ConfirmMessageController msg = ResourceManager.Instance.Instantiate("Prefabs/UI/ConfirmMessage", UICanvas).GetComponent<ConfirmMessageController>();
		msg.Init("모든 데이터가 사라집니다", () =>
		{
			Data.UserData UserData = new Data.UserData();
			string UserDataPath = Path.Combine(Application.persistentDataPath, "UserData.json");
			DataManager.Instance.SaveClassToJson<Data.UserData>(UserDataPath, UserData);
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
		});
	}
}
