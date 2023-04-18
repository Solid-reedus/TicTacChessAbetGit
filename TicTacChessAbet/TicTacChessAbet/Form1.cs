
using System.Diagnostics;
using System.Numerics;
using System.Reflection;
using System.Windows.Forms;
using System.IO.Ports;
using serialPortDot6Test;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Reflection.Emit;

namespace TicTacChessAbet
{
    public partial class Form1 : Form
    {
        //this class manages the connection between the c# game and the adruino robot
        //this class works by giving it comands
        QueManager queManager;

        //this is a nullable IchessPiece that will temporarily save the selected chess piece
        IchessPiece? SelectedChessPiece;

        bool whitesTurn = true;
        bool gameHasBegun = false;
        int whiteScore = 0;
        int blackScore = 0;

        //these delegates are used to use methods on a other thread
        delegate void IsEnabledDelegate(bool value);
        delegate void UpdateTextDelegate(string value);

        //these bools are for checking the states of the arduino code
        bool usingArduino = false;
        bool connected = false;
        bool AdruinoReady = false;

        //TileDic is a Dictionary that accepts 2 ints that is used to lookup a tile with coordinates
        Dictionary<(int, int), Tile> TileDic = new Dictionary<(int, int), Tile>();

        //this is a list of all possible winnable combinations for both white and black
        List<(Tile, Tile, Tile)> WinList = new List<(Tile, Tile, Tile)>();

        //all the pieces will get instantiated at the beginning
        TowerChessPiece  BlackTw = new TowerChessPiece("black tower",    true);
        KnightChessPiece BlackKgt = new KnightChessPiece("black knight", true);
        QueenChessPiece  BlackQwn = new QueenChessPiece("black queen",   true);
        BishopChessPiece BlackBis = new BishopChessPiece("black bishop", true);
        KingChessPiece   BlackKng = new KingChessPiece("black king",     true);
        WizardChessPiece BlackWzd = new WizardChessPiece("black wizard", true);

        TowerChessPiece  WhiteTw = new TowerChessPiece("white tower",    false);
        KnightChessPiece WhiteKgt = new KnightChessPiece("white knight", false);
        QueenChessPiece  WhiteQwn = new QueenChessPiece("white queen",   false);
        BishopChessPiece WhiteBis = new BishopChessPiece("white bishop", false);
        KingChessPiece   WhiteKng = new KingChessPiece("white king",     false);
        WizardChessPiece WhiteWzd = new WizardChessPiece("white wizard", false);

        //this is a list of eachs players pieces
        List<IchessPiece> WhiteChessPieces = new List<IchessPiece>();
        List<IchessPiece> BlackChessPieces = new List<IchessPiece>();

        //here it will be defined where the piece will be selectable
        List<(Panel, IchessPiece)> WhiteSelectablePieces = new List<(Panel, IchessPiece)>();
        List<(Panel, IchessPiece)> BlackSelectablePieces = new List<(Panel, IchessPiece)>();

        //these two list manage most of the playable board
        //panels contain all of the panels and gets parented to the tile
        //the tile class manages all the playable board code and contains all a tiles 
        //information
        List<Panel> panels = new List<Panel>();
        List<Tile>? tiles = new List<Tile>();

        string basePath = "Resources//images//";

        //this is a list of coordinates that gets used by the arduino arm to place and 
        //pick up all the pieces
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

        //in the form when the game loads a lot of lists a filled to later dynamicly be able
        //to change and eddit code
        public Form1()
        {
            InitializeComponent();

            gbxInoSettings.Visible = false;

            WhiteChessPieces.Add(WhiteTw);
            WhiteChessPieces.Add(WhiteBis);
            WhiteChessPieces.Add(WhiteQwn);
            WhiteChessPieces.Add(WhiteKgt);
            WhiteChessPieces.Add(WhiteKng);
            WhiteChessPieces.Add(WhiteWzd);

            BlackChessPieces.Add(BlackTw);
            BlackChessPieces.Add(BlackBis);
            BlackChessPieces.Add(BlackQwn);
            BlackChessPieces.Add(BlackKgt);
            BlackChessPieces.Add(BlackKng);
            BlackChessPieces.Add(BlackWzd);

            TileSetup();

            WinList.Add((tiles[3], tiles[4], tiles[5]));

            WinList.Add((tiles[0], tiles[3], tiles[6]));
            WinList.Add((tiles[1], tiles[4], tiles[7]));
            WinList.Add((tiles[2], tiles[5], tiles[8]));

            WinList.Add((tiles[0], tiles[4], tiles[8]));
            WinList.Add((tiles[6], tiles[4], tiles[2]));

            UpdateManager();
        }

