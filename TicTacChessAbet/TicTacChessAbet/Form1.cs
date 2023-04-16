
using System.Diagnostics;
using System.Numerics;
using System.Reflection;
using System.Windows.Forms;
using System.IO.Ports;

namespace TicTacChessAbet
{
    public partial class Form1 : Form
    {
        // IMPORTANT bishop/queen need code rework
        // IMPORTANT pieces before game start code rework needed
        // IMPORTANT placement bug

        SerialPort serialPort = new SerialPort();


        IchessPiece? SelectedChessPiece;
        bool whitesTurn = true;
        bool gameHasBegun = false;
        int whiteScore = 0;
        int blackScore = 0;

        Dictionary<(int, int), Tile> TileDic = new Dictionary<(int, int), Tile>();
        List<(Tile, Tile, Tile)> WinList = new List<(Tile, Tile, Tile)>();

        //Dictionary<Tile, (int, int)> IndexDic = new Dictionary<Tile, (int, int)>();

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

        List<Tile> tiles = new List<Tile>();
        List<Panel> panels = new List<Panel>();

        string basePath = "Resources//images//";

        readonly Tuple<int, int>[] coordinates = 
        {
            Tuple.Create(320, 20),
            Tuple.Create(400, 135),
            Tuple.Create(570, 245),
            Tuple.Create(850, 0),
            Tuple.Create(900, 110),
            Tuple.Create(1050, 200),
            Tuple.Create(1330, 0),
            Tuple.Create(1400, 95),
            Tuple.Create(1520, 175)
        };

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
                    tiles.Add(new Tile(i, j, 
                        panels[val].Name, panels[val], 
                        coordinates[val].Item1, coordinates[val].Item2));
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
            lblcWhiteScore.Text = "white score:" + whiteScore;
            lblBlackScore.Text = "black score:" + blackScore;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void CheckWin()
        {
            for (int i = 0; i < WinList.Count; i++)
            {
                if (WinList[i].Item1.TileOccupier?.IsBlack == true
                    && WinList[i].Item2.TileOccupier?.IsBlack == true
                    && WinList[i].Item3.TileOccupier?.IsBlack == true)
                {
                    WinList[i].Item1.Panel.BackColor= Color.Green;
                    WinList[i].Item2.Panel.BackColor = Color.Green;
                    WinList[i].Item3.Panel.BackColor = Color.Green;

                    groupBox1.Enabled = false;
                    MessageBox.Show("black won");
                    blackScore++;
                }
                else if (WinList[i].Item1.TileOccupier?.IsBlack == false
                    && WinList[i].Item2.TileOccupier?.IsBlack == false
                    && WinList[i].Item3.TileOccupier?.IsBlack == false)
                {
                    WinList[i].Item1.Panel.BackColor = Color.Green;
                    WinList[i].Item2.Panel.BackColor = Color.Green;
                    WinList[i].Item3.Panel.BackColor = Color.Green;

                    groupBox1.Enabled = false;
                    MessageBox.Show("white won");
                    whiteScore++;
                }
            }
        }

        private void pnlChessTiles_Click(object sender, EventArgs e)
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
            if (TileDic[key].TileOccupier == SelectedChessPiece
                && !gameHasBegun
                || TileDic[key].TileOccupier?.IsBlack == whitesTurn)
            {
                lblStatus.Text = "no piece selected";
                SelectedChessPiece = null;
                return;
            }

