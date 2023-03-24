using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku.Gomoku
{
    public class TicTacToe : Gomoku
    {

        public TicTacToe() : base(3, 3, 2, (x) => x == 3 ? 1 : 0) { }

        public override bool GameEnded()
        {
            foreach (int score in PlayerScores)
                if (score > 0)
                    return true;
            return IsFull();
        }

        public override void ShowConsole()
        {
            char[] colors = new char[2] { 'O', 'X' };
            Console.WriteLine("  0 1 2");
            for(int i = 0; i < Height; i++)
            {
                Console.Write($"{i} ");
                for (int j = 0; j < Width; j++)
                {
                    Console.Write(Grid[j, i] == Empty ? "  " : $"{colors[Grid[j, i]]} ");
                }
                Console.WriteLine();
            }
        }

    }
}
