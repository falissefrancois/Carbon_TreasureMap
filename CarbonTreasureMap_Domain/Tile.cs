namespace CarbonTreasureMap_Domain
{
    public class Tile
    {
        public Terrain Terrain { get; set; }
        public int TreasureNumber { get; set; }
        public bool HasAdventurerOnIt { get; set; }

        public Tile(Terrain terrain = Terrain.Plain, bool hasAdventurerOnIt = false)
        {
            Terrain = terrain;
            TreasureNumber = 0;
            HasAdventurerOnIt = hasAdventurerOnIt;
        }

        public Tile(Tile other)
        {
            Terrain = other.Terrain;
            TreasureNumber = other.TreasureNumber;
            HasAdventurerOnIt = other.HasAdventurerOnIt;
        }
    }
}
