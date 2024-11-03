using UnityEngine;
using UnityEngine.UI;

public class ComboUIController : MonoBehaviour
{
    Image image;

	private void Awake()
	{
		image = GetComponent<Image>();
	}

	void Update()
    {
		Color32 color = image.color;
        color.a = (byte)(Mathf.PingPong(Time.time * 100, 205) + 50);

		image.color = color;
    }
}
