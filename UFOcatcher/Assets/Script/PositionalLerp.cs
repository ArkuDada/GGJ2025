using UnityEngine;

public class PositionalLerp : MonoBehaviour
{
	public float lerp = 0.5f;
	public Vector3 destination = Vector3.zero;
	public bool destroyWhenDone = true;
	// Decrease scale to 0, then destroy
	public bool scaleDestroy = false;
	public float doneThreshold = 0.2f;
	public float scaleDestroyMultiplier = 0.5f;
	public float scaleDoneThreshold = 0.1f;

	private bool state_Scaling = false;

	void DestroyObj()
	{
		if (transform.parent != null && transform.TryGetComponent(out BaseObject baseObject))
		{
			baseObject.DespawnObject();
		}
		else
		{
			Destroy(gameObject);
		}
	}

	void FixedUpdate()
	{
		if (!state_Scaling)
		{
			// Moving state
			transform.position = Vector3.Lerp(transform.position, destination, lerp);
			if (Vector3.Distance(transform.position, destination) <= doneThreshold)
			{
				if (scaleDestroy)
				{
					state_Scaling = true;
				}
				else
				{
					DestroyObj();
				}
			}
		}
		else
		{
			// Scaling state
			transform.localScale *= scaleDestroyMultiplier;
			if (transform.localScale.x <= scaleDoneThreshold) {
				DestroyObj();
			}
		}
	}
}
