using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacChessAbet
{
    internal class KingChessPiece : IchessPiece
    {
        public string Name { private set; get; }
        public string ImgName { private set; get; }
        public bool IsBlack { private set; get; }

        public int yAxis { private set; get; }
        public int xAxis { private set; get; }

        public KingChessPiece(string _name, bool _isBlack)
        {
            Name = _name;
            IsBlack = _isBlack;

            if (IsBlack)
            {
                ImgName = "Slave.png";
            }
            else
            {
                ImgName = "Spearman.png";
            }
        }
    }
}
