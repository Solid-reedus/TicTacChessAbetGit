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


        public void Move(Dictionary<(int, int), Tile> _dic)
        {
            List<Tile> tiles = new List<Tile>();

            //increment
            int posX = xAxis - 1;
            Debug.WriteLine("posX = " + posX);
            for (int i = xAxis + 1; i < 3; i++)
            {
                Debug.WriteLine("aaaa");
                if (_dic[(posX + i,i)] != null)
                {
                    int a = posX + i;
                    Debug.WriteLine("posX + i = " + a );
                    if (_dic[(posX + i, i)].TileOccupier == null)
                    {
                        _dic[(posX + i , i)].Panel.BackColor = Color.Green;
                    }
                    else
                    {
                        _dic[(posX + i, i)].Panel.BackColor = Color.Red;
                        break;
                    }
                }
            }
            /*
            for (int i = yAxis + 1; i < 3; i++)
            {
                if (_dic[(2 - i, 2 - i)] != null)
                {
                    int a = 2 - i;
                    Debug.WriteLine("i = " + i);
                    Debug.WriteLine("a = " + a);

                    if (_dic[(i, 2 - i)].TileOccupier == null)
                    {
                        _dic[(i, 2 - i)].Panel.BackColor = Color.Green;
                    }
                    else
                    {
                        _dic[(i, 2 - i)].Panel.BackColor = Color.Red;
                        break;
                    }
                }
            }
            */

            /*
            for (int i = yAxis - 1; i > -1; i--)
            {
                if (_dic[(xAxis + i, yAxis + i)] != null)
                {
                    if (_dic[(xAxis + i, yAxis + i)].TileOccupier == null)
                    {
                        _dic[(xAxis + i, yAxis + i)].Panel.BackColor = Color.Green;
                    }
                    else
                    {
                        _dic[(xAxis + i, yAxis + i)].Panel.BackColor = Color.Red;
                        break;
                    }
                }
            }
            */

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
        }

        public BishopChessPiece(string _name, bool _isBlack)
        {
            Name = _name;
            IsBlack = _isBlack;

            if (IsBlack)
            {
                ImgName = "Chess_Piece_b_bishop.png";
            }
            else
            {
                ImgName = "Chess_Piece_w_bishop.png";
            }
        }
    }
}
