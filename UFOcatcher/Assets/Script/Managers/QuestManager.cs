using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
	public List<ObjectList> availableQuests; // Quests that could be chosen as currentQuest
	public ObjectList CurrentQuest { get; private set; } // The current quest, containing a list of objects for the player to collect
	public List<bool> ObjectsCollected { get; private set; } // Acts like a checklist for the current quest's items
	public int ObjectsLeftToCollect { get; private set; } // Number of items left to collect
	public TextMeshProUGUI tempQuestText; // temp for debug until UI is finalized

	public void Start()
	{
		InitQuest();
	}

	// Choose a new random quest for the player to complete
	private void InitQuest()
	{
		CurrentQuest = availableQuests[Random.Range(0, availableQuests.Count)];

		ObjectsCollected = new(CurrentQuest.objects.Count);
		for (int i = 0; i < ObjectsCollected.Capacity; i++)
		{
			ObjectsCollected.Add(false);
		}

		ObjectsLeftToCollect = ObjectsCollected.Count;

		TempUpdateQuestUI();
	}

	// Temp until UI is finalized
	private void TempUpdateQuestUI()
	{
		string text = CurrentQuest.questName + ':';
		for (int i = 0; i < ObjectsCollected.Count; i++)
		{
			if (!ObjectsCollected[i])
			{
				text += '\n' + Utility.Objects.GetObjectName(CurrentQuest.objects[i]);
			}
		}
		tempQuestText.text = text;
	}

	// Handler for when currentQuest is completed
	private void QuestCompleted()
	{
		// TODO: Talk to game manager?

		InitQuest();
	}

	// Mark an object as collected if it's on the current quest's list.
	public void CollectedObject(BaseObject objectCollected)
	{
		ScoreManager scoreManager = GameManager.Instance.ScoreManager;
		int points = objectCollected.GetScore();

		for (int i = 0; i < CurrentQuest.objects.Count; ++i)
		{
			if (CurrentQuest.objects[i] == objectCollected.Type && !ObjectsCollected[i])
			{
				// Collected an uncollected object on our list
				ObjectsCollected[i] = true;
				--ObjectsLeftToCollect;

				if (ObjectsLeftToCollect <= 0)
				{
					QuestCompleted();
				}
				else
				{
					TempUpdateQuestUI();
				}

				scoreManager.IncrementScore(points);
			}

			return;
		}

		// Object wasn't on the quest list
		scoreManager.DecrementScore(points);
	}
}
