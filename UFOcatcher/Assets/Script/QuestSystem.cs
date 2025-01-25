using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Utility;

public class QuestSystem : MonoSingleton<QuestSystem>
{
	public List<ObjectList> availableQuests; // Quests that could be chosen as currentQuest
	public ObjectList currentQuest { get; private set; } // The current quest, containing a list of objects for the player to collect
	public List<bool> objectsCollected { get; private set; } // Acts like a checklist for the current quest's items
	public int objectsLeftToCollect { get; private set; } // Number of items left to collect
	public TextMeshProUGUI tempQuestText; // temp for debug until UI is finalized

	protected override void Init()
	{
	}

	public void Start()
	{
		InitQuest();
	}

	public void Update()
	{
	}

	// Choose a new random quest for the player to complete
	private void InitQuest()
	{
		currentQuest = availableQuests[Random.Range(0, availableQuests.Count)];

		objectsCollected = new(currentQuest.objects.Count);
		for (int i = 0; i < objectsCollected.Capacity; i++)
		{
			objectsCollected.Add(false);
		}

		objectsLeftToCollect = objectsCollected.Count;

		TempUpdateQuestUI();
	}

	// Temp until UI is finalized
	private void TempUpdateQuestUI()
	{
		string text = currentQuest.questName + ':';
		for (int i = 0; i < objectsCollected.Count; i++)
		{
			if (!objectsCollected[i])
			{
				text += '\n' + Objects.GetObjectName(currentQuest.objects[i]);
			}
		}
		tempQuestText.text = text;
	}

	// Handler for when currentQuest is completed
	private void QuestCompleted()
	{
		// TODO: Talk to game manager

		InitQuest();
	}

	// Mark an object as collected if it's on the current quest's lsit.
	public void CollectedObject(Objects.ObjectType itemCompleted)
	{
		for (int i = 0; i < currentQuest.objects.Count; ++i)
		{
			if (currentQuest.objects[i] == itemCompleted && !objectsCollected[i])
			{
				// Collected an uncollected item on our list
				objectsCollected[i] = true;
				--objectsLeftToCollect;
			}
		}

		if (objectsLeftToCollect <= 0)
		{
			QuestCompleted();
		}

		TempUpdateQuestUI();
	}
}
