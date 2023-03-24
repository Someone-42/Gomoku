using Gomoku.Players;
using Gomoku.Gomoku;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gomoku.Structures;
using System.Diagnostics;

namespace Gomoku.Competition
{
    public class Simulator
    {
        public Gomoku.Gomoku Game { get; set; }
        public IPlayer[] Players;
        public bool TimeAIs;

        public Simulator(Gomoku.Gomoku game, IPlayer[] players, bool timeAIs)
        {
            Game = game;
            Players = players;
            TimeAIs = timeAIs;
        }

        private void ShowGameState()
        {
            Console.WriteLine("\nCurrent game state :");
            Game.ShowConsole();
            Console.WriteLine("\nScores :");
            for (int i = 0; i < Game.Players; i++)
            {
                Console.WriteLine($"\t> {Players[i].Name} (player {i}) : {Game.PlayerScores[i]}");
            }
        }


        public void RunConsole(int startPlayer = 0)
        {
            int turn = 0;
            byte player = (byte) (startPlayer % Game.Players);

            Stopwatch watch = new Stopwatch();

            while (!Game.GameEnded())
            {
                Console.WriteLine("\n\n##############################################\n\n");
                turn++;
                Console.WriteLine($"> Turn : {turn}");
                ShowGameState();
                watch.Restart();
                Position pos = Players[player].Play();
                watch.Stop();
                Console.WriteLine($"Player {Players[player].Name} ({player}) thought about such extravagant move in {watch.ElapsedTicks / 10}us");

                Game.Place(pos, player);

                foreach (IPlayer p in Players)
                    p.AlertPlay(pos, Players[player]);

                player = (byte) ((player + 1) % Game.Players);
            }
            Console.WriteLine("\n");
            int winner = Game.Winner();
            if (winner < 0)
            {
                Console.WriteLine("The game resulted in a tie, between the players : ");
                byte[] winners = Game.Ties();
                foreach (byte p in winners)
                {
                    Console.Write($"{Players[p].Name} ({p}), ");
                }
                Console.WriteLine("\n");
                return;
            }
            else
            {
                Console.WriteLine($"Winner is : {Players[winner].Name} ({winner})");
            }
        }

    }
}
