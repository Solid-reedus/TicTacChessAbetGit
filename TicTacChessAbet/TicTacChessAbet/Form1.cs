
using System.Diagnostics;
using System.Numerics;
using System.Reflection;
using System.Windows.Forms;

namespace TicTacChessAbet
{
    public partial class Form1 : Form
    {
        // IMPORTANT bishop/queen need code rework

        IchessPiece? SelectedChessPiece;
        bool whitesTurn = true;
        bool GameHasBegun = false;


        Dictionary<(int, int), Tile> TileDic = new Dictionary<(int, int), Tile>();
        List<(Tile, Tile, Tile)> WinList = new List<(Tile, Tile, Tile)>();

        //Dictionary<Tile, (int, int)> IndexDic = new Dictionary<Tile, (int, int)>();

        int rowIndex = 0;
        int colIndex = 0;

        TowerChessPiece  BlackTw = new TowerChessPiece("blackTw", true);
        KnightChessPiece BlackKgt = new KnightChessPiece("blackKgt", true);
        QueenChessPiece  BlackQwn = new QueenChessPiece("blackQwn", true);
        BishopChessPiece BlackBis = new BishopChessPiece("blackBis", true);

        KnightChessPiece WhiteKgt = new KnightChessPiece("whiteKgt", false);
        QueenChessPiece  WhiteQwn = new QueenChessPiece("whiteQwn", false);
        TowerChessPiece  WhiteTw = new TowerChessPiece("whiteTw", false);
        BishopChessPiece WhiteBis = new BishopChessPiece("whiteBis", false);

        List<IchessPiece> WhiteChessPieces = new List<IchessPiece>();
        List<IchessPiece> BlackChessPieces = new List<IchessPiece>();

        List<(Panel, IchessPiece)> WhiteSelectablePieces = new List<(Panel, IchessPiece)>();
        List<(Panel, IchessPiece)> BlackSelectablePieces = new List<(Panel, IchessPiece)>();

        //pnlSetupTileWhite1

        List<Tile> tiles = new List<Tile>();
        List<Panel> panels = new List<Panel>();

        string basePath = "Resources//images//";

        public Form1()
        {
            InitializeComponent();

            WhiteChessPieces.Add(WhiteTw);
            WhiteChessPieces.Add(WhiteBis);
            WhiteChessPieces.Add(WhiteQwn);
            WhiteChessPieces.Add(WhiteKgt);

            BlackChessPieces.Add(BlackTw);
            BlackChessPieces.Add(BlackBis);
            BlackChessPieces.Add(BlackQwn);
            BlackChessPieces.Add(BlackKgt);


            TileSetup();
            //pictureBox1.Image = Image.FromFile("Resources//images//Chess_Piece_b_bishop.png");

            foreach (var item in tiles)
            {
                if (item.Row == 0)
                {
                    item.Panel.BackColor = Color.Yellow;
                }
            }

            richTextBox1.AppendText("\n");

            for (int i = 0; i < tiles.Count; i++)
            {
                richTextBox1.AppendText("tiles["+ i +"] = " + tiles[i].Name + "\n");
            }

            WinList.Add((tiles[3], tiles[4], tiles[5]));

            WinList.Add((tiles[0], tiles[3], tiles[6]));
            WinList.Add((tiles[1], tiles[4], tiles[7]));
            WinList.Add((tiles[2], tiles[5], tiles[8]));

            WinList.Add((tiles[0], tiles[4], tiles[8]));
            WinList.Add((tiles[6], tiles[4], tiles[2]));

            UpdateManager();
        }

        void TileSetup()
        {
            List<string> names = new List<string>();

            panels.Add(pnlChessTileA1);
            panels.Add(pnlChessTileA2);
            panels.Add(pnlChessTileA3);

            panels.Add(pnlChessTileB1);
            panels.Add(pnlChessTileB2);
            panels.Add(pnlChessTileB3);

            panels.Add(pnlChessTileC1);
            panels.Add(pnlChessTileC2);
            panels.Add(pnlChessTileC3);


            WhiteSelectablePieces.Add((pnlSetupTileWhite1, WhiteChessPieces[0]));
            WhiteSelectablePieces.Add((pnlSetupTileWhite2, WhiteChessPieces[1]));
            WhiteSelectablePieces.Add((pnlSetupTileWhite3, WhiteChessPieces[2]));
            WhiteSelectablePieces.Add((pnlSetupTileWhite4, WhiteChessPieces[3]));

            BlackSelectablePieces.Add((pnlSetupTileBlack1, BlackChessPieces[0]));
            BlackSelectablePieces.Add((pnlSetupTileBlack2, BlackChessPieces[1]));
            BlackSelectablePieces.Add((pnlSetupTileBlack3, BlackChessPieces[2]));
            BlackSelectablePieces.Add((pnlSetupTileBlack4, BlackChessPieces[3]));

            for (int i = 0; i < WhiteSelectablePieces.Count; i++)
            {
                WhiteSelectablePieces[i].Item1.BackgroundImage = Image.FromFile(basePath + WhiteSelectablePieces[i].Item2.ImgName);
            }

            for (int i = 0; i < BlackSelectablePieces.Count; i++)
            {
                BlackSelectablePieces[i].Item1.BackgroundImage = Image.FromFile(basePath + BlackSelectablePieces[i].Item2.ImgName);
            }

            int val = 0;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    tiles.Add(new Tile(i, j, panels[val].Name, panels[val]));
                    val++;
                }
            }

