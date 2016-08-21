using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using engine;

namespace alone
{
	class MainClass
	{
		private static Player _player;
		public static void Main(string[] args)
		{
			Console.WriteLine ("" +
				" ▄▄▄       ██▓     ▒█████   ███▄    █ ▓█████ \n" +
				"▒████▄    ▓██▒    ▒██▒  ██▒ ██ ▀█   █ ▓█   ▀ \n" +
				"▒██  ▀█▄  ▒██░    ▒██░  ██▒▓██  ▀█ ██▒▒███   \n" +
				"░██▄▄▄▄██ ▒██░    ▒██   ██░▓██▒  ▐▌██▒▒▓█  ▄ \n" +
				"▓█   ▓██▒░██████▒░ ████▓▒░▒██░   ▓██░░▒████▒\n" +
				" ▒▒   ▓▒█░░ ▒░▓  ░░ ▒░▒░▒░ ░ ▒░   ▒ ▒ ░░ ▒░ ░\n" +
				"  ▒   ▒▒ ░░ ░ ▒  ░  ░ ▒ ▒░ ░ ░░   ░ ▒░ ░ ░  ░\n" +
				"  ░   ▒     ░ ░   ░ ░ ░ ▒     ░   ░ ░    ░   \n" +
				"      ░  ░    ░  ░    ░ ░           ░    ░  ░\n" +
				"                                             ");
			Console.WriteLine ("");
			Console.WriteLine ("You wake up not knowing where you are, you can't really remeber where you " +
			"where going, you smell the burning of flesh and wood. What happened here? Why is it burning " +
			"you ask yourself. Dammit, why can't I remember what happened? First things first, I remember " +
			"what my name is...");

			// Fråga efter namn
			Console.WriteLine("So, what is your name?");
			string playerName = Console.ReadLine ();

			// player constructor, tar in properties playerName och default stats
			_player = new Player (playerName,1,20,20,0);
			// rusty sword
			_player.CurrentWeapon = (Weapon)World.ItemByID(1);

			// flyttar spelaren till start området och skriver ut vart man är
			_player.MoveTo (World.LocationByID (World.LOCATION_ID_START));
			Console.WriteLine ("");
			DisplayCurrentLocation ();

			// skriv ut kommandon för att navigera i mörkret.
			Console.WriteLine ("");
			Console.WriteLine ("Type 'Help' to see a list of commands\n");

			// main spelloop, en infiniteloop som fortsätter tills man skriver exit.
			while (true) {
				Console.Write (">");

				string userInput = Console.ReadLine ();

				if (userInput == null) {
					continue;
				}

				// .ToLower för att förenkla så att det kvittar om 
				// användaren skriver in med stora eller små bokstäver
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

		// metod för att skriva ut vilka quester man har och vilka items som
		// behövs för att slutföra dem.
		private static void ListQuests()
		{
			int _questID = 0;
			Quest _currentQuest;

			// forloop iterera genom användarens questlista
			for (int i = 0; i < _player.Quests.Count; i++) {
				if (_player.Quests [i] != null) {
					if (_player.Quests [i].IsCompleted == false) {
						// om questen inte finns i listan lägg till den i _questID
						if (_player.Quests [i].Details.ID != _questID) {
							_questID = _player.Quests [i].Details.ID;

							// konvertera _questID till ett fullt objekt så man kan hämta
							// alla properties
							_currentQuest = World.QuestByID (_questID);

							// lägger till alla QuestCompletionItems i en ny lista för att
							// senare kunna jämföra dem med vilka item användaren har i sin Inventory
							List<QuestCompletionItem> qciList = new List<QuestCompletionItem> ();
							foreach (QuestCompletionItem qci in _currentQuest.QuestCompletionItems) {
								if (_currentQuest.QuestCompletionItems != null) {
									qciList.Add (qci);
								}
							}

							// Skriv ut vad questen heter
							Console.WriteLine ("{0}: {1}" + "\nItem required for completion: ", 
								_player.Quests [i].Details.Name, _player.Quests [i].IsCompleted ? "Completed" : "Incomplete");

							// Om spelaren har alla items för att slutföra questen skriv ut
							// Namnet på itemet och x / x
							if (_player.HasAllQuestCompletionItems (_currentQuest) == true) {
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
							// om spelaren inte har alla items med några av dem skriv ut
							// Namnet på itemet och x / x
							else if (_player.HasPartialQuestCompletionItems (_currentQuest) == true) {
								foreach (QuestCompletionItem qci in qciList) {
									foreach (InventoryItem ii in _player.Inventory) {
										if (ii != null) {
											if (qci.Details.ID == ii.Details.ID) {
												Console.WriteLine (qci.Details.Name + " " + ii.Quantity + " / " + qci.Quantity);
											}
										}
									}
								}
							} else {
								// Om spelaren inte har något alls av itemet/itemen som behövs för att slutföra questen
								// Skriv ut namnet/namnen på itemsen som behövs för att slutföra questen.
								if (_player.HasAllQuestCompletionItems (_currentQuest) == false) {
									foreach (QuestCompletionItem qci in qciList) {
										Console.WriteLine (qci.Details.Name + " 0 / " + qci.Quantity);
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
			// feature
		}


		private static void DrinkPotion(string input)
		{
			// feature
		}

		private static void ViewTradeInventory()
		{
			// feature
		}

		private static void BuyItem(string input)
		{
			// feature
		}

		private static void SellItem(string input)
		{
			// feature
		}

		private static bool LocationDoesNotHaveVendor()
		{
			// feature
		} 
		*/

		// Skriv ut vart man är någonstans på kartan
		private static void DisplayCurrentLocation()
		{
			Console.WriteLine ("You are at: {0}", _player.CurrentLocation.Name);
			if (_player.CurrentLocation.Description != "") {
				Console.WriteLine (_player.CurrentLocation.Description);
			}
		}

		// Skriv ut iaf där är en Mob på location
		private static void DisplayCurrentMonster()
		{
			if (_player.CurrentMonster != null) {
				Console.WriteLine ("You see a: {0}", _player.CurrentMonster.Name);
			}
		}
	}
}
