using System;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace engine
{
	// "public" för att de ska kunna nås genom hela programmet

	// inheritance, när man har flera klasser som representerar nästa
	// samma sak. Här använder vi Living som basklass till både Player
	// och Monster. Sedan använder vi Item som basklass för både
	// Healingpotion och Weapon. 

	// constructor, använder också "public" för att kunna nås genom hela programmet
	// Använder här custom constructor kod för default skapande av objekt,
	// som måste ta in några default värde innan de kan skapas.

	// Klass för alla levande object
	public class Living
	{
		public int CurrentHitPoints { get; set; }
		public int MaximumHitPoints { get; set; }
		public bool IsDead { get { return CurrentHitPoints <= 0; } }

		// living constructor
		public Living(int currentHitPoints, int maximumHitPoints)
		{
			CurrentHitPoints = currentHitPoints;
			MaximumHitPoints = maximumHitPoints;
		}
	}

	// Klass för locations
	public class Location
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public Item ItemRequiredToEnter { get; set; }
		public Quest QuestAvailableHere { get; set; }
		public Quest QuestCompletionArea { get; set; }
		public Monster MonsterLivingHere { get; set; }
		//public Vendor VendorWorkingHere { get; set; }
		public Location LocationToNorth { get; set; }
		public Location LocationToEast { get; set; }
		public Location LocationToSouth { get; set; }
		public Location LocationToWest { get; set; }

		public bool HasAQuest { get { return QuestAvailableHere != null; } } 
		public bool CompletionArea { get { return QuestCompletionArea != null; } }

		// location constructor
		public Location(int id, string name, string description,
			Item itemRequiredToEnter = null, Quest questAvailableHere = null, 
			Quest questCompletionArea = null, Monster monsterLivingHere = null)
		{
			ID = id;
			Name = name;
			Description = description;
			ItemRequiredToEnter = itemRequiredToEnter;
			QuestAvailableHere = questAvailableHere;
			QuestCompletionArea = questCompletionArea;
			MonsterLivingHere = monsterLivingHere;
		}

		public Monster SpawnMonsterLivingHere()
		{
			return MonsterLivingHere == null ? null : MonsterLivingHere.SpawnMonster ();
		}
	}
	

	// man använder ": Klass" för att visa att en klass har en basklass
	// som nedan. När man sedan definerar "public player()" så lägger man till
	// "public player()':base()'" för att visa vilka properties som ska användas
	// från basklassen.

	// Monster klass som även använder basen Living för att konstruera ett objekt
	public class Monster : Living
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public int MaximumDamage { get; set; }
		public int RewardExperiencePoints { get; set; }
		public int RewardGold { get; set; }
		public List<LootItem> LootTable { get; set; }
		public List<InventoryItem> LootItems { get; set; }

		// monster constructor
		public Monster(int id, string name, int maximumDamage, int rewardExperiencePoints, int rewardGold, int currentHitPoints, int maximumHitPoints)
			:base(currentHitPoints, maximumHitPoints)
		{
			ID = id;
			Name = name;
			MaximumDamage = maximumDamage;
			RewardExperiencePoints = rewardExperiencePoints;
			RewardGold = rewardGold;
			LootTable = new List<LootItem> ();
			LootItems = new List<InventoryItem> ();
		}

		internal Monster SpawnMonster()
		{
			Monster newMonster =
				new Monster (ID, Name, MaximumDamage, RewardExperiencePoints, RewardGold, CurrentHitPoints,
					MaximumHitPoints);

			foreach (LootItem lootItem in LootTable.Where(lootItem => RNG.NumberBetween(1, 100) <= lootItem.DropPercentage)) {
				newMonster.LootItems.Add (new InventoryItem (lootItem.Details, 1));
			}

			if (newMonster.LootItems.Count == 0) {
				foreach (LootItem lootItem in LootTable.Where(x => x.IsDefaultItem)) {
					newMonster.LootItems.Add (new InventoryItem (lootItem.Details, 1));
				}
			}

			return newMonster;
		}
	}
		
	// Item Klass
	public class Item
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string NamePlural { get; set; }
		public int Price { get; set; }

		// item constructor
		public Item(int id, string name, string namePlural, int price)
		{
			ID = id;
			Name = name;
			NamePlural = namePlural;
			Price = price;
		}
	}

	// Weapon Klass som även använder Item för att konstruera ett objekt
	public class Weapon : Item
	{
		public int MinimumDamage { get; set; }
		public int MaximumDamage { get; set; }

		// weapon constructor
		public Weapon(int id, string name, string namePlural, int minimumDamage, int maximumDamage, int price) : base(id, name, namePlural, price)
		{
			MinimumDamage = minimumDamage;
			MaximumDamage = maximumDamage;
		}
	}

	// HealingPotion Klass använder Item som bas klass
	public class HealingPotion : Item
	{
		public int AmountToHeal { get; set; }

		// pot constructor
		public HealingPotion(int id, string name, string namePlural, int amountToHeal, int price) : base(id, name, namePlural, price)
		{
			AmountToHeal = amountToHeal;
		}
	}

	// Quest klass
	public class Quest
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int RewardExperiencePoints { get; set; }
		public int RewardGold { get; set; }
		public List<QuestCompletionItem> QuestCompletionItems { get; set; }
		public Item RewardItem { get; set; }

		// quest constructor
		public Quest(int id, string name, string description, int rewardExperiencePoints, int rewardGold)
		{
			ID = id;
			Name = name;
			Description = description;
			RewardExperiencePoints = rewardExperiencePoints;
			RewardGold = rewardGold;
			QuestCompletionItems = new List<QuestCompletionItem> ();
		}
	}

	// InventoryItem Klass
	public class InventoryItem
	{
		public Item Details { get; set; }
		public int Quantity { get; set; }
		public string Description { get { return Quantity > 1 ? Details.NamePlural : Details.Name; } }

		// InventoryItem constructor
		public InventoryItem(Item details, int quantity)
		{
			Details = details;
			Quantity = quantity;
		}
	}

	// LootItem Klass
	public class LootItem
	{
		public Item Details { get; set; }
		public int DropPercentage { get; set; }
		public bool IsDefaultItem { get; set; }

		// LootItem constructor
		public LootItem(Item details, int dropPercentage, bool isDefaultItem)
		{
			Details = details;
			DropPercentage = dropPercentage;
			IsDefaultItem = isDefaultItem;
		}
	}

	// PlayerQuest klass
	public class PlayerQuest
	{
		public Quest Details { get; set; }
		public bool IsCompleted { get; set; }
		public List<QuestCompletionItem> pQuestCompletionItems { get; set; }

		// PlayerQuest constructor
		public PlayerQuest(Quest details) 
		{
			Details = details;
			IsCompleted = false;
			pQuestCompletionItems = new List<QuestCompletionItem> ();
		}
	}

	// QuestCompletionItem Klass
	public class QuestCompletionItem
	{
		public Item Details { get; set; }
		public int Quantity { get; set; }

		// QuestCompletionItem Klass
		public QuestCompletionItem(Item details, int quantity)
		{
			Details = details;
			Quantity = quantity;
		}
	}

	/*public class Vendor
	{
		// Kod kommer inom kort
	}*/

	public static class RNG
	{
		private static Random _generator = new Random();

		public static int NumberBetween(int minimumValue, int maximumValue)
		{
			return _generator.Next (minimumValue, maximumValue + 1);
		}
	}
}

