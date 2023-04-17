using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacChessAbet
{
    internal class WizardChessPiece : IchessPiece
    {
        public string Name { private set; get; }
        public string ImgName { private set; get; }
        public bool IsBlack { private set; get; }

        public int yAxis { private set; get; }
        public int xAxis { private set; get; }


        public WizardChessPiece(string _name, bool _isBlack)
        {
            Name = _name;
            IsBlack = _isBlack;

            if (IsBlack)
            {
                ImgName = "Assassin.png";
            }
            else
            {
                ImgName = "Tunnenler.png";
            }
        }
        public void Move(Dictionary<(int, int), Tile> _dic)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (_dic[(i, j)].TileOccupier == null)
                    {
                        _dic[(i, j)].Panel.BackColor = Color.Green;
                        _dic[(i, j)].isPlaceable = true;
                    }
                    else if (_dic[(i, j)].TileOccupier.IsBlack == IsBlack)
                    {
                        _dic[(i, j)].Panel.BackColor = Color.Purple;
                        _dic[(i, j)].isPlaceable = true;
                    }
                }
            }
        }

        public void SetPos(Tile _tile)
        {
            xAxis = _tile.Row;
            yAxis = _tile.Column;
            _tile.TileOccupier = this;
        }

    }
}
