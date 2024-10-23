using TMPro;
using UnityEngine;

public class Jelly : MonoBehaviour
{
	[SerializeField]
	TMP_Text NumberText;
	public GameManager gameManager;

	public bool isMoving;

	public Define.JellyType Type;
	public int Number;

	public Jelly Parent;
	public Jelly Child;

	private Renderer jellyRenderer;
	private Vector3 prevPosition;

	private void Awake()
	{
		jellyRenderer = GetComponent<Renderer>();
	}

	private void OnEnable()
	{
		InputManager.Instance.OnTouchPressed += TouchPressed;
		InputManager.Instance.OnTouching += Touching;
		InputManager.Instance.OnTouchReleased += TouchReleased;
	}

	private void OnDisable()
	{
		if (InputManager.hasInstance())
		{
			InputManager.Instance.OnTouchPressed -= TouchPressed;
			InputManager.Instance.OnTouching -= Touching;
			InputManager.Instance.OnTouchReleased -= TouchReleased;
		}
	}

	public void UpdatePos(Vector3 pos)
	{
		transform.position = new Vector3(pos.x, pos.y, pos.z);
	}

	public void ChangeType(Define.JellyType t)
	{
		Type = t;
		jellyRenderer.material.color = Define.JellyColor[t];
		NumberText.text = $"{Number}";
	}

	private void TouchPressed(Jelly selectedJelly)
	{
		if (selectedJelly == this && IsHierarchy())
		{
			isMoving = true;
			gameManager.anyJellyMoving = true;
			prevPosition = transform.position;
		}
	}

	private void Touching(Vector3 pos)
	{
		if (isMoving)
		{
			transform.position = pos + new Vector3(0, 0.25f, 0);
		}
	}

	private void TouchReleased(Vector3 pos)
	{
		if (isMoving)
		{
			isMoving = false;
			gameManager.anyJellyMoving = false;

			this.gameObject.GetComponent<Collider>().enabled = false;

			RaycastHit hit;

			if (Physics.Raycast(transform.position, Vector3.forward, out hit, 1f))
			{
				if (hit.collider.CompareTag("JellyEntity"))
				{
					if (hit.collider.name.Equals("LineRoot"))
					{
						if (hit.collider.transform.childCount == 0)
						{
							UpdatePos(hit.collider.transform.position + new Vector3(0, 0, -0.75f));
							if (Parent != null)
							{
								Parent.Child = null;
							}
							Parent = null;
							transform.SetParent(hit.collider.transform, true);
							gameObject.GetComponent<Collider>().enabled = true;
							gameManager.OnJellyChanged();
							return;
						}
					}
					else
					{
						Jelly parentJelly = hit.collider.GetComponent<Jelly>();
						if (parentJelly.Number == Number - 1 && parentJelly.Child == null)
						{
							UpdatePos(hit.collider.transform.position + new Vector3(0, 0, -1f));
							if (Parent != null)
							{
								Parent.Child = null;
							}
							Parent = parentJelly;
							parentJelly.Child = this;
							transform.SetParent(hit.collider.transform, true);
							gameObject.GetComponent<Collider>().enabled = true;
							gameManager.OnJellyChanged();
							return;
						}
					}
				}
			}

			gameObject.GetComponent<Collider>().enabled = true;
			UpdatePos(prevPosition);
		}
	}

	public bool IsHierarchy()
	{
		if (Child == null)
		{
			return true;
		}
		else
		{
			if (Child.Number == Number + 1 && Child.Type == Type)
			{
				return Child.IsHierarchy();
			}
			else
			{
				return false;
			}
		}
	}

	public bool IsHierarchyReverse()
	{
		if (Number == 1)
		{
			return true;
		}

		if (Parent == null)
		{
			return false;
		}
		else
		{
			if (Parent.Number == Number - 1 && Parent.Type == Type)
			{
				return Parent.IsHierarchyReverse();
			}
			else
			{
				return false;
			}
		}
	}

	public void Pop(int i)
	{
		if (Number == 1)
		{
			if (Parent != null)
			{
				Parent.Child = null;
			}
		}
		else
		{	
			Parent.Pop(i);
		}

		ResourceManager.Instance.Destroy(gameObject);
	}
}
