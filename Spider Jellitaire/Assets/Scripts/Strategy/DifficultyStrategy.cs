using UnityEngine;

public interface IDifficulty
{
	public GameObject CreateNewJelly();
}

public class EasyStrategy : IDifficulty
{
	public GameObject CreateNewJelly()
	{
		GameObject newJelly = ResourceManager.Instance.Instantiate("Prefabs/GameEntity/Jelly");
		Jelly j = newJelly.GetComponent<Jelly>();
		j.Number = Random.Range(1, 6);
		j.ChangeType(Define.JellyType.Blue);

		return newJelly;
	}
}

public class NormalStrategy : IDifficulty
{
	public GameObject CreateNewJelly()
	{
		GameObject newJelly = ResourceManager.Instance.Instantiate("Prefabs/GameEntity/Jelly");
		Jelly j = newJelly.GetComponent<Jelly>();
		j.Number = Random.Range(1, 6);
		j.ChangeType((Define.JellyType)Random.Range(1, 3));

		return newJelly;
	}
}

public class HardStrategy : IDifficulty
{
	public GameObject CreateNewJelly()
	{
		GameObject newJelly = ResourceManager.Instance.Instantiate("Prefabs/GameEntity/Jelly");
		Jelly j = newJelly.GetComponent<Jelly>();
		j.Number = Random.Range(1, 6);
		j.ChangeType((Define.JellyType)Random.Range(1, 5));

		return newJelly;
	}
}
