using NCC.Utility.Objects;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MoveableObject))]
public class CowData : MonoBehaviour
{
	public WheatData targetWheat = null; // Used to cancel eating of wheat when abducted
	public IEnumerator wheatEatingCoroutine = null;
	private MoveableObject moveableObject;
	private const float COW_EAT_TIME = 5.0f;
	private const float COW_EAT_PARTICLE_VERT_OFFSET = 1.0f;
	public GameObject wheatEatingParticle = null;

	public bool isEating => wheatEatingCoroutine != null;

	public void Start()
	{
		moveableObject = GetComponent<MoveableObject>();
	}

	public void StopEating()
	{
		if (!isEating)
			return;

		if (wheatEatingCoroutine != null)
		{
			StopCoroutine(wheatEatingCoroutine);
			wheatEatingCoroutine = null;
		}

		Destroy(wheatEatingParticle);
		moveableObject.ChooseNewDestination();
		targetWheat.cowEating = null;
		wheatEatingParticle = null;
		targetWheat.GetComponent<Rigidbody>().isKinematic = false;
	}

	private void OnDestroy()
	{
		StopEating();
	}

	public void StartEating(GrowingObject wheat)
	{
		if (isEating || moveableObject._base.State != BaseObject.ObjectState.Grounded || wheat.State != BaseObject.ObjectState.Grounded)
			return;

		wheatEatingCoroutine = CowEat(wheat);
		StartCoroutine(wheatEatingCoroutine);
	}

	private IEnumerator CowEat(GrowingObject wheat)
	{
		// Ignore wheat already being eaten by another cow
		if (targetWheat.beingEaten)
		{
			moveableObject.ChooseNewDestination();
			yield break;
		}

		// Prevent wheat from moving away during the animation
		wheat.GetComponent<Rigidbody>().isKinematic = true;

		wheatEatingParticle = Instantiate(moveableObject._surface.transform.Find("EatingWheatParticles").gameObject);
		wheatEatingParticle.transform.position = targetWheat.transform.position + Vector3.up * COW_EAT_PARTICLE_VERT_OFFSET;
		wheatEatingParticle.SetActive(true);
		Destroy(wheatEatingParticle, COW_EAT_TIME);

		yield return new WaitForSeconds(COW_EAT_TIME);

		wheat.DestroyObject();
		wheatEatingCoroutine = null;
		StopEating();
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.TryGetComponent<GrowingObject>(out var growingObject))
		{
			WheatData wheatData = growingObject.GetComponent<WheatData>();
			// Collided with wheat, not currently eating
			if (growingObject.Type == Utility.Objects.ObjectType.Wheat && wheatData == targetWheat)
			{
				StartEating(growingObject);
			}
		}
	}
}