        //this method set ups the tiles to be able to use later
        void TileSetup()
        {
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
            WhiteSelectablePieces.Add((pnlSetupTileWhite5, WhiteChessPieces[4]));
            WhiteSelectablePieces.Add((pnlSetupTileWhite6, WhiteChessPieces[5]));

            BlackSelectablePieces.Add((pnlSetupTileBlack1, BlackChessPieces[0]));
            BlackSelectablePieces.Add((pnlSetupTileBlack2, BlackChessPieces[1]));
            BlackSelectablePieces.Add((pnlSetupTileBlack3, BlackChessPieces[2]));
            BlackSelectablePieces.Add((pnlSetupTileBlack4, BlackChessPieces[3]));
            BlackSelectablePieces.Add((pnlSetupTileBlack5, BlackChessPieces[4]));
            BlackSelectablePieces.Add((pnlSetupTileBlack6, BlackChessPieces[5]));

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

        //this code will visually update the board and other minor images 
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

        //this code check if someone won
        //it will first check 2 specific unique 3 rows
        //then it will go through a list twice to for both players
        private void CheckWin()
        {
            //checks blacks unique win rows
            if (tiles[0].TileOccupier?.IsBlack == true &&
                tiles[1].TileOccupier?.IsBlack == true &&
                tiles[2].TileOccupier?.IsBlack == true)
            {
                tiles[0].Panel.BackColor = Color.Green;
                tiles[1].Panel.BackColor = Color.Green;
                tiles[2].Panel.BackColor = Color.Green;

                gbxTiles.Enabled = false;
                MessageBox.Show("black won");
                blackScore++;
            }
            
            //checks blacks unique win rows
            if (tiles[6].TileOccupier?.IsBlack == false &&
                tiles[7].TileOccupier?.IsBlack == false &&
                tiles[8].TileOccupier?.IsBlack == false)
            {
                tiles[6].Panel.BackColor = Color.Green;
                tiles[7].Panel.BackColor = Color.Green;
                tiles[8].Panel.BackColor = Color.Green;

                gbxTiles.Enabled = false;
                MessageBox.Show("white won");
                whiteScore++;
            }

            //checks both in a list
            for (int i = 0; i < WinList.Count; i++)
            {
                //checks if black won
                if (WinList[i].Item1.TileOccupier?.IsBlack == true
                    && WinList[i].Item2.TileOccupier?.IsBlack == true
                    && WinList[i].Item3.TileOccupier?.IsBlack == true)
                {
                    WinList[i].Item1.Panel.BackColor = Color.Green;
                    WinList[i].Item2.Panel.BackColor = Color.Green;
                    WinList[i].Item3.Panel.BackColor = Color.Green;

                    gbxTiles.Enabled = false;
                    MessageBox.Show("black won");
                    blackScore++;
                }
                //checks if white won
                else if (WinList[i].Item1.TileOccupier?.IsBlack == false
                    && WinList[i].Item2.TileOccupier?.IsBlack == false
                    && WinList[i].Item3.TileOccupier?.IsBlack == false)
                {
                    WinList[i].Item1.Panel.BackColor = Color.Green;
                    WinList[i].Item2.Panel.BackColor = Color.Green;
                    WinList[i].Item3.Panel.BackColor = Color.Green;

                    gbxTiles.Enabled = false;
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
                lblStatus.Text = SelectedChessPiece?.Name + " is selected";
                SelectedChessPiece?.Move(TileDic);
            }
            //otherwise it will place the selected chess piece on a tile that is placeable
            else if (TileDic[key].isPlaceable && 
                gameHasBegun && 
                TileDic[key]?.TileOccupier != SelectedChessPiece &&
                SelectedChessPiece != null)
            {
                for (int i = 0; i < tiles.Count; i++)
                {
                    tiles[i].isPlaceable = false;
                }

                (int, int) a = TileDic.FirstOrDefault(x => x.Value.TileOccupier == SelectedChessPiece).Key;

                if (TileDic[a].TileOccupier != null && TileDic[key].TileOccupier == null)
                {
                    TileDic[a].TileOccupier = null;
                    SelectedChessPiece.SetPos(TileDic[key]);
                }
                //in the exeption that the chesspiece clicks one of its own and is a wizard 
                //they will swap
                else if (SelectedChessPiece is WizardChessPiece && 
                    TileDic[a]?.TileOccupier?.IsBlack == TileDic[key]?.TileOccupier?.IsBlack)
                {
                    IchessPiece swpPiece = TileDic[key].TileOccupier;
                    IchessPiece wizPiece = TileDic[a].TileOccupier;
                    TileDic[key].TileOccupier = wizPiece;
                    TileDic[a].TileOccupier = swpPiece;
                }

                if (whitesTurn)
                {
                    whitesTurn = false;
                }
                else
                {
                    whitesTurn = true;
                }
                lblStatus.Text = SelectedChessPiece.Name + "has been placed on row " + key.Item1 + " and col " + key.Item2;
                //update the screen
                UpdateManager();

                //when the piece is placed it will be unselected
                SelectedChessPiece = null;

                //if the game is using adruino it will do a extra step
                if (usingArduino && connected)
                {
                    PickNDrop(TileDic[a].Horizontal, TileDic[a].Rotation , TileDic[key].Horizontal, TileDic[key].Rotation);
                }
            }
            //if the game hasnt begun it will will set the selected piece on the placeable tiles
            //and will also hide and disable the selected piece from the selection
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
        //this code will allow white to only place pieces on tile 0 1 and 2
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

        //this code will allow black to only place pieces on tile 6 7 and 8
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

        //this method will start the game by making the gameHasBegun true
        //and making all the selecting pieces items disappear
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

            gbxWhiteLabels.Visible = false;
            gbxBlackLabels.Visible = false;

            lblStatus.Text = "game has started";
            gameHasBegun = true;
        }

        //this code resets the pieces
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
            gbxTiles.Enabled = true;
            UpdateManager();
        }

        //this method enables and disables the arduino
        private void cbxUsingArduino_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxUsingArduino.Checked)
            {
                pnlSetupTileWhite6.Visible = false;
                pnlSetupTileWhite6.Enabled = false;
                pnlSetupTileBlack6.Visible = false;
                pnlSetupTileBlack6.Enabled = false;

                usingArduino = true;
                gbxInoSettings.Visible = true;
            }
            else
            {
                pnlSetupTileWhite6.Visible = true;
                pnlSetupTileWhite6.Enabled = true;
                pnlSetupTileBlack6.Visible = true;
                pnlSetupTileBlack6.Enabled = true;

                usingArduino = false;
                gbxInoSettings.Visible = false;
            }
        }

