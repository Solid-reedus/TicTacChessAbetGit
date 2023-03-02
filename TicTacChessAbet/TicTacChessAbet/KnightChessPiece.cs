using System;
using System.Collections.Generic;
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

        void Move(int x, int y)
        {

        }

        public string Burb()
        {
            string _res;

            _res = "name = " + Name + "\n" + "x axis = " + xAxis.ToString()+ "\n" + "y axis = " + yAxis.ToString();


            return _res;
        }

        public KnightChessPiece(string _name, bool _isBlack, int _xAxis, int _yAxis)
        {
            Name = _name;
            IsBlack = _isBlack;
            xAxis = _xAxis;
            yAxis = _yAxis;

            if (IsBlack)
            {
                ImgName = "Chess_Piece_b_knight.png";
            }
            else
            {
                ImgName = "Chess_Piece_w_knight.png";
            }
        }
    }
}
