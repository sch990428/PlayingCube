using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmMessageController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text Message;

    public Button Accept;
	public Button Deny;

	private void Awake()
	{
        Time.timeScale = 0f;
	}

	public void Init(string msg)
    {
        Message.text = msg;
    }
    
    public void Close()
    {
		Time.timeScale = 1f;
		ResourceManager.Instance.Destroy(gameObject);
    }
}
