using CarbonTreasureMap_Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarbonTreasureMap_ConsoleApp
{
    public class GameRenderer
    {
        public void DisplayGame(Game game)
        {
            var width = game.Map.Tiles.GetLength(0);
            var height = game.Map.Tiles.GetLength(1);

            var tilesToDisplay = new char[width, height];

            var tiles = game.Map.Tiles;
            var adventurers = game.Adventurers;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    var tile = tiles[j, i];

                    string stringToDisplay = MapTerrainToCharDisplay(tile.Terrain);

                    if (tile.TreasureNumber > 0)
                        stringToDisplay = $"T({tile.TreasureNumber})";

                    var adventurer = adventurers.SingleOrDefault(a => j == a.x && i == a.y);
                    if (adventurer is not null)
                        stringToDisplay = $"A({adventurer.name})";

                    DisplayString(stringToDisplay);
                }

                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine("---------------------");
            Console.WriteLine();
        }

        public void DisplayScore(Game game)
        {
            foreach(var adv in game.Adventurers)
            {
                Console.WriteLine($"{adv.name} : {adv.collectedTreasures}");
            }
        }

        private static string MapTerrainToCharDisplay(Terrain terrain)
        {
            return terrain switch
            {
                Terrain.Plain => ".",
                Terrain.Mountain => "M",
                _ => throw new ArgumentException($"Invalid terrain : {terrain}")
            };
        }

        private void DisplayString(string stringToDisplay)
        {
            Console.Write(stringToDisplay);
            var whitespacePadding = new string(' ', 10 - stringToDisplay.Length);
            Console.Write(whitespacePadding);
        }
    }
}