        //this method searches all the ports and puts them into cbxPorts
        private void btnSearchPtr_Click(object sender, EventArgs e)
        {
            String[] ports = SerialPort.GetPortNames();
            Array.Sort(ports);
            string m_portWithOutLastCharacter;

            foreach (String port in ports)
            {
                m_portWithOutLastCharacter = port;
                cbxPorts.Items.Add(m_portWithOutLastCharacter);
            }
        }

        //this method is used to pick and drop pieces ad various places
        private void PickNDrop(int _fromHoz, int _fromRot, int _toHoz, int _ToRot)
        {
            //this is a list of all the things the arduino must do
            List<MulticastDelegate> delegateList = new List<MulticastDelegate>();

            //this delegate enabels and disables the board
            IsEnabledDelegate gbxTilesEnabledDelegate = (bool _value) =>
            {
                gbxTiles.Enabled = _value;
            };
            //this delegate is used to update the status text
            UpdateTextDelegate UpdateText = (string _status) =>
            {
                lblStatus.Text = _status;
            };

            //all the methods will be added here
            Action methodWithoutParams = () =>
            {
                Invoke(UpdateText, "moving piece");
                gbxTiles.Invoke(gbxTilesEnabledDelegate, false);
                queManager.MoveTo(_fromHoz, _fromRot);
                queManager.PickOrDrop();
                queManager.MoveTo(_toHoz, _ToRot);
                queManager.PickOrDrop();
                queManager.ReturnToStartPos();
                gbxTiles.Invoke(gbxTilesEnabledDelegate, true);
                Invoke(UpdateText, "done moving piece");
            };
            delegateList.Add(methodWithoutParams);

            //this loop will go through each method one for one
            Thread thread = new Thread(() =>
            {
                foreach (var item in delegateList)
                {
                    item.DynamicInvoke();
                }
            });
            thread.Start();
        }

        //this code will connect and ready the arduino
        private void btnConnectToPtr_Click(object sender, EventArgs e)
        {
            //if there isnt anything selected then return early
            if (cbxPorts.Text == "")
            {
                return;
            }

            //this delegate enabels and disables the board
            IsEnabledDelegate gbxTilesEnabledDelegate = (bool _value) =>
            {
                gbxTiles.Enabled = _value;
            };

            //this thread will execute the ready method that will ready the arduino
            //it will also disable the board
            queManager = new QueManager(cbxPorts.Text, ref connected, ref gbxInoSettings, ref lblStatusIno);
            Thread thread = new Thread(() =>
            {
                gbxTiles.Invoke(gbxTilesEnabledDelegate, false);
                queManager.Ready(ref AdruinoReady);
                gbxTiles.Invoke(gbxTilesEnabledDelegate, true);
            });
            thread.Start();
        }
    }
}