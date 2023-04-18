using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacChessAbet
{
    internal class BishopChessPiece : IchessPiece
    {
        public string Name { private set; get;}
        public string ImgName { private set; get;}
        public bool IsBlack { private set; get;}

        public int yAxis { private set; get; }
        public int xAxis { private set; get; }

        //the bishop is able to move based on 4 for loops 
        //it will diagonally check foreach tile
        //if its occupied by a other piece it will break the loop
        //and if its out of bounds it will break the loop too
        public void Move(Dictionary<(int, int), Tile> _dic)
        {
            List<Tile> tiles = new List<Tile>();

            //right down
            for (int i = yAxis + 1; i < 3; i++)
            {
                int min = i - yAxis;
                int x = xAxis + min;

                if (x < 0 || i < 0 || x > 2 || i > 2)
                {
                    break;
                }

                if (_dic[(x, i)] != null)
                {
                    if (_dic[(x, i)].TileOccupier == null)
                    {
                        _dic[(x, i)].Panel.BackColor = Color.Green;
                        _dic[(x, i)].isPlaceable = true;
                    }
                    else
                    {
                        _dic[(x, i)].Panel.BackColor = Color.Red;
                        break;
                    }
                }
            }

            //left up
            for (int i = yAxis - 1; i > -1; i--)
            {
                int min = i - yAxis;
                int x = xAxis + min;

                if (x < 0 || i < 0 || x > 2 || i > 2)
                {
                    break;
                }

                if (_dic[(x, i)] != null)
                {
                    if (_dic[(x, i)].TileOccupier == null)
                    {
                        _dic[(x, i)].Panel.BackColor = Color.Green;
                        _dic[(x, i)].isPlaceable = true;
                    }
                    else
                    {
                        _dic[(x, i)].Panel.BackColor = Color.Red;
                        break;
                    }
                }
            }

            //right up
            for (int i = yAxis + 1; i < 3; i++)
            {
                int min = i - yAxis;
                int x = xAxis - min;

                if (x < 0 || i < 0 || x > 2 || i > 2)
                {
                    break;
                }

                if (_dic[(x, i)] != null)
                {
                    if (_dic[(x, i)].TileOccupier == null)
                    {
                        _dic[(x, i)].Panel.BackColor = Color.Green;
                        _dic[(x, i)].isPlaceable = true;
                    }
                    else
                    {
                        _dic[(x, i)].Panel.BackColor = Color.Red;
                        break;
                    }
                }
            }

            //left down
            for (int i = yAxis - 1; i > -1; i--)
            {
                int min = i - yAxis;
                int x = xAxis - min;


                if (x < 0 || i < 0 || x > 2 || i > 2)
                {
                    break;
                }

                if (_dic[(x, i)] != null)
                {
                    if (_dic[(x, i)].TileOccupier == null)
                    {
                        _dic[(x, i)].Panel.BackColor = Color.Green;
                        _dic[(x, i)].isPlaceable = true;
                    }
                    else
                    {
                        _dic[(x, i)].Panel.BackColor = Color.Red;
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

        public BishopChessPiece(string _name, bool _isBlack)
        {
            Name = _name;
            IsBlack = _isBlack;

            if (IsBlack)
            {
                ImgName = "fire_thrower.png";
            }
            else
            {
                ImgName = "Monk.png";
            }
        }
    }
}
