using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacChessAbet
{
    internal class TowerChessPiece : IchessPiece
    {
        public string Name { private set; get;}
        public string ImgName { private set; get;}
        public bool IsBlack { private set; get;}

        public int yAxis { private set; get; }
        public int xAxis { private set; get; }

        //the rook works by 4 for in loops that go in 4 directions that
        //go through the board and check if its occupied
        //if so the loop will break and go to the next loop
        public void Move(Dictionary<(int, int), Tile> _dic)
        {
            List<Tile> tiles = new List<Tile>();

            //r
            for (int i = yAxis + 1; i < 3; i++)
            {
                if (_dic[(xAxis, i)] != null)
                {
                    if (_dic[(xAxis, i)].TileOccupier == null)
                    {
                        _dic[(xAxis, i)].Panel.BackColor = Color.Green;
                        _dic[(xAxis, i)].isPlaceable = true;
                    }
                    else
                    {
                        _dic[(xAxis, i)].Panel.BackColor = Color.Red;
                        break;
                    }
                }
            }
            
            //b
            for (int i = xAxis + 1; i < 3; i++)
            {
                if (_dic[(i, yAxis)] != null)
                {
                    if (_dic[(i, yAxis)].TileOccupier == null)
                    {
                        _dic[(i, yAxis)].Panel.BackColor = Color.Green;
                        _dic[(i, yAxis)].isPlaceable = true;
                    }
                    else
                    {
                        _dic[(i, yAxis)].Panel.BackColor = Color.Red;
                        break;
                    }
                }
            }

            //l
            for (int i = yAxis - 1; i > -1; i--)
            {
                if (_dic[(xAxis, i)] != null)
                {

                    if (_dic[(xAxis, i)].TileOccupier == null)
                    {
                        _dic[(xAxis, i)].Panel.BackColor = Color.Green;
                        _dic[(xAxis, i)].isPlaceable = true;
                    }
                    else
                    {
                        _dic[(xAxis, i)].Panel.BackColor = Color.Red;
                        break;
                    }
                }
            }
            
            //t
            for (int i = xAxis - 1; i > -1; i--)
            {
                if (_dic[(i, yAxis)] != null)
                {
                    if (_dic[(i, yAxis)].TileOccupier == null)
                    {
                        _dic[(i, yAxis)].Panel.BackColor = Color.Green;
                        _dic[(i, yAxis)].isPlaceable = true;
                    }
                    else
                    {
                        _dic[(i, yAxis)].Panel.BackColor = Color.Red;
                        break;
                    }
                }
            }
        }

        public string Burb()
        {
            string _res;

            _res = "name = " + Name + "\n" + "x axis = " + xAxis.ToString()+ "\n" + "y axis = " + yAxis.ToString();

            return _res;
        }

        public void SetPos(Tile _tile)
        {
            xAxis = _tile.Row;
            yAxis = _tile.Column;
            _tile.TileOccupier = this;
        }

        public TowerChessPiece(string _name, bool _isBlack)
        {
            Name = _name;
            IsBlack = _isBlack;

            if (IsBlack)
            {
                ImgName = "arabic_archer.png";
            }
            else
            {
                ImgName = "Archer.png";
            }
        }
    }
}
