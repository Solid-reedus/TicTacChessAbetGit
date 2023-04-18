using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacChessAbet
{
    //the IchessPiece interface defines multiple varables and methods that need to be implemented
    //when implementing this interface
    //this makes the code more abstract and makes it so I dont need to specify wich class I am using
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