            //if there is a tile selected and there inst already a selected chess piece then it will
            //highlight the posible positions with .move
            if (TileDic[key].TileOccupier != null
                && SelectedChessPiece == null
                && gameHasBegun)
            {
                SelectedChessPiece = TileDic[key].TileOccupier;
                lblStatus.Text = SelectedChessPiece.Name + " is selected";
                SelectedChessPiece?.Move(TileDic);
            }
            //otherwise it will place the selected chess piece on a tile that is placeable
            else if (TileDic[key].isPlaceable && gameHasBegun && SelectedChessPiece != null)
            {
                for (int i = 0; i < tiles.Count; i++)
                {
                    tiles[i].isPlaceable = false;
                }

                (int, int) a = TileDic.FirstOrDefault(x => x.Value.TileOccupier == SelectedChessPiece).Key;
                TileDic[a].TileOccupier = null;

                SelectedChessPiece.SetPos(TileDic[key]);
                if (whitesTurn)
                {
                    whitesTurn = false;
                }
                else
                {
                    whitesTurn = true;
                }
                lblStatus.Text = SelectedChessPiece.Name + "has been placed on row " + key.Item1 + " and col " + key.Item2;
                UpdateManager();

                SelectedChessPiece = null;

                //this is some debug code 
                // TileDic[a] is where the arm should pick up the piece
                // TileDic[key] is there it should be dropped
                /*
                MessageBox.Show($"posStart = {TileDic[a].Horizontal} and {TileDic[a].Rotation} " +
                    $"to = {TileDic[key].Horizontal} and {TileDic[key].Rotation}");
                */
            }
            // needs rework
            else if (TileDic[key].isPlaceable 
                && !gameHasBegun
                && SelectedChessPiece != null)
            {
                for (int i = 0; i < tiles.Count; i++)
                {
                    tiles[i].isPlaceable = false;
                }

                SelectedChessPiece.SetPos(TileDic[key]);

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < WhiteSelectablePieces.Count; j++)
                    {
                        if (tiles[i].TileOccupier == WhiteSelectablePieces[j].Item2)
                        {
                            WhiteSelectablePieces[j].Item1.Visible = false;
                        }
                    }
                }
                for (int i = 6; i < 9; i++)
                {
                    for (int j = 0; j < BlackSelectablePieces.Count; j++)
                    {
                        if (tiles[i].TileOccupier == BlackSelectablePieces[j].Item2)
                        {
                            BlackSelectablePieces[j].Item1.Visible = false;
                        }
                    }
                }

                UpdateManager();
                SelectedChessPiece = null;
            }
            else
            {
                SelectedChessPiece = null;
            }
        }

        private void pnlSetupTileWhite1_Click(object sender, EventArgs e)
        {
            tiles[0].isPlaceable= true;
            tiles[1].isPlaceable= true;
            tiles[2].isPlaceable= true;

            tiles[0].Panel.BackColor = Color.Yellow;
            tiles[1].Panel.BackColor = Color.Yellow;
            tiles[2].Panel.BackColor = Color.Yellow;

            Panel pnl = (Panel)sender;
            SelectedChessPiece = WhiteSelectablePieces.FirstOrDefault(x => x.Item1 == pnl).Item2;
        }

        private void pnlSetupTileBlack1_Click(object sender, EventArgs e)
        {
            tiles[6].isPlaceable = true;
            tiles[7].isPlaceable = true;
            tiles[8].isPlaceable = true;

            tiles[6].Panel.BackColor = Color.Yellow;
            tiles[7].Panel.BackColor = Color.Yellow;
            tiles[8].Panel.BackColor = Color.Yellow;

            Panel pnl = (Panel)sender;
            SelectedChessPiece = BlackSelectablePieces.FirstOrDefault(x => x.Item1 == pnl).Item2;
        }

        private void btnStartGame_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 3; i++)
            {
                if (tiles[i].TileOccupier == null)
                {
                    return;
                }
            }

            for (int i = 6; i < 9; i++)
            {
                if (tiles[i].TileOccupier == null)
                {
                    return;
                }
            }


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
            lblStatus.Text = "game has started";
            gameHasBegun = true;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                tiles[i].TileOccupier = null;
                tiles[i].Panel.BackColor = Color.White;
            }
            for (int i = 0; i < WhiteSelectablePieces.Count; i++)
            {
                WhiteSelectablePieces[i].Item1.Enabled = true;
                WhiteSelectablePieces[i].Item1.Visible = true;
            }
            for (int i = 0; i < BlackSelectablePieces.Count; i++)
            {
                BlackSelectablePieces[i].Item1.Enabled = true;
                BlackSelectablePieces[i].Item1.Visible = true;
            }
            whitesTurn = true;
            gameHasBegun = false;
            groupBox1.Enabled = true;
            UpdateManager();
        }

    }
}