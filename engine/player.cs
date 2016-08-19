using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace engine
{
	// Player Klass med Living som bas klass
	public class Player : Living
	{
		

		public string Name { get; set; }
		public int Gold { get; set; }
		public int ExperiencePoints { get; set; }
		public int Level { get; set; }
		public Monster CurrentMonster { get; set; }
		public Weapon CurrentWeapon { get; set; }
		public Location CurrentLocation { get; set; }
		//public List<InventoryItem> Inventory { get; set; }
		public InventoryItem[] Inventory;
		public List<PlayerQuest> Quests { get; set; }
		public List<Weapon> Weapons { get; set; }

		// Player constructor
		public Player (string name, int currentHitPoints, int maximumHitPoints, int gold, int experiencePoints, int level) : base (currentHitPoints, maximumHitPoints)
		{
			Name = name;
			Gold = gold;
			ExperiencePoints = experiencePoints;
			Level = level;

			// skapa ny inventory för Player
			//Inventory = new List<InventoryItem>();
			Inventory = new InventoryItem[16];
			// skapa ny questlista för Player
			Quests = new List<PlayerQuest>();
		}

		public bool HasRequiredItemToEnterThisLocation(Location location)
		{
			if (location.ItemRequiredToEnter == null) {
				return true;
			} 

			// forloop kollar först att Inventory[i] inte är "null"
			// och går sedan genom Inventory array och letar efter en matchning.
			for (int i = 0; i < Inventory.Length; i++) {
				if (Inventory [i] != null) {
					if (Inventory [i].Details.ID == location.ItemRequiredToEnter.ID) {
						return true;
					}
				}
			}
			return false;
		}

		public bool HasThisQuest(Quest quest)
		{
			foreach (PlayerQuest playerQuest in Quests) {
				if (playerQuest.Details.ID == quest.ID) {
					return true;
				}
			}
			return false;
		}

		public bool CompletedThisQuest(Quest quest)
		{
			foreach (PlayerQuest playerQuest in Quests) {
				if (playerQuest.Details.ID == quest.ID) {
					return playerQuest.IsCompleted;
				}
			}
			return false;
		}

		// for loop går genom hela Inventory arrayen och foreach går genom
		// hela QuestCompletionItems listan. Om inventory[i] inte är null så
		// kollar den iaf man har tillräckligt många av typen man behöver
		// för att kunna lämna in questen. Iaf det är sant så retunera true;
		// annars retunera false;
		public bool HasAllQuestCompletionItems(Quest quest)
		{
			bool found = false;
			for(int i = 0; i < Inventory.Length; i++) {
				for(int j = 0; j < quest.QuestCompletionItems.Count; j++) {
					if(Inventory[i] != null) {
						if(Inventory[i].Details.ID == quest.QuestCompletionItems[j].Details.ID) {
							if(Inventory[i].Quantity >= quest.QuestCompletionItems[j].Quantity) {
								found = true;
							}
						}
					}
				}
			}
			//Console.WriteLine(found);
			return found;
		}

		// Förlitar sig på 'HasAllQuestCompletionItems' metoden och får signal
		// av 'MoveTo' metoden som kollar iaf 'HasAllQuestCompletionItems' är true
		// Då initieras denna metod som går genom Inventory array med hjälp av en for loop
		// och QuestCompletionItems listan med hjälp av en foreach loop.
		public void RemoveQuestCompletionItems(Quest quest)
		{
			for (int i = 0; i < Inventory.Length; i++) {
				foreach (QuestCompletionItem qci in quest.QuestCompletionItems) {
					if (Inventory [i] != null) {
						// om Inventory[i] inte är noll, kolla så att ID matchar
						if (Inventory [i].Details.ID == qci.Details.ID) {
							if (Inventory [i].Quantity == qci.Quantity) {
								// iaf de matchar, subtrahera antalet i arrayen
								Inventory [i].Quantity -= qci.Quantity;
								// om där inte finns några items kvar i listan så noll ställ
								// den platsen i arrayen för att kunna lägga till nya
								// items i framtiden.
								if (Inventory [i].Quantity == 0) {
									Inventory [i] = null;
								}
							}
						}
					}
				}
			}
		}

		// metod som retunerar första lediga sloten i en array
		private int FirstFreeInvSlot() {
			for (int i = 0; i < Inventory.Length; ++i) {
				if (Inventory [i] == null) {
					return i;
				}
			}
			return -1;
		}

		// list inventory add item
		public void AddItemToInventory(Item itemToAdd, int quantity = 1)
		{
			// kallar metod FireFreeSlot som går genom listan och hittar
			// första lediga arraySlot
			int invFree = FirstFreeInvSlot ();

			// kollar genom array om där finns några matchande
			// inlägg och plusar på deras antal isåfall.
			for (int i = 0; i < Inventory.Length; i++) {
				if (Inventory [i] != null) {
					if (Inventory [i].Details.ID == itemToAdd.ID) {
						Inventory [i].Quantity += quantity;
					}
				} 
			}

			// för att se så att där inte blir dubbleter i arrayen
			// använder String.Equals för att se om man namnen matchar,
			// gör dom inte det så förblir strCheck false, om den hittar en
			// matchande sträng ändras den till True.
			string str = itemToAdd.Name;
			bool strCheck = false;
			foreach (InventoryItem ii in Inventory) {
				if (ii != null) {
					if (ii.Details.Name.Equals (str)) {
						strCheck = true;
						break;
					}
				}
			}

			// om strCheck fortfarande är false så lägg till ett nytt inlägg
			// i arrayen
			if (strCheck == false) {
				Inventory [invFree] = new InventoryItem (itemToAdd, quantity);
			}
		}

		public void RemoveItemFromInventory(Item itemToRemove, int quantity = 1)
		{
			for (int i = 0; i < Inventory.Length; i++) {
				if (Inventory [i] != null) {
					if (Inventory [i].Details.ID == itemToRemove.ID) {
						Inventory [i].Quantity -= quantity;
						if (Inventory [i].Quantity == 0) {
							Inventory [i] = null;
						}
						break;
					}
				}
			}
		}

		public void MarkQuestCompleted(Quest quest)
		{
			foreach (PlayerQuest pq in Quests) {
				if (pq.Details.ID == quest.ID) {
					pq.IsCompleted = true;
					return;
				}
			}
		}

		public void MoveTo(Location location)
		{
			// kollar om man har ett specifikt item för att kunna 
			// gå till denna location
			if (HasRequiredItemToEnterThisLocation (location) == false) {
				Console.WriteLine ("You must have " + location.ItemRequiredToEnter.Name + " to enter.");
				return;
			}

			CurrentLocation = location;

			// Kollar ifall man har en quest när man kommer till en ny location
			// har man inte den, så får man den. Kollar sedan efter om man har
			// questen redan. Har man Questen och de items som behövs för att klara questen
			// får man Quest rewarden.
			if (location.HasAQuest) {
				if (HasThisQuest (location.QuestAvailableHere) == false) {
					GivePlayerQuest (location.QuestAvailableHere);
				} else {
					if (CompletedThisQuest (location.QuestAvailableHere) == false &&
					    HasAllQuestCompletionItems (location.QuestAvailableHere) == true) {
						GiveQuestReward (location.QuestAvailableHere);
					}
				}
			}

			// Samma som ovan, fast kollar iaf man har nått en specifik plats
			// för att klara questen.
			if (location.CompletionArea) {
				if(HasThisQuest(location.QuestCompletionArea) == true) {
					if (CompletedThisQuest (location.QuestCompletionArea) == false &&
					   HasAllQuestCompletionItems (location.QuestCompletionArea) == true) {
						GiveQuestReward (location.QuestCompletionArea);
					}
				}
			}
			CurrentMonsterLocation (location);
		}

		private void CurrentMonsterLocation(Location location)
		{
			CurrentMonster = location.SpawnMonsterLivingHere ();

			if (CurrentMonster != null) {
				Console.WriteLine ("You see a " + location.MonsterLivingHere.Name);
			}
		}

		private void GivePlayerQuest(Quest quest)
		{
			Console.WriteLine ("You receive the: " + quest.Name + " quest.");
			Console.WriteLine ("Quest Description: " + quest.Description);
			Quests.Add (new PlayerQuest (quest));
		}

		private void GiveQuestReward(Quest quest)
		{
			Console.WriteLine ("");
			Console.WriteLine ("You completed the '" + quest.Name + "' quest.");
			Console.WriteLine ("You receive: ");
			Console.WriteLine (quest.RewardExperiencePoints + " experience points");
			Console.WriteLine (quest.RewardGold + " gold");
			Console.WriteLine (quest.RewardItem.Name, true);

			AddExperiencePoints (quest.RewardExperiencePoints);
			Gold += quest.RewardGold;

			RemoveQuestCompletionItems (quest);
			AddItemToInventory (quest.RewardItem);

			MarkQuestCompleted (quest);
		}

		public void UseWeapon(Weapon weapon)
		{
			int damage = RNG.NumberBetween (weapon.MinimumDamage, weapon.MaximumDamage);
			if (CurrentMonster != null) {
				if (damage == 0) {
					Console.WriteLine ("You missed the " + CurrentMonster.Name);
				} else {
					CurrentMonster.CurrentHitPoints -= damage;
					Console.WriteLine ("You hit the " + CurrentMonster.Name + " for " + damage + " point.");
				}

				if (CurrentMonster.CurrentHitPoints <= 0) {
					LootCurrentMonster ();
					CurrentMonster = null;
				} else {
					LetMobAttack ();
				}
			} else {
				if (CurrentMonster == null) {
					Console.WriteLine ("There is nothing to attack.");
				}
			}
		}

		private void LetMobAttack()
		{
			int damageToPlayer = RNG.NumberBetween (0, CurrentMonster.MaximumDamage);
			if (damageToPlayer == 0) {
				Console.WriteLine (CurrentMonster.Name + " missed you.");
			} else {
				Console.WriteLine ("The " + CurrentMonster.Name + " did " + damageToPlayer + " points of damage.");

				CurrentHitPoints -= damageToPlayer;
			}
			if (CurrentHitPoints <= 0) {
				Console.WriteLine ("The " + CurrentMonster.Name + " killed you.");
			}
		}

		private void LootCurrentMonster ()
		{
			Console.WriteLine ("");
			Console.WriteLine ("You defeated the " + CurrentMonster.Name);
			Console.WriteLine ("You receive " + CurrentMonster.RewardExperiencePoints + " experience points");
			Console.WriteLine ("Your receive " + CurrentMonster.RewardGold + " gold");

			AddExperiencePoints (CurrentMonster.RewardExperiencePoints);
			Gold += CurrentMonster.RewardGold;

			foreach (InventoryItem inventoryItem in CurrentMonster.LootItems) {
				AddItemToInventory (inventoryItem.Details);
				Console.WriteLine ("You loot {0} {1}", inventoryItem.Quantity, inventoryItem.Description);
			}
		}

		private void AddExperiencePoints(int experiencePointsToAdd)
		{
			ExperiencePoints += experiencePointsToAdd;
			MaximumHitPoints = (Level * 10);
		}
		// metod för att flytta Norr
		public void MoveNorth()
		{
			// kollar om LocationToNorth inte är null=ingenting
			// om där är något, så flytta
			if (CurrentLocation.LocationToNorth != null) {
				MoveTo (CurrentLocation.LocationToNorth);
			}
		}

		// metod för att flytta öst
		public void MoveEast()
		{
			// kollar om LocationToEast inte är null=ingenting
			// om där är något, så flytta
			if (CurrentLocation.LocationToEast != null) {
				MoveTo (CurrentLocation.LocationToEast);
			}
		}

		// metod för att flytta söder
		public void MoveSouth()
		{
			// kollar om LocationToSouth inte är null=ingenting
			// om där är något, så flytta
			if (CurrentLocation.LocationToSouth != null) {
				MoveTo (CurrentLocation.LocationToSouth);
			}
		}

		// metod för att flytta väst
		public void MoveWest()
		{
			// kollar om LocationToWest inte är null=ingenting
			// om där är något, så flytta
			if (CurrentLocation.LocationToWest != null) {
				MoveTo (CurrentLocation.LocationToWest);
			}
		}

	}
}

