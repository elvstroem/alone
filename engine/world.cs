using System;
using System.Collections.Generic;
using System.Linq;
using engine;

namespace engine
{
	public class World
	{
		public static readonly List<Item> Items = new List<Item>();
		public static readonly List<Monster> Monsters = new List<Monster>();
		public static readonly List<Quest> Quests = new List<Quest>();
		public static readonly List<Location> Locations = new List<Location>();

		// const variabler då dessa värden aldrig ska ändras
		// public för att de ska kunna nås genom hela programmet
		// namngivning och ger dem en int för att lättare kunna
		// iterera genom dem senare om man behöver jämföra dem mot någon 
		// annan lista.

		// osäljbart item
		public const int UNSELLABLE_ITEM_PRICE = -1;

		// items
		public const int ITEM_ID_RUSTY_SWORD = 1;
		public const int ITEM_ID_ROTTING_FLESH = 2;
		public const int ITEM_ID_BROKEN_TOOTH = 3;
		public const int ITEM_ID_WOLF_PELT = 4;
		public const int ITEM_ID_SHARP_FANG = 5;
		public const int ITEM_ID_CLUB = 6;
		public const int ITEM_ID_HEALING_POTION = 7;
		public const int ITEM_ID_RIB_CAGE = 8;
		public const int ITEM_ID_OLD_COINS = 9;
		public const int ITEM_ID_MULTI_PASS = 10;

		// mobs
		public const int MONSTER_ID_ZOMBIE = 1;
		public const int MONSTER_ID_WOLF = 2;
		public const int MONSTER_ID_SKELETON = 3;

		// quests
		public const int QUEST_ID_START_QUEST = 1;
		public const int QUEST_ID_CLEAR_FORREST = 2;
		public const int QUEST_ID_CLEAR_PASSAGE = 3;
		public const int QUEST_ID_CLEAR_GRAVEYARD = 4;

		// locations
		public const int LOCATION_ID_INN = 1;
		public const int LOCATION_ID_TOWN_SQUARE = 2;
		public const int LOCATION_ID_GATE = 3;
		public const int LOCATION_ID_WEST_PASSAGE = 4;
		public const int LOCATION_ID_EAST_PASSAGE = 5;
		public const int LOCATION_ID_START = 6;
		public const int LOCATION_ID_GRAVEYARD = 7;
		public const int LOCATION_ID_BRIDGE = 8;
		public const int LOCATION_ID_FORREST = 9;

		private static void PopulateItems()
		{
			Items.Add (new Weapon (ITEM_ID_RUSTY_SWORD, "Rusty sword", "Rusty swords", 0, 5, 5));
			Items.Add (new Item (ITEM_ID_ROTTING_FLESH, "Rotting flesh", "Rotting flesh", 1));
			Items.Add (new Item (ITEM_ID_BROKEN_TOOTH, "Broken tooth", "Broken teeth", 1));
			Items.Add (new Item (ITEM_ID_WOLF_PELT, "Wolf pelt", "Wolf pelts",1 ));
			Items.Add (new Item (ITEM_ID_SHARP_FANG, "Sharp fang", "Sharp fangs", 2));
			Items.Add (new Weapon (ITEM_ID_CLUB, "Club", "Clubs", 3, 10, 8));
			Items.Add (new HealingPotion (ITEM_ID_HEALING_POTION, "Healing potion", "Healing potions", 5, 3));
			Items.Add (new Item (ITEM_ID_RIB_CAGE, "Rib cage", "Rib cages", 1));
			Items.Add (new Item (ITEM_ID_OLD_COINS, "Old coin", "Old coins", 1));
			Items.Add (new Item (ITEM_ID_MULTI_PASS, "Multi pass", "Multi passes", UNSELLABLE_ITEM_PRICE));
		}

