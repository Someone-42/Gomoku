using Gomoku.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku.Players
{
    public class ConsolePlayer : IPlayer
    {
        public Gomoku.Gomoku Game { get; set; }

        public byte Player { get; }

        public string Name { get; }

        public ConsolePlayer(Gomoku.Gomoku game, byte player, string name)
        {
            Game = game;
            Player = player;
            Name = name;
        }

        public void AlertPlay(Position pos, IPlayer player)
        {
            if (player.Player == Player)
            {
                Console.WriteLine($"You played in : {pos}");
                return;
            }
            Console.WriteLine($"Player {player.Name} ({player.Player}) played in {pos}");
        }

        private Position AskPosition()
        {
            Position pos = new Position(-1, -1);
            Console.WriteLine("\nPlease input a position to play in :");
            while (true)
            {
                try
                {
                    pos = new Position(Console.ReadLine() ?? "");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                if (Game.CaseExists(pos) && Game.Placeable(pos))
                    break;
                Console.WriteLine("The case doesn't exist, or isn't valid (Someone played already)");
            }
            return pos;
        }

        public Position Play()
        {
            return AskPosition();
        }


    }
}
