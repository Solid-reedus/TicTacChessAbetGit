using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacChessAbet
{
    internal interface IchessPiece
    {
        public string Name {get;}
        public string ImgName {get;}
        public bool IsBlack { get; }

        public int xAxis { get; }
        public int yAxis { get; }

        //movement
        public void Move(Dictionary<(int, int), Tile> _dic) { }
        void SetPos(Tile _tile) { }
    }
}
