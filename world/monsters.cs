using System;
using System.Collections.Generic;
using System.Linq;
using engine;

namespace world
{
	public class monsters
	{
		public static readonly List<Monster> Monsters = new List<Monster>();

		// mobs
		public const int MONSTER_ID_ZOMBIE = 1;
		public const int MONSTER_ID_WOLF = 2;
		public const int MONSTER_ID_SKELETON = 3;

		// metod för att skapa mobs
		// Skapa först moben med tillhörande properties
		// Skapa deras lootTable om de ska ha någon
		// Slutför med att lägga till mobsen i listan "Monsters"
		public static void Populate()
		{
			Monster zombie = new Monster (MONSTER_ID_ZOMBIE, "Zombie", 5, 3, 10, 3, 3);
			zombie.LootTable.Add (new LootItem (items.ItemByID (items.ITEM_ID_ROTTING_FLESH), 75, true));
			zombie.LootTable.Add (new LootItem (items.ItemByID (items.ITEM_ID_BROKEN_TOOTH), 75, false));

			Monster wolf = new Monster (MONSTER_ID_WOLF, "Wolf", 5, 3, 10, 3, 3);
			wolf.LootTable.Add (new LootItem (items.ItemByID (items.ITEM_ID_WOLF_PELT), 75, true));
			wolf.LootTable.Add (new LootItem (items.ItemByID (items.ITEM_ID_SHARP_FANG), 75, false));

			Monster skeleton = new Monster (MONSTER_ID_SKELETON, "Skeleton", 20, 5, 40, 10, 10);
			skeleton.LootTable.Add (new LootItem (items.ItemByID (items.ITEM_ID_RIB_CAGE), 75, true));
			skeleton.LootTable.Add (new LootItem (items.ItemByID (items.ITEM_ID_OLD_COINS), 25, false));

			Monsters.Add (zombie);
			Monsters.Add (wolf);
			Monsters.Add (skeleton);
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
	}
}

