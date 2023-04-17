using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacChessAbet
{
    //the main use of the tile class is to be easily set and get information of a tile
    internal class Tile
    {
        public string Name { get; private set; }
        public int Row;
        public int Column;
        public Panel Panel;
        //this value is nullable because a tile can have nothing occupying it
        public IchessPiece? TileOccupier;
        //this value is used to check if you can place a chess piece on it
        public bool isPlaceable;

        //arduino coordinates
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
