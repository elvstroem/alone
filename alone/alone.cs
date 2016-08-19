using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using engine;
using world;

namespace alone
{
	class MainClass
	{
		//public static int _questID;
		//public static Quest _currentQuest;
		private static Player _player;
		public static void Main(string[] args)
		{
			// Fråga efter namn
			Console.WriteLine("State your name traveller!");
			string playerName = Console.ReadLine ();

			// player constructor, tar in properties playerName och default stats
			_player = new Player (playerName,20,10,20,0,1);
			// rusty sword
			_player.CurrentWeapon = (Weapon)World.ItemByID(1);
			// rusty sword
			//_player.AddItemToInventory(World.ItemByID (1), 1);
			// multipass
			_player.AddItemToInventory (World.ItemByID(10));

			// skriv ut kommandon för att navigera i mörkret.
			Console.WriteLine ("");
			Console.WriteLine ("Type 'Help' to see a list of commands\n");

			// flyttar spelaren till start området och skriver ut vart man är
			_player.MoveTo (World.LocationByID (World.LOCATION_ID_START));
			Console.WriteLine ("");
			DisplayCurrentLocation ();

			// main spelloop, en infiniteloop som fortsätter tills man skriver exit.
			while (true) {
				Console.Write (">");

				string userInput = Console.ReadLine ();

				if (userInput == null) {
					continue;
				}

				// .ToLower för att förenkla så att det kvittar om 
				// använder skriver in med stora eller små bokstäver
				string cleanedInput = userInput.ToLower ();

				if (cleanedInput == "exit") {
					break;
				}

				ParseInput (cleanedInput);
			}
		}

		// metod för de olika spel kontrollerna
		// tar emot en sträng som argument.
		private static void ParseInput(string input)
		{
			// if-else-if statements för att göra korrensponderande
			// vad värdet av input är.
			if (input.Contains ("help") || input == "?") {
				DisplayHelpText ();
			} else if (input == "stats") {
				DisplayPlayerStats ();
			} else if (input == "look") {
				DisplayCurrentLocation ();
				DisplayCurrentMonster ();
			} else if (input == "north") {
				if(_player.CurrentLocation.LocationToNorth == null) {
					Console.WriteLine("You cannot move North");
				} else {
					_player.MoveNorth ();
				}
			} else if(input.Contains("east")) {
				if(_player.CurrentLocation.LocationToEast == null) {
					Console.WriteLine("You cannot move East");
				} else {
					_player.MoveEast();
				}
			} else if(input.Contains("south")) {
				if(_player.CurrentLocation.LocationToSouth == null) {
					Console.WriteLine("You cannot move South");
				} else {
					_player.MoveSouth();
				}
			} else if(input.Contains("west")) {
				if(_player.CurrentLocation.LocationToWest == null) {
					Console.WriteLine("You cannot move West");
				} else {
					_player.MoveWest();
				}
			} else if(input == "inventory") {
				// array loop
				for (int i = 0; i < _player.Inventory.Length; i++) {
					if (_player.Inventory [i] != null) {
						Console.WriteLine ("{0}, Quantity: {1}", _player.Inventory [i].Details.Name,
							_player.Inventory[i].Quantity);
					}
				}
			} else if(input == "quests") {
				if(_player.Quests.Count == 0) {
					Console.WriteLine("You do not have any quests");
				} else {
					ListQuests ();
					//Console.WriteLine (_questID);
					//Console.WriteLine (_currentQuest.Name);
				} 
			} else if(input.Contains("attack")) {
				AttackMonster();
			} else if(input.StartsWith("equip ")) {
				//EquipWeapon(input);
			} else if(input.StartsWith("drink ")) {
				//DrinkPotion(input);
			} else if(input == "trade") {
				//ViewTradeInventory();
			} else if(input.StartsWith("buy ")) {
				//BuyItem(input);
			} else if(input.StartsWith("sell ")) {
				//SellItem(input);
			} else {
				// Om metoden inte kan tolka vad man har skrivit
				Console.WriteLine("I do not understand");
				Console.WriteLine("Type 'Help' to see a list of available commands");
			}

			Console.WriteLine("");
				
		}

