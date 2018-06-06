using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Caro
{
    public partial class Form1 : Form
    {
        #region Properties
        ChessBoardManager chessBoard;
        SocketManager socket;
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

            socket = new SocketManager();
            NewGame();
        }

        #region Methods
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
            pnlChessBoard.Enabled = false;
            undoToolStripMenuItem.Enabled = false;
            MessageBox.Show("Game over!");            
        }

        private void NewGame()
        {
            prcbCoolDown.Value = 0;
            tmCoolDown.Stop();
            undoToolStripMenuItem.Enabled = true;
            chessBoard.DrawChessBoard();            
        }

        private void Quit()
        {
            Application.Exit();
        }

        private void Undo()
        {
            chessBoard.Undo();
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

        private void btnLan_Click(object sender, EventArgs e)
        {
            socket.IP = txtIP.Text;

            if (!socket.ConnectServer())
            {
                socket.CreateServer();

                Thread listenThread = new Thread(() => {
                    while (true)
                    {
                        Thread.Sleep(500);
                        try
                        {
                            Listen();
                            break;
                        }
                        catch (Exception)
                        {
                            
                        }                        
                    }                    
                });
                listenThread.IsBackground = true;
                listenThread.Start();
            }                
            else
            {
                Thread listenThread = new Thread(() => {
                    Listen();
                });
                listenThread.IsBackground = true;
                listenThread.Start();

                socket.Send("Thông tin từ client");
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            txtIP.Text = socket.GetLocalIPv4(System.Net.NetworkInformation.NetworkInterfaceType.Wireless80211);
            if (string.IsNullOrEmpty(txtIP.Text))
                txtIP.Text = socket.GetLocalIPv4(System.Net.NetworkInformation.NetworkInterfaceType.Ethernet);
        }

        private void Listen()
        {
            string data = (string)socket.Receive();
            MessageBox.Show(data);
        }
        #endregion
    }
}