		// metod för att skapa mobs
		// Skapa först moben med tillhörande properties
		// Skapa deras lootTable om de ska ha någon
		// Slutför med att lägga till mobsen i listan "Monsters"
		private static void PopulateMonsters()
		{
			Monster zombie = new Monster (MONSTER_ID_ZOMBIE, "Zombie", 5, 3, 10, 3, 3);
			zombie.LootTable.Add (new LootItem (ItemByID (ITEM_ID_ROTTING_FLESH), 75, true));
			zombie.LootTable.Add (new LootItem (ItemByID (ITEM_ID_BROKEN_TOOTH), 75, false));

			Monster wolf = new Monster (MONSTER_ID_WOLF, "Wolf", 5, 3, 10, 3, 3);
			wolf.LootTable.Add (new LootItem (ItemByID (ITEM_ID_WOLF_PELT), 75, true));
			wolf.LootTable.Add (new LootItem (ItemByID (ITEM_ID_SHARP_FANG), 75, false));

			Monster skeleton = new Monster (MONSTER_ID_SKELETON, "Skeleton", 20, 5, 40, 10, 10);
			skeleton.LootTable.Add (new LootItem (ItemByID (ITEM_ID_RIB_CAGE), 75, true));
			skeleton.LootTable.Add (new LootItem (ItemByID (ITEM_ID_OLD_COINS), 25, false));

			Monsters.Add (zombie);
			Monsters.Add (wolf);
			Monsters.Add (skeleton);
		}

		// metod för att skapa quester
		// Skapa först questen "Quest namnPåQuest" och fyll på med tillhörande properties.
		// Lägg till om man behöver något item för att klara questen.
		// Lägg till om man får någon reward av questen
		// Slutför med att lägga till questerna i listan "Quests"
		public static void PopulateQuests()
		{
			Quest startQuest =
				new Quest (
					QUEST_ID_START_QUEST,
					"Make your way to the gate",
					"All hope is lost! Only hope for salvation is if you make it to the gate!\n" +
					"Show your worth by killing a zombie on your way over and collect some Rotting flesh", 100, 10);

			startQuest.QuestCompletionItems.Add (new QuestCompletionItem (ItemByID (ITEM_ID_ROTTING_FLESH), 1));
			startQuest.RewardItem = ItemByID (ITEM_ID_HEALING_POTION);

			Quest clearForrest =
				new Quest (
					QUEST_ID_CLEAR_FORREST,
					"Clear the forrest of wolfs",
					"Kill of the wolfs and bring back 3 sharp fangs", 10, 10);

			clearForrest.QuestCompletionItems.Add (new QuestCompletionItem (ItemByID (ITEM_ID_SHARP_FANG), 3));
			clearForrest.RewardItem = ItemByID (ITEM_ID_MULTI_PASS);
			clearForrest.RewardItem = ItemByID (ITEM_ID_HEALING_POTION);

			Quest clearPassage =
				new Quest (
					QUEST_ID_CLEAR_PASSAGE,
					"Clear the passage of zombies",
					"The zombies are roaming free on the passage! Clear the way and i will reward you!\n" +
					"Bring back 2 Broken Teeth to the lazy guard", 10, 10);

			clearPassage.QuestCompletionItems.Add (new QuestCompletionItem (ItemByID(ITEM_ID_BROKEN_TOOTH), 2));
			clearPassage.QuestCompletionItems.Add (new QuestCompletionItem (ItemByID (ITEM_ID_ROTTING_FLESH), 1));
			clearPassage.RewardItem = ItemByID (ITEM_ID_OLD_COINS);

			Quest clearGraveyard =
				new Quest (
					QUEST_ID_CLEAR_GRAVEYARD,
					"Clear the skeletons roaming the graveyard",
					"Clear the graveyard of those pesky skeletons that keep coming back to life!", 10, 10);

			clearGraveyard.QuestCompletionItems.Add (new QuestCompletionItem (ItemByID (ITEM_ID_RIB_CAGE), 3));
			clearGraveyard.RewardItem = ItemByID (ITEM_ID_CLUB);


			Quests.Add (startQuest);
			Quests.Add (clearForrest);
			Quests.Add (clearPassage);
			Quests.Add (clearGraveyard);
		}

