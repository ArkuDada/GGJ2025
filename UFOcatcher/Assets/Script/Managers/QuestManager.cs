using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

public class QuestManager : MonoBehaviour
{
	public List<RecipeObject> availableRecipes; // Quests that could be chosen as currentQuest

	public RecipeObject
		CurrentQuest
	{ get; private set; } // The current quest, containing a list of objects for the player to collect

	public List<int> ObjectsCollected { get; private set; } // Acts like a checklist for the current quest's items
	public int ObjectsLeftToCollect { get; private set; } // Number of items left to collect

	public Arcade arcade;

	public GameObject correctObjectParticles;
	public Material bubbleMaterial;

	[SerializeField]
	ScoreManager scoreManager;

	public void Start()
	{
		if (arcade == null)
		{
			GameObject arcadeGameObject = GameObject.Find("Arcade");
			if (!arcadeGameObject)
				Debug.LogError("Arcade not found!");

			arcade = arcadeGameObject.GetComponent<Arcade>();
			if (!arcade)
				Debug.Log("Arcade has no Arcade script!");
		}

		StartCoroutine(InitQuest());

	}

	// Choose a new random quest for the player to complete
	private IEnumerator InitQuest()
	{
		SoundManager.instance.PlaySFX("Quest Ping");
		CurrentQuest = availableRecipes[Random.Range(0, availableRecipes.Count)];

		ObjectsLeftToCollect = 0;
		ObjectsCollected = new(CurrentQuest.objects.Count);
		for (int i = 0; i < ObjectsCollected.Capacity; i++)
		{
			ObjectsLeftToCollect += CurrentQuest.quantities[i];
			ObjectsCollected.Add(0);
		}

		for (int i = 0; i < 4; i++)
		{
			yield return new WaitForSeconds(plopTime);
			arcade.UnsetButtonIcon(i);
			arcade.SetBorderFill(i, 0);
		}

		for (int i = 0; i < ObjectsCollected.Count; i++)
		{
			yield return new WaitForSeconds(plopTime);
			arcade.SetButtonIcon(i, CurrentQuest.objects[i]);
		}

		UpdateQuestUI();
	}

	private void UpdateQuestUI()
	{
		for (int i = 0; i < ObjectsCollected.Count; i++)
		{
			arcade.SetBorderFill(i, (float)ObjectsCollected[i] / (float)CurrentQuest.quantities[i]);
		}
	}

	// Handler for when currentQuest is completed
	private IEnumerator QuestCompleted()
	{
		SoundManager.instance.PlaySFX("Quest Complete");
		scoreManager.CompletedQuest();

		yield return new WaitForSeconds(blinkTime);

		for (int j = 0; j < 3; j++)
		{
			for (int i = 0; i < 4; i++)
			{
				arcade.SetBorderFill(i, 1);
			}

			yield return new WaitForSeconds(blinkTime);
			for (int i = 0; i < 4; i++)
			{
				arcade.SetBorderFill(i, 0);
			}

			yield return new WaitForSeconds(blinkTime);
		}

		for (int i = 0; i < 4; i++)
		{
			arcade.SetButtonIcon(i, Objects.ObjectType.End);
			yield return new WaitForSeconds(plopTime);

		}

		StartCoroutine(InitQuest());
	}

	[SerializeField] private float blinkTime = 0.25f;
	[SerializeField] private float plopTime = 0.15f;
	// Mark an object as collected if it's on the current quest's list.
	public void CollectedObject(BaseObject objectCollected)
	{
		for (int i = 0; i < CurrentQuest.objects.Count; ++i)
		{
			if (CurrentQuest.objects[i] == objectCollected.Type && ObjectsCollected[i] < CurrentQuest.quantities[i])
			{
				// Collected a correct object
				++ObjectsCollected[i];
				--ObjectsLeftToCollect;

				if (ObjectsLeftToCollect <= 0)
				{
					StartCoroutine(QuestCompleted());
				}
				else
				{
					UpdateQuestUI();
				}
				SoundManager.instance.PlaySFX("Collect Correct");
				scoreManager.IncrementScore(4000);

				GameObject newParticles = Instantiate(correctObjectParticles);
				Vector3 worldToMainCameraPos = GameObject.FindWithTag("Player").transform.position - GameObject.Find("Main Camera").transform.position;
				Vector3 mainCameraPosToArcadeCameraPos = GameObject.Find("Arcade Camera").transform.position + worldToMainCameraPos;
				newParticles.transform.position = objectCollected.transform.position;
				newParticles.GetComponent<ParentTransform>().following = objectCollected.transform;

				objectCollected.transform.position = mainCameraPosToArcadeCameraPos;
				var positionalLerp = objectCollected.AddComponent<PositionalLerp>();
				positionalLerp.destination = arcade.mesh[i].IconMesh.transform.position;
				positionalLerp.lerp = 0.03f;
				positionalLerp.doneThreshold = 0.5f;
				positionalLerp.scaleDestroy = true;
				
				Rigidbody objectRig = objectCollected.GetComponent<Rigidbody>();
				objectRig.isKinematic = false;
				objectRig.constraints = RigidbodyConstraints.FreezePosition;
				objectRig.angularVelocity = Vector3.one * -5f;

				var meshRenderers = objectCollected.transform.GetComponentsInChildren<MeshRenderer>();
				if (objectCollected.TryGetComponent(out MeshRenderer objectMeshRenderer)) {
					meshRenderers.Append(objectMeshRenderer);
				}
				foreach (var meshRenderer in meshRenderers)
				{
					meshRenderer.renderingLayerMask = 2;
					meshRenderer.material = bubbleMaterial;
				}

				var colliders = objectCollected.transform.GetComponentsInChildren<Collider>();
				if (objectCollected.TryGetComponent(out Collider objectCollider))
				{
					colliders.Append(objectCollider);
				}
				foreach (var collider in colliders)
				{
					collider.enabled = false;
				}
				objectCollected._bubble.SetActive(false);

				return;
			}
		}

		// This object wasn't correct
		SoundManager.instance.PlaySFX("Collect Wrong");
		scoreManager.IncrementScore(1000);
		objectCollected.DespawnObject();
	}
}