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


        //int IchessPiece.xAxis => throw new NotImplementedException();

        public void Move(Dictionary<(int, int), Tile> _dic)
        {
            List<Tile> tiles = new List<Tile>();

            int x = 0;
            int y = 0;



            //total checked tiles = total tiles - position

            //forward
            for (int i = 0; i <  yAxis; i++)
            {
                //2 , -1
                if (_dic[(xAxis, y)] != null)
                {
                    Debug.WriteLine("y = " + y);

                    if (_dic[(xAxis, y)].TileOccupier == null)
                    {
                        _dic[(xAxis, y)].Panel.BackColor= Color.Green;
                        y++;
                    }
                    else
                    {
                        _dic[(xAxis, y)].Panel.BackColor= Color.Red;
                        break;
                    }
                }
            }

            for (int i = 0; i < xAxis; i++)
            {
                //2 , -1
                if (_dic[(x, y)] != null)
                {
                    Debug.WriteLine("y = " + y);

                    if (_dic[(x, yAxis)].TileOccupier == null)
                    {
                        _dic[(x, yAxis)].Panel.BackColor = Color.Green;
                        x++;
                    }
                    else
                    {
                        _dic[(x, yAxis)].Panel.BackColor = Color.Red;
                        break;
                    }
                }
            }


            /*
            x = xAxis;

            //backwards
            for (int i = 0; i < 2; i++)
            {
                if (_dic[(x, y)] != null)
                {
                    if (_dic[(x, y)].TileOccupier == null)
                    {
                        _dic[(x, y)].Panel.BackColor = Color.Green;
                        y--;
                    }
                    else
                    {
                        _dic[(x, y)].Panel.BackColor = Color.Red;
                        break;
                    }
                }
            }

            //left
            for (int i = 0; i < 2; i++)
            {
                if (_dic[(x, y)] != null)
                {
                    if (_dic[(x, y)].TileOccupier == null)
                    {
                        _dic[(x, y)].Panel.BackColor = Color.Green;
                        x++;
                    }
                    else
                    {
                        _dic[(x, y)].Panel.BackColor = Color.Red;
                        break;
                    }
                }
            }

            //right
            for (int i = 0; i < 2; i++)
            {
                if (_dic[(x, y)] != null)
                {
                    if (_dic[(x, y)].TileOccupier == null)
                    {
                        _dic[(x, y)].Panel.BackColor = Color.Green;
                        x--;
                    }
                    else
                    {
                        _dic[(x, y)].Panel.BackColor = Color.Red;
                        break;
                    }
                }
            }
            */

            //Tile tile = _dic[(_x, _y)];
        }

        public string Burb()
        {
            string _res;

            _res = "name = " + Name + "\n" + "x axis = " + xAxis.ToString()+ "\n" + "y axis = " + yAxis.ToString();

            return _res;
        }

        //, int _xAxis, int _yAxis

        public void SetPos(Tile _tile)
        {
            xAxis = _tile.Row;
            yAxis = _tile.Column;
        }

        public TowerChessPiece(string _name, bool _isBlack)
        {
            Name = _name;
            IsBlack = _isBlack;

            if (IsBlack)
            {
                ImgName = "Chess_Piece_b_tower.png";
            }
            else
            {
                ImgName = "Chess_Piece_w_tower.png";
            }
        }
    }
}
