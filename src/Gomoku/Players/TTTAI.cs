using Gomoku.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gomoku.Players;

namespace Gomoku.Players
{
    /// <summary>
    /// TicTacToe AI
    /// </summary>
    public class TTTAI : IAI
    {
        public Gomoku.Gomoku Game { get; }

        public byte Player { get; }

        private AccessLinkedList<Position> positionList;
        private int MaxDepth = 9;

        public Func<Position> PlayAI { get; protected set; }

        private int IndexFromPosition(int x, int y)
        {
            return x + y * Game.Width;
        }

        private int IndexFromPosition(Position p)
        {
            return p.x + p.y * Game.Width;
        }

        public string Name { get; }

        public TTTAI(Gomoku.Gomoku game, byte player, Func<Position>? playAI = null)
        {
            Name = "TTTAI-V1";
            Game = game;
            Player = player;
            positionList = new AccessLinkedList<Position>(Game.Width * Game.Height);
            for (int i = 0; i < Game.Width; i++)
            {
                for (int j = 0; j < Game.Height; j++)
                {
                    positionList.values[IndexFromPosition(i, j)] = new Position(i, j);
                }
            }
            PlayAI = playAI ?? _Play;
        }

        public void AlertPlay(Position pos, IPlayer player)
        {
            positionList.Pop(IndexFromPosition(pos));
        }

        public Position Play() => PlayAI();

        private Position _Play()
        {
            Position pos = MinMax2P(0, out int _);
            return pos;
        }

        /// <summary>
        /// Minmax function for 2 player tic tac toe
        /// </summary>
        /// <param name="depth"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        private Position MinMax2P(int depth, out int score)
        {
            if (Game.PlayerScores[Player] > 0)
            {
                score = 1;
                return new Position(-1, -1);
            }
            if (Game.PlayerScores[1 - Player] > 0)
            {
                score = -1;
                return new Position(-1, -1);
            }
            if (depth >= MaxDepth || positionList.IsEmpty())
            {
                score = 0;
                return new Position(-1, -1);
            }
            byte player = (byte)((depth + Player) & 1);
            int sign = player == Player ? 1 : -1;
            int placedScore, endScore;
            Position bestPos = new Position(-1, -1), pos;
            score = -(sign * 2);

            foreach (int posIndex in positionList)
            {
                pos = positionList.Pop(posIndex);
                placedScore = Game.Place(pos, player);

                MinMax2P(depth + 1, out endScore);
                if (endScore * sign > score * sign)
                {
                    bestPos = pos;
                    score = endScore;
                }
                
                Game.Remove(pos, placedScore);
                positionList.Push(posIndex);
            }

            return bestPos;
        }

    }
}
