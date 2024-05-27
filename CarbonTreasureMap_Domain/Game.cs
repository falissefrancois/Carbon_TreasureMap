using CarbonTreasureMap_ConsoleApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarbonTreasureMap_Domain
{
    public class Game
    {
        public Map Map { get; set; }
        public List<Adventurer> Adventurers { get; set; }

        public Game(Map map, List<Adventurer> adventurers)
        {
            Map = map;
            Adventurers = adventurers;
        }

        public Game Play()
        {
            var resultMap = Map;
            var resultAdventurers = Adventurers;
            var renderer = new GameRenderer();

            ProcessInitialPositions();

            var longestActionCount = Adventurers.Max(a => a.actions.Length);
            for (int i = 0; i < longestActionCount; i++)
            {
                foreach (var adventurer in resultAdventurers)
                {
                    if (i < adventurer.actions.Length)
                    {
                        var (x, y) = adventurer.PlayNextAction();

                        if ((x, y) != (adventurer.x, adventurer.y))
                        {
                            var nextTile = GetTileOnMapAtPosition(x, y);
                            if (nextTile is null)
                                continue;

                            if (nextTile.HasAdventurerOnIt)
                                continue;

                            switch (nextTile.Terrain)
                            {
                                case Terrain.Mountain:
                                    break;
                                case Terrain.Plain:
                                    var previousTile = GetTileOnMapAtPosition(adventurer.x, adventurer.y);
                                    previousTile!.HasAdventurerOnIt = false;
                                    nextTile.HasAdventurerOnIt = true;

                                    adventurer.x = x;
                                    adventurer.y = y;

                                    if (nextTile.TreasureNumber > 0)
                                    {
                                        adventurer.collectedTreasures++;
                                        nextTile.TreasureNumber--;
                                    }
                                    break;
                            }
                        }
                    }
                }

                renderer.DisplayGame(this);
            }

            return new Game(resultMap, resultAdventurers);
        }

        private void ProcessInitialPositions()
        {
            foreach(var adventurer in Adventurers)
            {
                var tile = Map.Tiles[adventurer.x, adventurer.y];

                switch (tile.Terrain)
                {
                    case Terrain.Mountain:
                        throw new Exception("Adventurer declared on a mountain");
                    case Terrain.Plain:
                        if (tile.TreasureNumber > 0)
                        {
                            adventurer.collectedTreasures++;
                            tile.TreasureNumber--;
                        }
                        break;
                }
            }
        }

        private Tile? GetTileOnMapAtPosition(int x, int y)
        {
            if (x >= 0 && x < Map.width && y >= 0 && y < Map.height)
                return Map.Tiles[x, y];

            return null;
        }
    }
}
