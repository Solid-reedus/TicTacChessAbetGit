﻿using System;
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

            for (int i = yAxis + 1; i < 3; i++)
            {
                if (_dic[(xAxis, i)] != null)
                {
                    if (_dic[(xAxis, i)].TileOccupier == null)
                    {
                        _dic[(xAxis, i)].Panel.BackColor = Color.Green;
                    }
                    else
                    {
                        _dic[(xAxis, i)].Panel.BackColor = Color.Red;
                        break;
                    }
                }
            }

            for (int i = xAxis + 1; i < 3; i++)
            {
                if (_dic[(i, yAxis)] != null)
                {
                    if (_dic[(i, yAxis)].TileOccupier == null)
                    {
                        _dic[(i, yAxis)].Panel.BackColor = Color.Green;
                    }
                    else
                    {
                        _dic[(i, yAxis)].Panel.BackColor = Color.Red;
                        break;
                    }
                }
            }

            for (int i = yAxis - 1; i > -1; i--)
            {
                if (_dic[(xAxis, i)] != null)
                {
                    Debug.WriteLine("i = " + i);

                    if (_dic[(xAxis, i)].TileOccupier == null)
                    {
                        _dic[(xAxis, i)].Panel.BackColor = Color.Green;
                    }
                    else
                    {
                        _dic[(xAxis, i)].Panel.BackColor = Color.Red;
                        break;
                    }
                }
            }

            for (int i = xAxis - 1; i > -1; i--)
            {
                if (_dic[(i, yAxis)] != null)
                {
                    //Debug.WriteLine("i = " + i);

                    if (_dic[(i, yAxis)].TileOccupier == null)
                    {
                        _dic[(i, yAxis)].Panel.BackColor = Color.Green;
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