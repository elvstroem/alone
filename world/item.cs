using System;
using System.Collections.Generic;
using System.Linq;
using engine;

namespace world
{
	public class items
	{

		// const variabler då dessa värden aldrig ska ändras
		// public för att de ska kunna nås genom hela programmet
		// namngivning och ger dem en int för att lättare kunna
		// iterera genom dem senare om man behöver jämföra dem mot någon 
		// annan lista.

		public static readonly List<Item> Items = new List<Item>();

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


		// metod för att lägga till en massa object i en lista
		// använd ListNamn.Add om du vill lägga till fler i framtiden
		public static void Populate()
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
			
		// metod för att söka genom <list>Items med hjälp av ID
		// och retunera rätt Item om inget matchar retunera ingenting(null)
		public static Item ItemByID(int id)
		{
			foreach (Item item in items.Items) {
				if(item.ID == id) {
					return item;
				}
			}
			return null;
		}
		void Populate();
	}
}

