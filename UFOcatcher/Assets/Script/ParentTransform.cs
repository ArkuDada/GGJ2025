using Unity.VisualScripting;
using UnityEngine;

public class ParentTransform : MonoBehaviour
{
	public Transform following;
	public bool destroyIfParentDestroyed = true;

	void Update()
	{
		if (following != null && !following.gameObject.IsDestroyed())
		{
			transform.position = following.position;
		}
		else if(destroyIfParentDestroyed) {
			Destroy(gameObject);
		}
	}
}
