using UnityEngine;
using System.Collections;

public abstract class VRCInteractable : MonoBehaviour
{
	protected RaycastHit _currentHit;
	protected bool _isSelected = false;
	public bool isSelected 
	{ 
		get { return _isSelected; }
	}

	public Transform interactTextPlacement;
	public string interactText = "Use";
	protected GameObject interactTextGO;

	public float proximity = 2.0f;

	public virtual void Awake()
	{
#if VRC_CLIENT
		gameObject.layer =  LayerMask.NameToLayer("Interactive");
#endif
	}

	public virtual void Start()
	{
#if VRC_CLIENT
		if(interactTextPlacement != null)
		{
			GameObject prefab = Resources.Load<GameObject>("UseText");
			interactTextGO = (GameObject)Instantiate(prefab);
			interactTextGO.transform.parent = gameObject.transform;
			interactTextGO.transform.position = Vector3.zero;
			interactTextGO.transform.localPosition = Vector3.zero;
			TextMesh textMesh = interactTextGO.transform.FindChild("TextMesh").GetComponent<TextMesh>();
			textMesh.text = interactText;
		}
#endif
	}

	public virtual void Select(RaycastHit hit)
	{
		if(hit.distance <= proximity)
		{
			_isSelected = true;
			_currentHit = hit;
			if(interactTextGO != null) 
			{
				interactTextGO.SetActive(true);
			}
		}
	}
	
	public virtual void Deselect()
	{
		if(_isSelected)
		{
			if(interactTextGO != null) 
			{
				interactTextGO.SetActive(false);
			}
			_currentHit = new RaycastHit();
			_isSelected = false;
		}
	}

}
