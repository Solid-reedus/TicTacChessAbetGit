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

        public Tile( int _row, int _column, string _name, Panel _panel)
        {
            Row = _row;
            Column = _column;
            Name = _name;
            Panel = _panel;
        }

        public void IsPlaceable(bool _isPlaceable)
        {
            if (_isPlaceable)
            {
                isPlaceable = true;
                Panel.BackColor= Color.Green;
            }
            else 
            {
                isPlaceable = false;
                Panel.BackColor = Color.White;
            }
        }

    }
}