		// Metod för att skapa Locations.
		// Skapa här en Location med "Location namnPåLocation = new Location(PROPERTIES)
		// Lägg till om här ska vara någon quest eller monster
		// Lägg till i listan "Locations"
		private static void PopulateLocations()
		{
			// startlocation
			Location start = new Location (LOCATION_ID_START, "Start", "You see a crashed caravan and some dead horses, not a welcoming start.");
			start.QuestAvailableHere = QuestByID (QUEST_ID_START_QUEST);

			// West Passage
			Location westPassage = new Location (LOCATION_ID_WEST_PASSAGE, "West Passage", "Nothing much here to see, just a road leading somewhere.");
			westPassage.MonsterLivingHere = MonsterByID (MONSTER_ID_ZOMBIE);

			// West Passage
			Location eastPassage = new Location (LOCATION_ID_EAST_PASSAGE, "East Passage", "Just gravel road, but you see a gloomy graveyard in the distance.");
			eastPassage.MonsterLivingHere = MonsterByID (MONSTER_ID_ZOMBIE);

			// Town Gate
			Location gate = new Location (LOCATION_ID_GATE, "Town Gate", "A scrawny looking fellow is standing guard and gives you a disapproving stare.");
			gate.QuestAvailableHere = QuestByID (QUEST_ID_CLEAR_PASSAGE);
			gate.QuestCompletionArea = QuestByID (QUEST_ID_START_QUEST);

			// Town Square
			Location townSquare = new Location (LOCATION_ID_TOWN_SQUARE, "Town Square", "Everythings grey or looks grey, I mean everything. Altho the fire in the middle is quite nice.");
			townSquare.QuestAvailableHere = QuestByID (QUEST_ID_CLEAR_GRAVEYARD);
			townSquare.QuestCompletionArea = QuestByID (QUEST_ID_CLEAR_FORREST);

			// Local Inn
			Location inn = new Location (LOCATION_ID_INN, "Happy Bobs Inn", "Says 'Happy Bobs Inn' on the sign, but the fellows here looks nothing like a happy bunch.");
			inn.QuestAvailableHere = QuestByID (QUEST_ID_CLEAR_FORREST);

			// Bridge
			Location bridge = new Location (LOCATION_ID_BRIDGE, "Old Bridge", "Old wooden bridge, looks quite sturdy.");

			// Wolf forrest
			Location forrest = new Location (LOCATION_ID_FORREST, "Forrest", "You can here the wind whistle and I dont think those glowing yellow eyes are friedly.");
			forrest.MonsterLivingHere = MonsterByID (MONSTER_ID_WOLF);

			// Skeleton Graveyard
			Location graveyard = new Location (LOCATION_ID_GRAVEYARD, "Ye olde graveyard", "I dont know why, but the skeletons seem like a happy bunch.");
			graveyard.MonsterLivingHere = MonsterByID (MONSTER_ID_SKELETON);
			graveyard.ItemRequiredToEnter = ItemByID (ITEM_ID_MULTI_PASS);

			// home
			Location home = inn;
			// LocationTo* för att veta vilket håll man ska
			// tex. 'inn.LocationToSouth = townSquare' visar att den enda
			// vägen man kan gå om man står på inn är South. Skulle man försöka
			// flytta i någon annan riktning så får man svar att man inte kan gå dit.
			inn.LocationToSouth = townSquare;

			townSquare.LocationToNorth = inn;
			townSquare.LocationToSouth = gate;

			start.LocationToEast = westPassage;

			westPassage.LocationToEast = gate;
			westPassage.LocationToWest = start;

			gate.LocationToSouth = bridge;
			gate.LocationToNorth = townSquare;
			gate.LocationToWest = westPassage;
			gate.LocationToEast = eastPassage;

			eastPassage.LocationToEast = graveyard;
			eastPassage.LocationToWest = gate;

			graveyard.LocationToWest = eastPassage;

			bridge.LocationToNorth = gate;
			bridge.LocationToSouth = forrest;

			forrest.LocationToNorth = bridge;

			Locations.Add (start);
			Locations.Add (townSquare);
			Locations.Add (gate);
			Locations.Add (bridge);
			Locations.Add (westPassage);
			Locations.Add (eastPassage);
			Locations.Add (graveyard);
			Locations.Add (bridge);
			Locations.Add (forrest);
			Locations.Add (home);
		}

		// Kör metoderna så att listorna fylls
		static World()
		{
			PopulateItems();
			PopulateMonsters ();
			PopulateQuests ();
			PopulateLocations ();

		}

		// metod för att söka genom <list>Items med hjälp av ID
		// och retunera rätt Item om inget matchar retunera ingenting(null)
		public static Item ItemByID(int id)
		{
			foreach (Item item in Items) {
				if(item.ID == id) {
					return item;
				}
			}
			return null;
		}

		// metod för att söka genom med <list>Monsters med hjälp av ID
		// och retunera rätt Mob, om inget matchar retunera ingenting(null)
		public static Monster MonsterByID(int id)
		{
			foreach (Monster monster in Monsters) {
				if (monster.ID == id) {
					return monster;
				}
			}
			return null;
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

		// metod för att söka genom <list>Locations med hjälp av ID
		// och retunera rätt Location, om ingeting matchar retunera ingenting(null)
		public static Location LocationByID(int id)
		{
			foreach (Location location in Locations) {
				if (location.ID == id) {
					return location;
				}
			}
			return null;
		}
	}
}

