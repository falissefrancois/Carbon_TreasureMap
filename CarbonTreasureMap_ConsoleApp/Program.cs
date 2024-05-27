using CarbonTreasureMap_Domain;
using DataAccess_Files;

namespace CarbonTreasureMap_ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filepath = "../../../../SampleMaps/Sample1.txt";

            var fileReader = new FileReader();

            var game = fileReader.ReadFile(filepath);

            var gameRenderer = new GameRenderer();
            gameRenderer.DisplayGame(game);

            var playedGame = game.Play();
            gameRenderer.DisplayGame(playedGame);
            gameRenderer.DisplayScore(playedGame);
        }
    }
}
