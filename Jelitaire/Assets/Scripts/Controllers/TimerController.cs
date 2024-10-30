using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    void Update()
    {
        GetComponent<Image>().fillAmount = GameManager.Instance.timer / GameManager.Instance.timerInterval;
	}
}
