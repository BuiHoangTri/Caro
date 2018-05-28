using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Caro
{
    public class ChessBoardManager
    {
        #region Properties
        private Panel chessBoard;
        public Panel ChessBoard
        {
            get
            {
                return chessBoard;
            }

            set
            {
                chessBoard = value;
            }
        }

        private List<Player> player;
        public List<Player> Player
        {
            get
            {
                return player;
            }

            set
            {
                player = value;
            }
        }

        private int currentPlayer;
        public int CurrentPlayer
        {
            get
            {
                return currentPlayer;
            }

            set
            {
                currentPlayer = value;
            }
        }

        private TextBox playerName;
        public TextBox PlayerName
        {
            get
            {
                return playerName;
            }

            set
            {
                playerName = value;
            }
        }

        private PictureBox playerMark;
        public PictureBox PlayerMark
        {
            get
            {
                return playerMark;
            }

            set
            {
                playerMark = value;
            }
        }
        #endregion

        #region Initialize
        public ChessBoardManager(Panel chessBoard, TextBox playerName, PictureBox mark)
        {
            this.ChessBoard = chessBoard;
            this.Player = new List<Caro.Player>()
            {
                new Player ("HowKteam", Image.FromFile(Application.StartupPath + "\\Resources\\P1.png")) ,
                new Player ("Education", Image.FromFile(Application.StartupPath + "\\Resources\\P2.png"))
            };
            this.CurrentPlayer = 0;
            this.PlayerName = playerName;
            this.PlayerMark = mark;

            ChangePlayer();
        }
        #endregion

        #region Method
        public void DrawChessBoard()
        {
            for (int i = 0; i < Cons.CHESS_BOARD_HEIGHT; i++)
            {                
                for (int j = 0; j < Cons.CHESS_BOARD_WIDTH; j++)
                {
                    Button btn = new Button()
                    {
                        Width = Cons.CHESS_WIDTH,
                        Height = Cons.CHESS_HEIGHT,
                    };
                    btn.Location = new Point(j * Cons.CHESS_WIDTH, i * Cons.CHESS_HEIGHT);
                    btn.BackgroundImageLayout = ImageLayout.Stretch;
                    btn.Click += Btn_Click;
                    ChessBoard.Controls.Add(btn);
                }
            }
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            if (btn.BackgroundImage != null) return;

            Mark(btn);
            ChangePlayer();
        }

        private void Mark(Button btn)
        {
            btn.BackgroundImage = Player[CurrentPlayer].Mark;
            CurrentPlayer = CurrentPlayer == 1 ? 0 : 1;
        }

        private void ChangePlayer()
        {
            PlayerName.Text = Player[CurrentPlayer].Name;
            PlayerMark.Image = Player[CurrentPlayer].Mark;
        }
        #endregion
    }
}
