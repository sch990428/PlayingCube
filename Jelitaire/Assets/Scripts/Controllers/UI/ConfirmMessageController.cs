using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmMessageController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text Message;

    public Button Accept;
	public Button Deny;

    public Action OnAccept;

	private void Awake()
	{
        Time.timeScale = 0f;
        Accept.onClick.AddListener(() => {
            if (OnAccept != null)
            {
                OnAccept.Invoke();
            }
        });
	}

	public void Init(string msg, Action onAccept)
    {
        Message.text = msg;
        OnAccept = onAccept;
    }
    
    public void Close()
    {
		Time.timeScale = 1f;
		ResourceManager.Instance.Destroy(gameObject);
    }
}
