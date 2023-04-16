using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacChessAbet
{
    internal class Tile
    {
        public string Name { get; private set; }
        public int Row;
        public int Column;
        public Panel Panel;
        public IchessPiece? TileOccupier;
        public bool isPlaceable;

        public int Horizontal { get; private set; }
        public int Rotation { get; private set; }

        public Tile( int _row, int _column, string _name, Panel _panel, int _horizontal, int _rotation)
        {
            Row = _row;
            Column = _column;
            Name = _name;
            Panel = _panel;
            Horizontal = _horizontal;
            Rotation = _rotation;
        }

    }
}
