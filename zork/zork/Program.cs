using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zork
{
	public static class Assert
    {
		[Conditional("DEBUG")]
		public static void IsTrue(bool expression, string message = null)
        {
			if (expression == false)
            {
				throw new Exception(message);
            }
        }
    }
	enum Commands
	{
		quit,
		look,
		north,
		south,
		east,
		west,
		unknown
	}
	class Program
	{
		private static Room CurrentRoom
        {
			get
            {
				return Rooms[Location.Row, Location.Column];
            }
        }
		static void Main(string[] args)
		{
			Console.WriteLine("Welcome to Zork!");
			InitializeRoomDescriptions();

			Room previousRoom = null;
			Commands command = Commands.unknown;
			while (command != Commands.quit)
			{
				Console.WriteLine(CurrentRoom);
					if (previousRoom != CurrentRoom)
					{
						Console.WriteLine(CurrentRoom.Description);
						previousRoom = CurrentRoom;
					}
				Console.Write("> ");
				command = ToCommand(Console.ReadLine().Trim());

				string outputString = "";
				switch (command)
				{
					case Commands.quit:
						outputString = "Thank you for playing!";
						break;

					case Commands.look:
						Console.WriteLine(CurrentRoom.Description);
						break;

					case Commands.north:
					case Commands.south:
					case Commands.east:
					case Commands.west:
						if (Move(command) == false)
                        {
							Console.WriteLine("The way is shut!");
                        }
						break;

					default:
						outputString = "Unknown command.";
						break;
				}

				bool Move(Commands c)
                {
					Assert.IsTrue(IsDirection(c), "Invalid direciton.");
					bool isValidMove = true;
					switch (c)
                    {
						case Commands.north when Location.Row < Rooms.GetLength(0) - 1:
							Location.Row++;
							break;

						case Commands.south when Location.Row > 0:
							Location.Row--;
							break;

						case Commands.east when Location.Column < Rooms.GetLength(1) - 1:
							Location.Column++;
							break;

						case Commands.west when Location.Column > 0:
							Location.Column--;
							break;

						default:
							isValidMove = false;
							break;
                    }

					return isValidMove;
                }

				Console.WriteLine(outputString);
			}
		}

		private static Commands ToCommand(string commandString) => 
			(Enum.TryParse<Commands>(commandString, true, out Commands result) ? result : Commands.unknown);

		private static readonly Room[,] Rooms =
		{
			{ new Room("Rocky Trail"), new Room("South of House"), new Room("Canyon View") },
			{ new Room("Forest"), new Room("West of House"), new Room("Behind House") },
			{ new Room("Dense Woods"), new Room("North of House"), new Room("Clearing") }
		};

		private static readonly List<Commands> Directions = new List<Commands>
		{
			Commands.north,
			Commands.south,
			Commands.east,
			Commands.west
		};

		private static (int Row, int Column) Location = (1, 1);

		private static void InitializeRoomDescriptions()
        {
			var roomMap = new Dictionary<string, Room>();
			foreach (Room room in Rooms)
            {
				roomMap[room.Name] = room;
            }

			roomMap["Rocky Trail"].Description = "You are on a rock-strewn trail.";
			roomMap["South of House"].Description = "You are facing the south side of a white house. There is no door here, and all the windows are barred.";
			roomMap["Canyon View"].Description = "You are at the top of the Great Canyon on its south wall.";
			roomMap["Forest"].Description = "This is a forest with trees in all directions around you.";
			roomMap["West of House"].Description = "This is an open field west of a white house with a boarded front door.";
			roomMap["Behind House"].Description = "You are behind the white house. In one corner of the house, there is a small window which is slightly ajar.";
			roomMap["Dense Woods"].Description = "This is a dimly lit forest with large trees all around. To the east, there appears to be sunlight.";
			roomMap["North of House"].Description = "You are facing the north side of a white house. There is no door here, and all the windows are barred.";
			roomMap["Clearing"].Description = "You are in a clearing with a forest surrounding you on the west and south.";
		}

	}
}
