using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku.Structures
{
    public struct Position
    {
        public int x;
        public int y;

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Position(string s)
        {
            try
            {
                string[] ss = s.Trim().Split(new char[] { ' ', ';', ',', ':' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                x = int.Parse(ss[0]);
                y = int.Parse(ss[1]);
            }
            catch
            {
                throw new Exception("String couldn't be parsed to a position, format is : x y");
            }
        }

        public override string ToString()
        {
            return $"column {x}, row {y}";
        }

    }
}
