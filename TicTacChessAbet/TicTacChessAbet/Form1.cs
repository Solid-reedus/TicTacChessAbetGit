
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

        Dictionary<(int, int), Tile> TileDic = new Dictionary<(int, int), Tile>();
        //Dictionary<Tile, (int, int)> IndexDic = new Dictionary<Tile, (int, int)>();

        int rowIndex = 0;
        int colIndex = 0;

        TowerChessPiece blackTw = new TowerChessPiece("blackTw", true);
        KnightChessPiece blackKgt = new KnightChessPiece("blackKgt", true);
        QueenChessPiece blackQwn = new QueenChessPiece("blackQwn", false);

        KnightChessPiece whiteKgt = new KnightChessPiece("whiteKgt", false);
        QueenChessPiece whiteQwn = new QueenChessPiece("whiteQwn", false);

        TowerChessPiece whiteTw = new TowerChessPiece("whiteTw", false);

        BishopChessPiece whiteBis = new BishopChessPiece("whiteBis", false);

        List<Tile> tiles = new List<Tile>();
        List<Panel> panels = new List<Panel>();
        string basePath = "Resources//images//";

        public Form1()
        {
            InitializeComponent();

            TileSetup();
            //pictureBox1.Image = Image.FromFile("Resources//images//Chess_Piece_b_bishop.png");

            foreach (var item in tiles)
            {
                if (item.Row == 0)
                {
                    item.Panel.BackColor = Color.Yellow;
                }
            }

            richTextBox1.AppendText(TileDic[(0, 0)].Name + "\n");
            richTextBox1.AppendText(TileDic[(0, 1)].Name + "\n");
            richTextBox1.AppendText(TileDic[(0, 2)].Name + "\n");

            richTextBox1.AppendText(TileDic[(1, 0)].Name + "\n");
            richTextBox1.AppendText(TileDic[(1, 1)].Name + "\n");
            richTextBox1.AppendText(TileDic[(1, 2)].Name + "\n");

            richTextBox1.AppendText(TileDic[(2, 0)].Name + "\n");
            richTextBox1.AppendText(TileDic[(2, 1)].Name + "\n");
            richTextBox1.AppendText(TileDic[(2, 2)].Name + "\n");

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

            int val = 0;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    tiles.Add(new Tile(i, j, panels[val].Name, panels[val]));
                    //IndexDic.Add(tiles[i], (i, j));
                    //IndexDic.Add(tiles[i], (i, j));
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
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateManager();
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
                && SelectedChessPiece == null)
            {
                SelectedChessPiece = TileDic[key].TileOccupier;
                lblSelectedChessPiece.Text = SelectedChessPiece.Name;
                SelectedChessPiece.Move(TileDic);
            }
            //otherwise it will place the selected chess piece on a tile that is placeable
            else if (TileDic[key].isPlaceable)
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
            else
            {
                SelectedChessPiece = null;
            }

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
            whiteTw.Move(TileDic);
            UpdateManager();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            whiteKgt.SetPos(tiles[3]);
            //whiteQwn.SetPos(tiles[4]);
            //whiteBis.SetPos(tiles[4]);
            //whiteTw.SetPos(tiles[4]);
            UpdateManager();
        }
    }

    #endregion
}