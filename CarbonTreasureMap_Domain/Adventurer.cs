using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CarbonTreasureMap_Domain
{
    public class Adventurer
    {
        public string name { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int collectedTreasures { get; set; }
        public Direction direction { get; set; }
        public Action[] actions { get; set; }
        private int actionCounter = 0;
        

        public Adventurer(string name, int x, int y, Direction direction, Action[] actions)
        {
            this.name = name;
            this.x = x;
            this.y = y;
            this.direction = direction;
            this.actions = actions;
            this.collectedTreasures = 0;
        }

        public Adventurer(Adventurer other)
        {
            this.name = other.name;
            this.x = other.x;
            this.y = other.y;
            this.direction = other.direction;
            this.actions = other.actions;
            this.collectedTreasures = other.collectedTreasures;
        }

        public (int, int) PlayNextAction()
        {
            int newX = this.x;
            int newY = this.y;

            switch (actions[actionCounter])
            {
                case Action.Forward:
                    (newX, newY) = direction switch
                    {
                        Direction.North => (x, y - 1),
                        Direction.East => (x + 1, y),
                        Direction.South => (x, y + 1),
                        Direction.West => (x - 1, y),
                        _ => throw new Exception("Invalid direction")
                    };
                    break;
                case Action.TurnLeft:
                    direction = direction switch
                    {
                        Direction.North => Direction.West,
                        Direction.West => Direction.South,
                        Direction.South => Direction.East,
                        Direction.East => Direction.North,
                        _ => throw new Exception("Invalid direction")
                    };
                    break;
                case Action.TurnRight:
                    direction = direction switch
                    {
                        Direction.North => Direction.East,
                        Direction.East => Direction.South,
                        Direction.South => Direction.West,
                        Direction.West => Direction.North,
                        _ => throw new Exception("Invalid direction")
                    };
                    break;
                default:
                    throw new Exception("Invalid action");
            }

            actionCounter++;

            return (newX, newY);
        }
    }
}