		private static void ListQuests()
		{
			int _questID = 0;
			Quest _currentQuest;

			for (int i = 0; i < _player.Quests.Count; i++) {
				if (_player.Quests [i] != null) {
					if (_player.Quests [i].IsCompleted == false) {
						if (_player.Quests [i].Details.ID != _questID) {
							_questID = _player.Quests [i].Details.ID;

							_currentQuest = World.QuestByID (_questID);
							//List<Quest> _listQuest = new List<Quest> ();
							//_listQuest.Add (_currentQuest);
							List<QuestCompletionItem> qciList = new List<QuestCompletionItem> ();
							foreach (QuestCompletionItem qci in _currentQuest.QuestCompletionItems) {
								if (_currentQuest.QuestCompletionItems != null) {
									qciList.Add (qci);
								}
							}

							Console.WriteLine ("{0}: {1}" + "\nItem required for completion: ", 
							_player.Quests [i].Details.Name, _player.Quests [i].IsCompleted ? "Completed" : "Incomplete");
							
							foreach (QuestCompletionItem qci in qciList) {
								foreach (InventoryItem ii in _player.Inventory) {
									if (ii != null) {
										if (qci.Details.ID == ii.Details.ID) {
											Console.WriteLine (qci.Details.Name + ", " + ii.Quantity + " / " + qci.Quantity);
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// hjälptext
		private static void DisplayHelpText()
		{
			Console.WriteLine ("Available commands");
			Console.WriteLine ("================================");
			Console.WriteLine ("Stats - Display Player Information");
			Console.WriteLine ("Look - Get the description of your location");
			Console.WriteLine ("Inventory - Display your inventory");
			Console.WriteLine ("Quests - Display your quests");
			Console.WriteLine ("Attack - Fight the monster");
			Console.WriteLine ("Equip <weapon name> - Set your current weapon");
			Console.WriteLine ("Drink <potion name> - Drink a potion");
			Console.WriteLine ("Trade - display your inventory and vendor's inventory");
			Console.WriteLine ("Buy <item name> - Buy an item from a vendor");
			Console.WriteLine ("Sell <item name> - Sell an item to a vendor");
			Console.WriteLine ("North - Move North");
			Console.WriteLine ("South - Move South");
			Console.WriteLine ("East - Move East");
			Console.WriteLine ("West - Move West");
			Console.WriteLine ("Exit -Save the game and exit");
		}

		// playerstats
		private static void DisplayPlayerStats()
		{
			Console.WriteLine ("Character name: {0}", _player.Name);
			Console.WriteLine ("Current hit points: {0}", _player.CurrentHitPoints);
			Console.WriteLine ("Maximum hit points: {0}", _player.MaximumHitPoints);
			Console.WriteLine ("Experience points: {0}", _player.ExperiencePoints);
			Console.WriteLine ("Level: {0}", _player.Level);
			Console.WriteLine ("Gold: {0}", _player.Gold);
		}
			
		private static void AttackMonster()
		{
			if (_player.CurrentLocation.MonsterLivingHere != null) {
				_player.UseWeapon (_player.CurrentWeapon);
			} 
		}

		/*private static void EquipWeapon(int input)
		{
			int inputWeaponName = input;

			if (string.IsNullOrEmpty (inputWeaponName)) {
				Console.WriteLine ("You must enter the name of the weapon to equip");
			} else {
				Weapon weaponToEquip =
					_player.Weapons.SingleOrDefault (
						x => x.Name.ToLower () == inputWeaponName);

				if (weaponToEquip == null) {
					Console.WriteLine ("You do not have the weapon: {0}", inputWeaponName);
				} else {
					_player.CurrentWeapon = weaponToEquip;
					Console.WriteLine ("You equip {0}", _player.CurrentWeapon.Name);
				}
			}
		}*/

		/**
		private static void DrinkPotion(string input)
		{
			string inputPotionName = input.Substring (6).Trim ();

			if (string.IsNullOrEmpty (inputPotionName)) {
				Console.WriteLine ("You must enter the name of the potion to drink");
			} else {
				HealingPotion potionToDrink =
					_player.Potions.SingleOrDefault (
						x => x.Name.ToLower () == inputPotionName || x.NamePlural.ToLower () == inputPotionName);

				if (potionToDrink == null) {
					Console.WriteLine ("You do not have the potion: {0}", inputPotionName);
				} else {
					_player.UsePotion (potionToDrink);
				}
			}
		}

		private static void ViewTradeInventory()
		{
			if (LocationDoeNotHaveVendor ()) {
				return;
			}

			Console.WriteLine ("PLAYER INVENTORY");
			Console.WriteLine ("================");

			if (_player.Inventory.Count (x => x.Price != World.UNSELLABLE_ITEM_PRICE) == 0) {
				Console.WriteLine ("You do not have any inventory");
			} else {
				foreach (
					InventoryItem inventoryItem in _player.Inventory.Where(x => x.Price != World.UNSELLABLE_ITEM_PRICE)) {
					Console.WriteLine ("{0} {1} Price: {2}", inventoryItem.Quantity, inventoryItem.Description,
						inventoryItem.Price);
				}
			}

			Console.WriteLine ("");
			Console.WriteLine ("VENDOR INVENTORY");
			Console.WriteLine ("================");

			if (_player.CurrentLocation.VendorWorkingHere.Inventory.Count == 0) {
				Console.WriteLine ("The vendor does not have any inventory");
			} else {
				foreach (InventoryItem inventoryItem in _player.CurrentLocation.VendorWorkingHere.Inventory) {
					Console.WriteLine ("{0} {1} Price: {2}", inventoryItem.Quantity, inventoryItem.Description,
						inventoryItem.Price);
				}
			}
		}

		private static void BuyItem(string input)
		{
			if (LocationDoesNotHaveVendor ()) {
				return;
			}

			string itemName = input.Substring (4).Trim ();
			if (string.IsNullOrEmpty (itemName)) {
				Console.WriteLine ("You must enter the name of the item to buy");
			} else {
				InventoryItem itemToBuy =
					_player.CurrentLocation.VendorWorkingHere.Inventory.SingleOrDefault (
						x => x.Details.Name.ToLower () == itemName);

				if (itemToBuy == null) {
					Console.WriteLine ("The vendor does not have any {0}", itemName);
				} else {
					if (_player.Gold < itemToBuy.Price) {
						Console.WriteLine ("You do not have enough gold to buy a {0}", itemToBuy.Description);
					} else {
						_player.AddItemToInventory (itemToBuy.Details);
						_player.Gold -= itemToBuy.Price;

						Console.WriteLine ("You bought one {0} for {1} gold", itemToBuy.Details.Name, itemToBuy.Price);
					}
				}
			}
		}

		private static void SellItem(string input)
		{
			if (LocationDoesNotHaveVendor ()) {
				return;
			}

			string itemName = input.Substring (5).Trim ();
			if (string.IsNullOrEmpty (itemName)) {
				Console.WriteLine ("You must enter the name of the item to sell");
			} else {
				InventoryItem itemToSell =
					_player.Inventory.SingleOrDefault (x => x.Details.Name.ToLower () == itemName &&
					x.Quantity > 0 && x.Price != World.UNSELLABLE_ITEM_PRICE);

				if (itemToSell == null) {
					Console.WriteLine ("The player cannot sell any {0}", itemName);
				} else {
					_player.RemoveItemFromInventory (itemToSell.Details);
					_player.Gold += itemToSell.Price;

					Console.WriteLine ("You receive {0} gold for {1}", itemToSell.Price, itemToSell.Details.Name);
				}
			}
		}

		private static bool LocationDoesNotHaveVendor()
		{
			bool locationDoesNotHaveVendor = _player.CurrentLocation.VendorWorkingHere == null;
			Console.WriteLine ("There is no vendor at this location");

			return locationDoesNotHaveVendor;
		} 
	**/

		// Skriv ut vart man är någonstans på kartan
		private static void DisplayCurrentLocation()
		{
			Console.WriteLine ("You are at: {0}", _player.CurrentLocation.Name);
			if (_player.CurrentLocation.Description != "") {
				Console.WriteLine (_player.CurrentLocation.Description);
			}
		}

		private static void DisplayCurrentMonster()
		{
			if (_player.CurrentMonster != null) {
				Console.WriteLine ("You see a: {0}", _player.CurrentMonster.Name);
			}
		}
	}
}
