using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gomoku.Structures;
using Gomoku.Gomoku;

namespace Gomoku.Players
{
    public interface IPlayer
    {
        public Gomoku.Gomoku Game { get; }
        public byte Player { get; }
        public Position Play();
        public void AlertPlay(Position pos, IPlayer player);
        public string Name { get; }
    }
}
