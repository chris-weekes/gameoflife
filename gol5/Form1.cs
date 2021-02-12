using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gol5
{
    public partial class Form1 : Form
    {
        int mX = 500;
        int mY = 250;
        int scale = 3;

        SolidBrush BrushBlack;
        SolidBrush BrushWhite;
        BufferedGraphicsContext currentContext;
        BufferedGraphics myBuffer;

        int[,] boardCache;

        Task worker;
        System.Threading.CancellationTokenSource tokenSource;
        System.Threading.CancellationToken token;

        public Form1()
        {
            InitializeComponent();
        }

        public  void DoWork()
        {

            for (int gen = 0; gen < 10000; gen++)
            {
                if (token.IsCancellationRequested)
                    return;

                Generate(boardCache);
               
            }

         
        }

        private void Run(int[,] board)
        {


            boardCache = board;

            DrawBoard(boardCache);

            tokenSource = new System.Threading.CancellationTokenSource();
            token= tokenSource.Token;
            worker =Task.Run(()=>DoWork(),token );

         

        }

        private int[,] Init()
        {

            currentContext = BufferedGraphicsManager.Current;
            myBuffer = currentContext.Allocate(panel1.CreateGraphics(), panel1.DisplayRectangle);

            BrushBlack = new SolidBrush(Color.DarkGreen);
            BrushWhite = new SolidBrush(Color.WhiteSmoke);

            int[,] board = new int[mX, mY];
            return board;
        }

        public void DrawBoard(int[,] b)
        {
            myBuffer.Graphics.Clear(Color.WhiteSmoke);


            for (int x = 0; x < mX; x++)
            {
                for (int y = 0; y < mY; y++)
                {
                    if (b[x, y] > 0)
                    {
                        myBuffer.Graphics.FillRectangle(BrushBlack, x * scale, y * scale, scale, scale);
                    }
                    //else
                    //{
                    //  //  myBuffer.Graphics.FillRectangle(BrushWhite, x * 5, y * 5, 5, 5); 
                    //}

                }
            }

            myBuffer.Render();

        }

        public void Generate(int[,] b)
        {
            int[,] board = new int[mX, mY];
            int t = 0;
            for (int x = 1; x < mX - 1; x++)
            {
                for (int y = 1; y < mY - 1; y++)
                {
                    t = b[x - 1, y - 1];
                    t += b[x - 1, y];
                    t += b[x - 1, y + 1];
                    t += b[x, y - 1];
                    t += b[x, y + 1];
                    t += b[x + 1, y - 1];
                    t += b[x + 1, y];
                    t += b[x + 1, y + 1];

                    if (b[x, y] == 0 & t == 3)
                        board[x, y] = 1;
                    else if (b[x, y] == 1 & (t == 2 | t == 3))
                        board[x, y] = 1;
                }
            }

            boardCache = board;
            //panel1.Refresh();
            DrawBoard(boardCache);

        }

        private void mnuStart_Click(object sender, EventArgs e)
        {
            // this.DoubleBuffered = true;

            int[,] board = Init();

            Random r = new Random();
            //create random board
            for (int x = 0; x < mX; x++)
            {
                for (int y = 0; y < mY; y++)
                {

                    board[x, y] = r.Next(0, 2);
                }
            }

            Run(board);
        }

        private void mnuStop_Click(object sender, EventArgs e)
        {
            tokenSource.Cancel();
        }

        private void mnuGlider_Click(object sender, EventArgs e)
        {
            int[,] board = Init();

            board[100, 100] = 1;
            board[101, 100] = 1;
            board[100, 101] = 1;
            board[101, 101] = 1;

            board[110, 100] = 1;
            board[110, 101] = 1;
            board[110, 102] = 1;

            board[111, 99] = 1;
            board[111, 103] = 1;

            board[112, 98] = 1;
            board[112, 104] = 1;
            board[113, 98] = 1;
            board[113, 104] = 1;

            board[114, 101] = 1;

            board[115, 99] = 1;
            board[115, 103] = 1;

            board[116, 100] = 1;
            board[116, 101] = 1;
            board[116, 102] = 1;

            board[117, 101] = 1;

            board[120, 98] = 1;
            board[120, 99] = 1;
            board[120, 100] = 1;

            board[121, 98] = 1;
            board[121, 99] = 1;
            board[121, 100] = 1;

            board[122, 97] = 1;
            board[122, 101] = 1;

            board[124, 97] = 1;
            board[124, 96] = 1;
            board[124, 102] = 1;
            board[124, 101] = 1;

            board[134, 98] = 1;
            board[134, 99] = 1;
            board[135, 98] = 1;
            board[135, 99] = 1;

            Run(board);
        }

        private void mnuInfinite_Click(object sender, EventArgs e)
        {
            int[,] board = Init();

            board[99, 100] = 1;

            board[101, 100] = 1;
            board[101, 99] = 1;

            board[103, 98] = 1;
            board[103, 97] = 1;
            board[103, 96] = 1;

            board[105, 97] = 1;
            board[105, 96] = 1;
            board[105, 95] = 1;

            board[106, 96] = 1;


            Run(board);
        }

        private void mnuPent_Click(object sender, EventArgs e)
        {
            int[,] board = Init();

            board[100, 100] = 1;
            board[101, 100] = 1;
            board[101, 99] = 1;
            board[102, 99] = 1;
            board[101, 101] = 1;

            Run(board);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            tokenSource.Cancel();
        }
    }
}
