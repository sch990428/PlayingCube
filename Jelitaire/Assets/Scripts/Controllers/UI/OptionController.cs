using Data;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionController : Singleton<OptionController>
{
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
}
