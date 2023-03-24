using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gomoku.Structures;

namespace Gomoku.Gomoku
{
    public class Gomoku
    {

        public const byte Empty = byte.MaxValue;

        private static readonly sbyte[,] directions = new sbyte[4, 2] {
            { 1, 1 },   // Up Right
            { 0, 1 },   // Up
            { 1, 0 },   // Right
            { -1, 1 }   // Up left
        };

        public int Width { get; }
        public int Height { get; }
        public int Players { get; }

        public byte[,] Grid { get; }
        public int[] PlayerScores { get; }

        /// <summary>
        /// This function returns how much score is gained from n aligned points
        /// </summary>
        public Func<int, int> GetScoreFromAligned;

        private int totalPlaced, maxPlaced;

        public Gomoku(int width, int height, int players, Func<int, int> getScoreFromAligned)
        {
            Width = width;
            Height = height;
            Players = players;
            Grid = new byte[width, height];
            PlayerScores = new int[players];

            maxPlaced = width * height;
            GetScoreFromAligned = getScoreFromAligned;

            Reset();
        }

        public void Reset()
        {
            totalPlaced = 0;
            for (byte i = 0; i < Players; i++)
            {
                PlayerScores[i] = 0;
            }
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    Grid[i, j] = Empty;
                }
            }
        }

        public bool IsFull()
        {
            return totalPlaced >= maxPlaced;
        }

        public virtual bool GameEnded()
        {
            // Could be changed for tictactoe where game ends as soon as a player gets a score > 0
            return IsFull();
        }

        /// <summary>
        /// Returns the index of the winning player, if there is a tie between two players, returns -1
        /// </summary>
        /// <returns></returns>
        public int Winner()
        {
            int maxI = 0;
            int tie = -1;
            for (byte i = 1; i < Players; i++)
            {
                if (PlayerScores[i] > PlayerScores[maxI])
                    maxI = i;
                else if (PlayerScores[i] == PlayerScores[maxI])
                    tie = i;
            }
            if (tie >= 0 && PlayerScores[tie] == PlayerScores[maxI])
                return -1;
            return maxI;
        }

        /// <summary>
        /// Returns am array of indices of the players who tied
        /// </summary>
        /// <returns></returns>
        public byte[] Ties()
        {

            // TODO: fix, doesn't work
            int ties = 0;
            byte maxI = 0;
            byte[] tiesA = new byte[Players];
            for (byte i = 1; i < Players; i++)
            {
                if (PlayerScores[i] > PlayerScores[maxI])
                {
                    ties = 0;
                    maxI = i;
                }
                else if (PlayerScores[i] == PlayerScores[maxI])
                {
                    tiesA[ties++] = i;
                }
            }
            tiesA[ties] = maxI;
            byte[] ret = new byte[++ties];
            Array.Copy(tiesA, ret, ties);
            return ret;
        }

        public bool CaseExists(Position pos) => pos.x >= 0 && pos.y >= 0 && pos.x < Width && pos.y < Height;

        /// <summary>
        /// Returns the amount of aligned points (minus 1) of same colors, from x,y to infinity in direction dx, dy
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <returns></returns>
        private int GetAlignedSingleDirection(Position pos, sbyte dx, sbyte dy)
        {
            int c = 0;
            byte p = Grid[pos.x, pos.y];
            pos.x += dx; pos.y += dy;
            while (CaseExists(pos) && p == Grid[pos.x, pos.y])
            {
                c++;
                pos.x += dx; pos.y += dy;
            }
            return c;
        }

        /// <summary>
        /// Calculates from x, y the score of every aligned points (cases corresponding to the player in x, y)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int CalculatePlacingScore(Position pos)
        {
            int s = 0;
            byte p = Grid[pos.x, pos.y];
            sbyte dx, dy;
            int a, b;
            for (byte i = 0; i < 4; i++)
            {
                dx = directions[i, 0];
                dy = directions[i, 1];
                a = GetAlignedSingleDirection(pos, dx, dy);
                b = GetAlignedSingleDirection(pos, (sbyte) -dx, (sbyte) -dy);
                s += GetScoreFromAligned(a + b + 1) - GetScoreFromAligned(a) - GetScoreFromAligned(b);
            }
            return s;
        }

        /// <summary>
        /// Returns whether the grid is empty in x, y (doesn't check for case validity)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Placeable(Position pos) => Grid[pos.x, pos.y] == Empty;

        /// <summary>
        /// Returns the amount of score that will be added when removing the case x, y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int CalculateRemovalScore(Position pos)
        {
            int s = 0;
            byte p = Grid[pos.x, pos.y];
            sbyte dx, dy;
            int a, b;
            for (byte i = 0; i < 4; i++)
            {
                dx = directions[i, 0];
                dy = directions[i, 1];
                a = GetAlignedSingleDirection(pos, dx, dy);
                b = GetAlignedSingleDirection(pos, (sbyte)-dx, (sbyte)-dy);
                s += GetScoreFromAligned(a) + GetScoreFromAligned(b) - GetScoreFromAligned(a + b + 1);
            }
            return s;
        }

        public int Place(Position pos, byte player)
        {
            Grid[pos.x, pos.y] = player;
            int score = CalculatePlacingScore(pos);

            PlayerScores[player] += score;
            totalPlaced++;

            return score;
        }

        public int Remove(Position pos, int? addedScore = null)
        {

            byte p = Grid[pos.x, pos.y];
            Grid[pos.x, pos.y] = Empty;
            totalPlaced--;

            if (addedScore is null)
                addedScore = CalculateRemovalScore(pos);
            PlayerScores[p] -= (int)addedScore;

            return (int)addedScore;
        }

        public virtual void ShowConsole()
        {
            throw new NotImplementedException();
        }

    }
}
