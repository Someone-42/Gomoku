using System;
using Gomoku.Gomoku;
using Gomoku.Structures;
using System.Diagnostics;
using Gomoku.Competition;
using Gomoku.Players;

public static class Program
{
    public static void Main(string[] args)
    {
        TicTacToe game = new TicTacToe();
        Simulator s = new Simulator(
            game,
            new IPlayer[2] {
                //new ConsolePlayer(game, 0, "AAAA"),
                new TTTAI(game, 0),
                new TTTAI(game, 1)
            },
            true
        );
        s.RunConsole();
    }

}