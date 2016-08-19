using System;
using System.Collections.Generic;
using System.Linq;
using engine;

namespace world
{
	public class locations
	{
		public static readonly List<Location> Locations = new List<Location>();

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

		// Metod för att skapa Locations.
		// Skapa här en Location med "Location namnPåLocation = new Location(PROPERTIES)
		// Lägg till om här ska vara någon quest eller monster
		// Lägg till i listan "Locations"
		public static void Populate()
		{
			// startlocation
			Location start = new Location (LOCATION_ID_START, "Start", "You see a crashed caravan and some dead horses, not a welcoming start.");
			start.QuestAvailableHere = quests.QuestByID (quests.QUEST_ID_START_QUEST);

			// West Passage
			Location westPassage = new Location (LOCATION_ID_WEST_PASSAGE, "West Passage", "Nothing much here to see, just a road leading somewhere.");
			westPassage.MonsterLivingHere = monsters.MonsterByID (monsters.MONSTER_ID_ZOMBIE);

			// West Passage
			Location eastPassage = new Location (LOCATION_ID_EAST_PASSAGE, "East Passage", "Just gravel road, but you see a gloomy graveyard in the distance.");
			eastPassage.MonsterLivingHere = monsters.MonsterByID (monsters.MONSTER_ID_ZOMBIE);

			// Town Gate
			Location gate = new Location (LOCATION_ID_GATE, "Town Gate", "A scrawny looking fellow is standing guard and gives you a disapproving stare.");
			gate.QuestAvailableHere = quests.QuestByID (quests.QUEST_ID_CLEAR_PASSAGE);

			// Town Square
			Location townSquare = new Location (LOCATION_ID_TOWN_SQUARE, "Town Square", "Everythings grey or looks grey, I mean everything. Altho the fire in the middle is quite nice.");
			townSquare.QuestAvailableHere = quests.QuestByID (quests.QUEST_ID_CLEAR_GRAVEYARD);

			// Local Inn
			Location inn = new Location (LOCATION_ID_INN, "Happy Bobs Inn", "Says 'Happy Bobs Inn' on the sign, but the fellows here looks nothing like a happy bunch.");
			inn.QuestAvailableHere = quests.QuestByID (quests.QUEST_ID_CLEAR_GRAVEYARD);

			// Bridge
			Location bridge = new Location (LOCATION_ID_BRIDGE, "Old Bridge", "Old wooden bridge, looks quite sturdy.");

			// Wolf forrest
			Location forrest = new Location (LOCATION_ID_FORREST, "Forrest", "You can here the wind whistle and I dont think those glowing yellow eyes are friedly.");
			forrest.MonsterLivingHere = monsters.MonsterByID (monsters.MONSTER_ID_WOLF);

			// Skeleton Graveyard
			Location graveyard = new Location (LOCATION_ID_GRAVEYARD, "Ye olde graveyard", "I dont know why, but the skeletons seem like a happy bunch.");
			graveyard.MonsterLivingHere = monsters.MonsterByID (monsters.MONSTER_ID_SKELETON);
			graveyard.ItemRequiredToEnter = items.ItemByID (items.ITEM_ID_MULTI_PASS);

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

