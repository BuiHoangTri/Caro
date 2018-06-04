using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Caro
{
    public partial class Form1 : Form
    {
        #region Properties
        ChessBoardManager chessBoard;
        #endregion

        public Form1()
        {
            InitializeComponent();

            chessBoard = new ChessBoardManager(pnlChessBoard, txtPlayerName, pctbMark);
            chessBoard.EndedGame += ChessBoard_EndedGame;
            chessBoard.PlayerMarked += ChessBoard_PlayerMarked;            

            prcbCoolDown.Step = Cons.COOL_DOWN_STEP;
            prcbCoolDown.Maximum = Cons.COOL_DOWN_TIME;
            prcbCoolDown.Value = 0;

            tmCoolDown.Interval = Cons.COOL_DOWN_INTERVAL;

            NewGame();
        }

        #region Method
        private void ChessBoard_PlayerMarked(object sender, EventArgs e)
        {
            tmCoolDown.Start();
            prcbCoolDown.Value = 0;
        }

        private void ChessBoard_EndedGame(object sender, EventArgs e)
        {
            EndGame();
        }

        private void tmCoolDown_Tick(object sender, EventArgs e)
        {
            prcbCoolDown.PerformStep();

            if(prcbCoolDown.Value >= prcbCoolDown.Maximum)
                EndGame();
        }

        private void EndGame()
        {
            tmCoolDown.Stop();
            MessageBox.Show("Game over!");
            pnlChessBoard.Enabled = false;
        }

        private void NewGame()
        {
            prcbCoolDown.Value = 0;
            tmCoolDown.Stop();
            chessBoard.DrawChessBoard();            
        }

        private void Quit()
        {
            Application.Exit();
        }

        private void Undo()
        {

        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Quit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn thoát?", "Thông báo", MessageBoxButtons.OKCancel) != DialogResult.OK)
                e.Cancel = true;
        }
        #endregion
    }
}