            TileDic.Add((0,0) , tiles[0]);
            TileDic.Add((0,1) , tiles[1]);
            TileDic.Add((0,2) , tiles[2]);

            TileDic.Add((1, 0), tiles[3]);
            TileDic.Add((1, 1), tiles[4]);
            TileDic.Add((1, 2), tiles[5]);

            TileDic.Add((2, 0), tiles[6]);
            TileDic.Add((2, 1), tiles[7]);
            TileDic.Add((2, 2), tiles[8]);
        }

        void UpdateManager()
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                if (tiles[i].TileOccupier != null)
                {
                    tiles[i].Panel.BackgroundImage = Image.FromFile(basePath + tiles[i].TileOccupier?.ImgName);
                }
                else
                {
                    tiles[i].Panel.BackgroundImage = null;
                }
            }

            if (whitesTurn)
            {
                pnlWhiteBanner.BackColor= Color.Green;
                pnlBlackBanner.BackColor= Color.White;
            }
            else
            {
                pnlWhiteBanner.BackColor = Color.White;
                pnlBlackBanner.BackColor = Color.Green;
            }

            CheckWin();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateManager();
        }

        private void CheckWin()
        {
            //WinList

            richTextBox1.AppendText("\n check " +  WinList.Count +" \n");

            for (int i = 0; i < WinList.Count; i++)
            {
                /*
                richTextBox1.AppendText("WinList[i].Item1.Name = " + WinList[i].Item1.Name + "\n");
                richTextBox1.AppendText("WinList[i].Item2.Name = " + WinList[i].Item2.Name + "\n");
                richTextBox1.AppendText("WinList[i].Item3.Name = " + WinList[i].Item3.Name + "\n \n");
                */

                if (WinList[i].Item1.TileOccupier?.IsBlack == true
                    && WinList[i].Item2.TileOccupier?.IsBlack == true
                    && WinList[i].Item3.TileOccupier?.IsBlack == true)
                {
                    MessageBox.Show("black won");
                }
                else if (WinList[i].Item1.TileOccupier?.IsBlack == false
                    && WinList[i].Item2.TileOccupier?.IsBlack == false
                    && WinList[i].Item3.TileOccupier?.IsBlack == false)
                {
                    MessageBox.Show("white won");
                }
            }
        }

        private void pnlChessTileA1_Click(object sender, EventArgs e)
        {
            Panel pnl = (Panel)sender;
            (int, int) key = TileDic.FirstOrDefault(x => x.Value.Name == pnl.Name).Key;

            //if there isnt a pawn selected then it will select a pawn and run .move
            //otherwise it will place the pawn on the placeable place
            //and if itself its pressed it will reset the selection


            //this will make all tiles white for visibility
            for (int i = 0; i < panels.Count; i++)
            {
                panels[i].BackColor = Color.White;
            }

            //if the Selected chess piece is once again clicked it will reset the selection 
            if (TileDic[key].TileOccupier == SelectedChessPiece)
            {
                SelectedChessPiece = null;
                return;
            }

            //if there is a tile selected and there inst already a selected chess piece then it will
            //highlight the posible positions with .move
            if (TileDic[key].TileOccupier != null
                && SelectedChessPiece == null
                && GameHasBegun)
            {
                SelectedChessPiece = TileDic[key].TileOccupier;
                lblSelectedChessPiece.Text = SelectedChessPiece.Name;
                SelectedChessPiece.Move(TileDic);
            }
            //otherwise it will place the selected chess piece on a tile that is placeable
            else if (TileDic[key].isPlaceable && GameHasBegun)
            {
                for (int i = 0; i < tiles.Count; i++)
                {
                    tiles[i].isPlaceable = false;
                }

                (int, int) a = TileDic.FirstOrDefault(x => x.Value.TileOccupier == SelectedChessPiece).Key;
                TileDic[a].TileOccupier = null;

                SelectedChessPiece.SetPos(TileDic[key]);
                UpdateManager();
                SelectedChessPiece = null;
            }
            else if (TileDic[key].isPlaceable && !GameHasBegun)
            {
                for (int i = 0; i < tiles.Count; i++)
                {
                    tiles[i].isPlaceable = false;
                }

                (int, int) a = TileDic.FirstOrDefault(x => x.Value.TileOccupier == SelectedChessPiece).Key;

                SelectedChessPiece.SetPos(TileDic[key]);
                UpdateManager();
            }
            else
            {
                SelectedChessPiece = null;
            }

            /*
            richTextBox1.AppendText("\n \n \n");
            for (int i = 0; i < tiles.Count; i++)
            {
                richTextBox1.AppendText("tiles[i].Panel.Name " + tiles[i].Panel.Name);
                if (tiles[i].TileOccupier != null)
                {
                    richTextBox1.AppendText(" tiles[i].TileOccupier.Name " + tiles[i]?.TileOccupier.Name + "\n");
                }
                else
                {
                    richTextBox1.AppendText(" n/a \n");
                }
            }
            */
        }

        #region testCode

        void updateIndexThingy()
        {
            label1.Text = rowIndex.ToString();
            label2.Text = colIndex.ToString();

            for (int i = 0; i < tiles.Count; i++)
            {
                tiles[i].Panel.BackColor = Color.White;
            }

            Tile tile = TileDic[(rowIndex, colIndex)];
            tile.Panel.BackColor = Color.Black;
            label3.Text = tile.Name;
        }

        private void increaseRow_Click(object sender, EventArgs e)
        {
            if (rowIndex < 2)
            {
                rowIndex++;
            }
            updateIndexThingy();
        }

        private void decreaseRow_Click(object sender, EventArgs e)
        {
            if (rowIndex > 0)
            {
                rowIndex--;
            }
            updateIndexThingy();
        }

        private void increaseCol_Click(object sender, EventArgs e)
        {
            if (colIndex < 2)
            {
                colIndex++;
            }
            updateIndexThingy();
        }

        private void decreaseCol_Click(object sender, EventArgs e)
        {
            if (colIndex > 0)
            {
                colIndex--;
            }
            updateIndexThingy();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            WhiteTw.Move(TileDic);
            UpdateManager();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            WhiteQwn.SetPos(tiles[0]);
            WhiteKgt.SetPos(tiles[1]);
            WhiteTw.SetPos(tiles[2]);

            //blackBis.SetPos(tiles[6]);
            BlackQwn.SetPos(tiles[6]);
            BlackKgt.SetPos(tiles[7]);
            BlackTw.SetPos(tiles[8]);

            UpdateManager();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = false;
            groupBox1.Visible = false;
        }

        private void pnlSetupTileWhite1_Click(object sender, EventArgs e)
        {
            tiles[0].isPlaceable= true;
            tiles[1].isPlaceable= true;
            tiles[2].isPlaceable= true;

            Panel pnl = (Panel)sender;
            SelectedChessPiece = WhiteSelectablePieces.FirstOrDefault(x => x.Item1 == pnl).Item2;
            richTextBox1.AppendText("SelectedChessPiece = " + SelectedChessPiece.Name);
        }

        private void pnlSetupTileBlack1_Click(object sender, EventArgs e)
        {
            tiles[6].isPlaceable = true;
            tiles[7].isPlaceable = true;
            tiles[8].isPlaceable = true;

            Panel pnl = (Panel)sender;
            SelectedChessPiece = BlackSelectablePieces.FirstOrDefault(x => x.Item1 == pnl).Item2;
            richTextBox1.AppendText("SelectedChessPiece = " + SelectedChessPiece.Name);
        }

        private void btnStartGame_Click(object sender, EventArgs e)
        {
            GameHasBegun = true;
            for (int i = 0; i < WhiteSelectablePieces.Count; i++)
            {
                WhiteSelectablePieces[i].Item1.Enabled = false;
                WhiteSelectablePieces[i].Item1.Visible = false;
            }
            for (int i = 0; i < BlackSelectablePieces.Count; i++)
            {
                BlackSelectablePieces[i].Item1.Enabled = false;
                BlackSelectablePieces[i].Item1.Visible = false;
            }
        }
    }

    #endregion
}