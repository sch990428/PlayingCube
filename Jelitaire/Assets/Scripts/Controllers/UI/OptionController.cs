using Data;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionController : Singleton<OptionController>
{
	[SerializeField]
	Transform UICanvas;

	Animator animator;

	public bool SoundOff;
	public bool SreenShakeOff;

	protected override void Awake()
	{
		animator = GetComponent<Animator>();
	}

	private void OnEnable()
	{
		animator.SetTrigger("FadeIn");
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
