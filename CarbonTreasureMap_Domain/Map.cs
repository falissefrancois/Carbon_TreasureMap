namespace CarbonTreasureMap_Domain
{
    public class Map
    {
        public readonly Tile[,] Tiles;
        public readonly int width;
        public readonly int height;

        public Map(int width, int height)
        {
            if (width <= 0 || height <= 0) throw new ArgumentException("Parameters cannot be lower or equal to 0");

            this.width = width;
            this.height = height;

            Tiles = new Tile[width, height];

            InitializeTiles();
        }

        public Map(Map other)
        {
            int width = other.width;
            int height = other.height;

            Tiles = new Tile[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Tiles[i, j] = new Tile(other.Tiles[i, j]);
                }
            }
        }

        public void AddMountain(int x, int y)
        {
            CheckBounds(x, y);

            Tiles[x, y] = new Tile(Terrain.Mountain);
        }

        public void AddTreasure(int x, int y, int treasureNumber)
        {
            CheckBounds(x, y);

            Tiles[x, y].TreasureNumber = treasureNumber;
        }

        private void CheckBounds(int x, int y)
        {
            if (x < 0 || x > width) throw new ArgumentException("X value is out of bounds");
            if (y < 0 || y > height) throw new ArgumentException("Y value is out of bounds");
        }

        private void InitializeTiles()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Tiles[i, j] = new Tile();
                }
            }
        }
    }
}
