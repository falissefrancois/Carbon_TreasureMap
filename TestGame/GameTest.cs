using CarbonTreasureMap_ConsoleApp;
using CarbonTreasureMap_Domain;
using Action = CarbonTreasureMap_Domain.Action;

namespace TestGame
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

/*
.          M           .
.          A(Lara)     M
.          .           .
T(2)       T(3)        .

Lara : AADADAGGA
*/
        [Test]
        public void TestScenario1()
        {
            var map = new Map(3, 4);

            InsertMountains(map, (1, 0), (2, 1));
            InsertTreasures(map, (0, 3, 2), (1, 3, 3));

            var Lara = InsertAdventurer(
                map,
                "Lara",
                1,
                1,
                Direction.South,
                [
                    Action.Forward,
                    Action.Forward,
                    Action.TurnRight,
                    Action.Forward,
                    Action.TurnRight,
                    Action.Forward,
                    Action.TurnLeft,
                    Action.TurnLeft,
                    Action.Forward,
                ]);

            var adventurers = new List<Adventurer>()
            {
                Lara
            };

            PlayTest(new Game(map, adventurers));

            Assert.That(Lara.collectedTreasures, Is.EqualTo(3));
            Assert.That((Lara.x, Lara.y), Is.EqualTo((0, 3)));
        }

/*
.          M           .
.          A(Lara)     M
.          M           .
T(2)       T(3)        .

Lara : AADAGAAGAAAAGAAAA
    */
        [Test]
        public void TestScenario2()
        {
            var map = new Map(3, 4);

            InsertMountains(map, (1, 0), (2, 1), (1, 2));
            InsertTreasures(map, (0, 3, 2), (1, 3, 3));

            var Lara = InsertAdventurer(
                map,
                "Lara",
                1,
                1,
                Direction.South,
                [
                    Action.Forward,
                    Action.Forward,
                    Action.TurnRight,
                    Action.Forward,
                    Action.TurnLeft,
                    Action.Forward,
                    Action.Forward,
                    Action.TurnLeft,
                    Action.Forward,
                    Action.Forward,
                    Action.Forward,
                    Action.Forward,
                    Action.TurnLeft,
                    Action.Forward,
                    Action.Forward,
                    Action.Forward,
                    Action.Forward,
                ]);

            var adventurers = new List<Adventurer>()
            {
                Lara
            };

            PlayTest(new Game(map, adventurers));

            Assert.That(Lara.collectedTreasures, Is.EqualTo(2));
            Assert.That((Lara.x, Lara.y), Is.EqualTo((2, 2)));
        }

/*
.          .           .           .           M        .       .
.          .           .           M           .        .       .
.          A(Lara)     M           T(2)        T(1)     .       .
A(Indiana) .           .           .           .        .       .
.          .           .           .           .        .       .

Indiana (E) : AAADDDADAGA
Lara (S)    : AGAAGADA
*/
        [Test]
        public void TestScenario3()
        {
            var map = new Map(7, 5);

            InsertMountains(map, (4, 0), (3, 1), (2, 2));
            InsertTreasures(map, (3, 2, 2), (4, 2, 1));

            var Indiana = InsertAdventurer(
                map,
                "Indiana",
                0,
                3,
                Direction.East,
                [
                    Action.Forward,
                    Action.Forward,
                    Action.Forward,
                    Action.TurnRight,
                    Action.TurnRight,
                    Action.TurnRight,
                    Action.Forward,
                    Action.TurnRight,
                    Action.Forward,
                    Action.TurnLeft,
                    Action.Forward,
                ]);

            var Lara = InsertAdventurer(
                map,
                "Lara",
                1,
                2,
                Direction.South,
                [
                    Action.Forward,
                    Action.TurnLeft,
                    Action.Forward,
                    Action.Forward,
                    Action.TurnLeft,
                    Action.Forward,
                    Action.TurnRight,
                    Action.Forward
                ]);

            var adventurers = new List<Adventurer>()
            {
                Indiana,
                Lara
            };

            PlayTest(new Game(map, adventurers));

            Assert.That(Lara.collectedTreasures, Is.EqualTo(0));
            Assert.That((Lara.x, Lara.y), Is.EqualTo((2, 1)));

            Assert.That(Indiana.collectedTreasures, Is.EqualTo(2));
            Assert.That((Indiana.x, Indiana.y), Is.EqualTo((4, 1)));
        }

        #region Privates
        private static void InsertMountains(Map map, params (int x, int y)[] locations)
        {
            foreach (var (x, y) in locations)
            {
                map.Tiles[x, y].Terrain = Terrain.Mountain;
            }
        }

        private static void InsertTreasures(Map map, params (int x, int y, int val)[] locations)
        {
            foreach (var (x, y, val) in locations)
            {
                map.Tiles[x, y].TreasureNumber = val;
            }
        }

        private static Adventurer InsertAdventurer(Map map, string name, int x, int y, Direction direction, Action[] actions)
        {
            map.Tiles[x, y].HasAdventurerOnIt = true;

            return new Adventurer(
                name,
                x,
                y,
                direction,
                actions);
        }

        private static void PlayTest(Game game)
        {
            var renderer = new GameRenderer();

            renderer.DisplayGame(game);

            var playedGame = game.Play();

            renderer.DisplayGame(playedGame);
            renderer.DisplayScore(playedGame);
        }
        #endregion
    }
}