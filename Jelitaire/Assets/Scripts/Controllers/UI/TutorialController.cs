using UnityEngine;

public class TutorialController : MonoBehaviour
{
    int index;
	int maxIndex;

	private void OnEnable()
	{
		index = 0;
		maxIndex = 2;
	}

	public void Update()
	{
		for (int i = 0; i <= maxIndex; i++)
		{
			transform.GetChild(i).gameObject.SetActive(i == index);
		}
	}

	public void Next()
	{
		if (index <= maxIndex)
		{
			index++;
		}

		if (index == maxIndex + 1)
		{
			gameObject.SetActive(false);
		}
	}

	public void Previous()
	{
        if (index > 0)
        {
			index--;
		}
	}
}
