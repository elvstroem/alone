using System;
using System.Collections.Generic;
using System.Linq;
using engine;

namespace world
{
	public class quests
	{
		public static readonly List<Quest> Quests = new List<Quest>();

		// quests
		public const int QUEST_ID_START_QUEST = 1;
		public const int QUEST_ID_CLEAR_FORREST = 2;
		public const int QUEST_ID_CLEAR_PASSAGE = 3;
		public const int QUEST_ID_CLEAR_GRAVEYARD = 4;

		// metod för att skapa quester
		// Skapa först questen "Quest namnPåQuest" och fyll på med tillhörande properties.
		// Lägg till om man behöver något item för att klara questen.
		// Lägg till om man får någon reward av questen
		// Slutför med att lägga till questerna i listan "Quests"
		public static void Populate()
		{
			Quest startQuest =
				new Quest (
					QUEST_ID_START_QUEST,
					"Make your way to the gate",
					"All hope is lost! Only hope for salvation is if you make it to the gate!", 10, 10);

			startQuest.QuestCompletionItems.Add (new QuestCompletionItem (items.ItemByID (items.ITEM_ID_ROTTING_FLESH), 1));
			startQuest.RewardItem = items.ItemByID (items.ITEM_ID_HEALING_POTION);

			Quest clearForrest =
				new Quest (
					QUEST_ID_CLEAR_FORREST,
					"Clear the forrest of wolfs",
					"Kill of the wolfs and bring back 3 pelts", 10, 10);

			clearForrest.QuestCompletionItems.Add (new QuestCompletionItem (items.ItemByID (items.ITEM_ID_SHARP_FANG), 3));
			clearForrest.RewardItem = items.ItemByID (items.ITEM_ID_MULTI_PASS);

			Quest clearPassage =
				new Quest (
					QUEST_ID_CLEAR_PASSAGE,
					"Clear the passage of zombies",
					"The zombies are roaming free on the passage! Clear the way and i will reward you!", 10, 10);

			clearPassage.QuestCompletionItems.Add (new QuestCompletionItem (items.ItemByID(items.ITEM_ID_BROKEN_TOOTH), 2));
			clearPassage.RewardItem = items.ItemByID (items.ITEM_ID_OLD_COINS);

			Quest clearGraveyard =
				new Quest (
					QUEST_ID_CLEAR_GRAVEYARD,
					"Clear the skeletons roaming the graveyard",
					"Clear the graveyard of those pesky skeletons that keep coming back to life!", 10, 10);

			clearGraveyard.QuestCompletionItems.Add (new QuestCompletionItem (items.ItemByID (items.ITEM_ID_RIB_CAGE), 3));
			clearGraveyard.RewardItem = items.ItemByID (items.ITEM_ID_CLUB);


			Quests.Add (startQuest);
			Quests.Add (clearForrest);
			Quests.Add (clearPassage);
			Quests.Add (clearGraveyard);
		}

		// metod för att söka genom <list>Quests med hjälp av ID
		// och retunera rätt Quest, om inget matchar retunera ingenting(null)
		public static Quest QuestByID(int id)
		{
			foreach (Quest quest in Quests) {
				if (quest.ID == id) {
					return quest;
				}
			}
			return null;
		}
	}
}

