using CarbonTreasureMap_Domain;
using System.Text.RegularExpressions;
using Action = CarbonTreasureMap_Domain.Action;

namespace DataAccess_Files
{
    public class FileReader
    {
        private const string regexPattern_Comment = @"^#.*$";
        private const string regexPattern_Map = @"^C - (\d+) - (\d+)$";
        private const string regexPattern_Mountain = @"^M - (\d+) - (\d+)$";
        private const string regexPattern_Treasure = @"^T - (\d+) - (\d+) - (\d+)$";
        private const string regexPattern_Adventurer = @"^A - ([a-zA-Z]+) - (\d+) - (\d+) - ([NSEO]) - ([ADG]+)$";

        public Game ReadFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException($"'{nameof(filePath)}' cannot be null or whitespace.", nameof(filePath));
            }

            int lineNumber = 0;
            try
            {
                Map? map = null;
                var adventurers = new List<Adventurer>();
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string? line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Trim();
                        Match match;

                        match = Regex.Match(line, regexPattern_Comment);
                        if (match.Success)
                        {
                            lineNumber++;
                            continue;
                        }

                        match = Regex.Match(line, regexPattern_Map);
                        if (match.Success)
                        {
                            if (map is not null)
                            {
                                throw new ArgumentException("Multiple map declarations");
                            }

                            map = new Map(int.Parse(match.Groups[1].Value),
                                          int.Parse(match.Groups[2].Value));

                            lineNumber++;
                            continue;
                        }

                        match = Regex.Match(line, regexPattern_Mountain);
                        if (match.Success)
                        {
                            if (map is null)
                            {
                                throw new ArgumentException("Map not declared yet");
                            }

                            map.AddMountain(int.Parse(match.Groups[1].Value),
                                            int.Parse(match.Groups[2].Value));

                            lineNumber++;
                            continue;
                        }

                        match = Regex.Match(line, regexPattern_Treasure);
                        if (match.Success)
                        {
                            if (map is null)
                            {
                                throw new ArgumentException("Map not declared yet");
                            }

                            map.AddTreasure(int.Parse(match.Groups[1].Value),
                                            int.Parse(match.Groups[2].Value),
                                            int.Parse(match.Groups[3].Value));

                            lineNumber++;
                            continue;
                        }

                        match = Regex.Match(line, regexPattern_Adventurer);
                        if (match.Success)
                        {
                            if (map is null)
                            {
                                throw new ArgumentException("Map not declared yet");
                            }

                            var name = match.Groups[1].Value;
                            var x = int.Parse(match.Groups[2].Value);
                            var y = int.Parse(match.Groups[3].Value);

                            var targetTile = GetTileOnMapAtPosition(map, x, y);

                            if (targetTile is null)
                                throw new ArgumentException("Adventurer position invalid");

                            if (adventurers.Any(a => a.name == name))
                                throw new ArgumentException($"Adventurer {name} already exists");

                            if (targetTile.Terrain == Terrain.Mountain)
                                throw new ArgumentException($"Adventurer cannot be declared on mountain");

                            if(targetTile.HasAdventurerOnIt)
                                throw new ArgumentException($"Adventurer already in the same location {x},{y}");

                            adventurers.Add(
                                new Adventurer(
                                    name,
                                    x,
                                    y,
                                    MapCharToDirection(match.Groups[4].Value[0]),
                                    MapStringToActions(match.Groups[5].Value)));

                            targetTile.HasAdventurerOnIt = true;
                            lineNumber++;
                            continue;
                        }
                    }
                }

                if (map is null) throw new ArgumentException("Map not declared");
                return new Game(map, adventurers);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Parsing the file encountered an error at line {lineNumber} : {ex.Message}");
                throw;
            }
        }

        private static Tile? GetTileOnMapAtPosition(Map? map, int x, int y)
        {
            if (map is null) throw new ArgumentException("Map cannot be null");

            if(x >= 0 && x < map.width && y >= 0 && y < map.height)
                return map.Tiles[x, y];

            return null;
        }

        internal static Action[] MapStringToActions(string input)
        {
            var actions = new List<Action>();

            foreach (char c in input)
            {
                Action action = c switch
                {
                    'A' => Action.Forward,
                    'D' => Action.TurnRight,
                    'G' => Action.TurnLeft,
                    _ => throw new ArgumentException($"Invalid action character: {c}")
                };

                actions.Add(action);
            }

            return actions.ToArray();
        }

        internal static Direction MapCharToDirection(char input)
        {
            return input switch
            {
                'N' => Direction.North,
                'S' => Direction.South,
                'E' => Direction.East,
                'O' => Direction.West,
                _ => throw new ArgumentException($"Invalid action character: {input}")
            };
        }
    }
}
