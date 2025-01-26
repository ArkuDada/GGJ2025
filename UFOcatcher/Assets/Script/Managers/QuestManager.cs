using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
	public List<RecipeObject> availableRecipes; // Quests that could be chosen as currentQuest
	public RecipeObject CurrentQuest { get; private set; } // The current quest, containing a list of objects for the player to collect
	public List<int> ObjectsCollected { get; private set; } // Acts like a checklist for the current quest's items
	public int ObjectsLeftToCollect { get; private set; } // Number of items left to collect
	public TextMeshProUGUI tempQuestText; // temp for debug until UI is finalized

	public void Start()
	{
		InitQuest();
	}

	// Choose a new random quest for the player to complete
	private void InitQuest()
	{
		CurrentQuest = availableRecipes[Random.Range(0, availableRecipes.Count)];

		ObjectsCollected = new(CurrentQuest.objects.Count);
		for (int i = 0; i < ObjectsCollected.Capacity; i++)
		{
			ObjectsLeftToCollect = CurrentQuest.quantities[i];
			ObjectsCollected.Add(0);
		}


		UpdateQuestUI();
	}

	private void UpdateQuestUI()
	{
		string text = CurrentQuest.questName + ':';
		for (int i = 0; i < ObjectsCollected.Count; i++)
		{
			text += string.Format("\n {0}: {1}/{2}", 
				Utility.Objects.GetObjectName(CurrentQuest.objects[i]),
				ObjectsCollected[i],
				CurrentQuest.quantities[i]
				);
		}
		tempQuestText.text = text;
	}

	// Handler for when currentQuest is completed
	private void QuestCompleted()
	{
		ScoreManager scoreManager = GameManager.Instance.ScoreManager;
		scoreManager.IncrementScore(1000); // Magic number: I asked designers for this number, and they said 1000 :)

		InitQuest();
	}

	// Mark an object as collected if it's on the current quest's list.
	public void CollectedObject(BaseObject objectCollected)
	{
		Debug.Log($"Collected {objectCollected.Type}");
		ScoreManager scoreManager = GameManager.Instance.ScoreManager;
		int points = objectCollected.GetScore();

		for (int i = 0; i < CurrentQuest.objects.Count; ++i)
		{
			Debug.Log(CurrentQuest.objects[i]);
			Debug.Log(ObjectsCollected[i]);
			Debug.Log(CurrentQuest.quantities[i]);
			if (CurrentQuest.objects[i] == objectCollected.Type && ObjectsCollected[i] < CurrentQuest.quantities[i])
			{
				Debug.Log("Got!");
				// Collected an uncollected object on our list
				Debug.Log(ObjectsCollected[i]);
				Debug.Log(ObjectsLeftToCollect);
				++ObjectsCollected[i];
				--ObjectsLeftToCollect;
				Debug.Log(ObjectsCollected[i]);
				Debug.Log(ObjectsLeftToCollect);

				if (ObjectsLeftToCollect <= 0)
				{
					QuestCompleted();
				}
				else
				{
					UpdateQuestUI();
				}

				scoreManager.IncrementScore(points);

				return;
			}
		}

		// Object wasn't on the quest list
		scoreManager.DecrementScore(points);
	}
}
