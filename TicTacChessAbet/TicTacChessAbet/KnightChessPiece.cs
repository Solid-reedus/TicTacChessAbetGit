using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacChessAbet
{
    internal class KnightChessPiece : IchessPiece
    {
        public string Name { private set; get;}
        public string ImgName { private set; get;}
        public bool IsBlack { private set; get;}

        public int yAxis { private set; get; }
        public int xAxis { private set; get; }

        public void Move(Dictionary<(int, int), Tile> _dic)
        {
            List<Tuple<int, int>> moves = new List<Tuple<int, int>>
            {
                Tuple.Create(xAxis + 1, yAxis + 2),
                Tuple.Create(xAxis - 1, yAxis + 2),

                Tuple.Create(xAxis + 1, yAxis - 2), 
                Tuple.Create(xAxis - 1, yAxis - 2), 

                Tuple.Create(xAxis + 2, yAxis + 1), 
                Tuple.Create(xAxis + 2, yAxis - 1), 

                Tuple.Create(xAxis - 2, yAxis - 1), 
                Tuple.Create(xAxis - 2, yAxis + 1), 
            };

            for (int i = 0; i < moves.Count ; i++)
            {
                (int, int) pos = moves[i].ToValueTuple();
                if (pos.Item1 < 3 
                    && pos.Item1 > -1
                    && pos.Item2 < 3
                    && pos.Item2 > -1)
                {
                    if (_dic[(pos)].TileOccupier == null)
                    {
                        _dic[pos].Panel.BackColor = Color.Green;
                        _dic[pos].isPlaceable = true;
                    }
                    else
                    {
                        _dic[pos].Panel.BackColor = Color.Red;
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

        public string Burb()
        {
            string _res;
            _res = "name = " + Name + "\n" + "x axis = " + xAxis.ToString()+ "\n" + "y axis = " + yAxis.ToString();
            return _res;
        }

        public KnightChessPiece(string _name, bool _isBlack)
        {
            Name = _name;
            IsBlack = _isBlack;

            if (IsBlack)
            {
                ImgName = "Arabic_Horse_archer.png";
            }
            else
            {
                ImgName = "Knight.png";
            }
        }
    }
}
