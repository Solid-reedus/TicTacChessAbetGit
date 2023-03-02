
using System.Numerics;
using System.Reflection;
using System.Windows.Forms;

namespace TicTacChessAbet
{
    public partial class Form1 : Form
    {
        // --- !!! important make exeption handling for chess pieces if they are on the same tile

        IchessPiece SelectedChessPiece;

        Dictionary<(int, int), Tile> TileDic = new Dictionary<(int, int), Tile>();


        int rowIndex = 0;
        int colIndex = 0;

        TowerChessPiece blackTw = new TowerChessPiece("blackTw", true);
        KnightChessPiece blackKgt = new KnightChessPiece("blackKgt", true, 1, 2);
        QueenChessPiece blackQwn = new QueenChessPiece("blackQwn", false, 1, 2);

        KnightChessPiece whiteKgt = new KnightChessPiece("whiteKgt", false, 1, 2);
        QueenChessPiece whiteQwn = new QueenChessPiece("whiteQwn", true, 1, 2);

        TowerChessPiece whiteTw = new TowerChessPiece("whiteTw", false);

        BishopChessPiece whiteBis = new BishopChessPiece("whiteBis", false);

        //List<IchessPiece> pieces;
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

            /*
            TowerChessPiece blackTw = new TowerChessPiece("blackTw", true, 1, 2);
            KnightChessPiece blackKgt = new KnightChessPiece("blackKgt", true, 1, 2);
            QueenChessPiece blackQwn = new QueenChessPiece("blackQwn", false, 1, 2);

            TowerChessPiece whiteTw = new TowerChessPiece("whiteTw", false, 1, 2);
            KnightChessPiece whiteKgt = new KnightChessPiece("whiteKgt", false, 1, 2);
            QueenChessPiece whiteQwn = new QueenChessPiece("whiteQwn", true, 1, 2);
            */


            tiles[1].TileOccupier = whiteTw;
            tiles[0].TileOccupier = whiteBis;

            //tiles[7].TileOccupier = blackTw;
            //tiles[3].TileOccupier = whiteQwn;
            //
            //tiles[8].TileOccupier = blackKgt;
            //tiles[7].TileOccupier = whiteKgt;
            //tiles[6].TileOccupier = blackQwn;

            foreach (var item in groupBox1.Controls.OfType<Panel>())
            {
                //richTextBox1.AppendText("\n" + item.Name);
            }

            //Panel pnl = TileDic[(rowIndex, colIndex)];
            richTextBox1.AppendText(TileDic[(0, 0)].Name + "\n");
            richTextBox1.AppendText(TileDic[(0, 1)].Name + "\n");
            richTextBox1.AppendText(TileDic[(0, 2)].Name + "\n");

            richTextBox1.AppendText(TileDic[(1, 0)].Name + "\n");
            richTextBox1.AppendText(TileDic[(1, 1)].Name + "\n");
            richTextBox1.AppendText(TileDic[(1, 2)].Name + "\n");

            richTextBox1.AppendText(TileDic[(2, 0)].Name + "\n");
            richTextBox1.AppendText(TileDic[(2, 1)].Name + "\n");
            richTextBox1.AppendText(TileDic[(2, 2)].Name + "\n");

            whiteTw.SetPos(tiles[1]);
            whiteBis.SetPos(tiles[0]);

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

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateManager();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pnlChessTileA1_Click(object sender, EventArgs e)
        {
            Panel pnl = (Panel)sender;


            for (int i = 0; i < tiles.Count; i++)
            {
                if (tiles[i].Panel == pnl)
                {
                    lblSlectedPiece.Text = tiles[i].TileOccupier?.Name;
                    SelectedChessPiece = tiles[i]?.TileOccupier;
                    break;
                    //pnl.BackgroundImage = Image.FromFile(basePath + tiles[0].TileOccupier.ImgName);
                }
            }
            for (int i = 0; i < tiles.Count; i++)
            {
                if (tiles[i].TileOccupier == null)
                {
                    //tiles[i].Panel.BackColor= Color.Green;
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
            //whiteTw.Move(TileDic);
            whiteBis.Move(TileDic);
            UpdateManager();
        }
    }

    #endregion
}