using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmMessageController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text Message;

    public Button Accept;
	public Button Deny;

	public void Init(string msg)
    {
        Message.text = msg;
    }
    
    public void Close()
    {
        ResourceManager.Instance.Destroy(gameObject);
    }
}
