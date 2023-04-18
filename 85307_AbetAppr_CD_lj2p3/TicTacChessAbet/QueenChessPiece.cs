using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacChessAbet
{
    internal class QueenChessPiece : IchessPiece
    {
        public string Name { private set; get;}
        public string ImgName { private set; get;}
        public bool IsBlack { private set; get;}

        public int yAxis { private set; get; }
        public int xAxis { private set; get; }

        //the queen works by combinging the rook and the bishop code
        //it will go through a for loop relative to its self and check if the tile is occupied
        //if so it will break and go to the next loop, otherwise its placeable
        public void Move(Dictionary<(int, int), Tile> _dic)
        {
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

        public QueenChessPiece(string _name, bool _isBlack)
        {
            Name = _name;
            IsBlack = _isBlack;

            if (IsBlack)
            {
                ImgName = "Arabian_Swordsman.png";
            }
            else
            {
                ImgName = "Swordman.png";
            }
        }
    }
}
