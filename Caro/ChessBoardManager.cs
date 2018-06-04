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

        private List<List<Button>> matrix;
        public List<List<Button>> Matrix
        {
            get
            {
                return matrix;
            }

            set
            {
                matrix = value;
            }
        }
        
        private event EventHandler playerMarked;
        public event EventHandler PlayerMarked
        {
            add
            {
                playerMarked += value;
            }
            remove
            {
                playerMarked -= value;
            }
        }

        private event EventHandler endedGame;
        public event EventHandler EndedGame
        {
            add
            {
                endedGame += value;
            }
            remove
            {
                endedGame -= value;
            }
        }

        private Stack<PlayerInfo> playTimeLine;
        public Stack<PlayerInfo> PlayTimeLine
        {
            get
            {
                return playTimeLine;
            }

            set
            {
                playTimeLine = value;
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
            
            this.PlayerName = playerName;
            this.PlayerMark = mark;
                        
        }
        #endregion

        #region Method
        public void DrawChessBoard()
        {
            ChessBoard.Enabled = true;
            chessBoard.Controls.Clear();
            PlayTimeLine = new Stack<PlayerInfo>();
            this.CurrentPlayer = 0;
            ChangePlayer();

            Matrix = new List<List<Button>>();

            for (int i = 0; i < Cons.CHESS_BOARD_HEIGHT; i++)
            {
                Matrix.Add(new List<Button>());
                for (int j = 0; j < Cons.CHESS_BOARD_WIDTH; j++)
                {
                    Button btn = new Button()
                    {
                        Width = Cons.CHESS_WIDTH,
                        Height = Cons.CHESS_HEIGHT,
                        Location = new Point(j * Cons.CHESS_WIDTH, i * Cons.CHESS_HEIGHT),
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Tag = i.ToString()
                    };  
                    btn.Click += Btn_Click;

                    ChessBoard.Controls.Add(btn);
                    Matrix[i].Add(btn);
                }
            }
        }
        
        private void Btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            if (btn.BackgroundImage != null) return;

            Mark(btn);
            PlayTimeLine.Push(new PlayerInfo(GetChessPoint(btn), CurrentPlayer));
            CurrentPlayer = CurrentPlayer == 1 ? 0 : 1;
            ChangePlayer();

            if (playerMarked != null)
                playerMarked(this, new EventArgs());

            if (isEndGame(btn))
            {
                EndGame();
            }            
        }

        private bool isEndGame(Button btn)
        {
            return isEndHorizontal(btn) || isEndVertical(btn) || isEndPrimary(btn) || isEndSub(btn);
        }

        private Point GetChessPoint(Button btn)
        {
            // lấy tọa độ button nằm trong matrix
            int vertical = Convert.ToInt32(btn.Tag);
            int horizontal = Matrix[vertical].IndexOf(btn);
            return new Point(horizontal, vertical);
        }

        /// <summary>
        /// 水平行で終わり　「Kết thúc tại hàng ngang」
        /// </summary>
        /// <param name="btn"></param>
        /// <returns></returns>
        private bool isEndHorizontal(Button btn)
        {
            Point point = GetChessPoint(btn);

            int countLeft = 0;
            for (int i = point.X; i >= 0; i--)
            {
                if (Matrix[point.Y][i].BackgroundImage == btn.BackgroundImage) ++countLeft;
                else break;
            }

            int countRight = 0;
            for (int i = point.X + 1; i < Cons.CHESS_BOARD_WIDTH; i++)
            {
                if (Matrix[point.Y][i].BackgroundImage == btn.BackgroundImage) ++countRight;
                else break;
            }

            return (countLeft + countRight) == 5;
        }

        /// <summary>
        /// 垂直列でおわり「Kết thúc tại cột dọc」
        /// </summary>
        /// <param name="btn"></param>
        /// <returns></returns>
        private bool isEndVertical(Button btn)
        {
            Point point = GetChessPoint(btn);

            int countTop = 0;
            for (int i = point.Y; i >= 0; i--)
            {
                if (Matrix[i][point.X].BackgroundImage == btn.BackgroundImage) ++countTop;
                else break;
            }

            int countBottom = 0;
            for (int i = point.Y + 1; i < Cons.CHESS_BOARD_HEIGHT; i++)
            {
                if (Matrix[i][point.X].BackgroundImage == btn.BackgroundImage) ++countBottom;
                else break;
            }

            return (countTop + countBottom) == 5;
        }

        /// <summary>
        /// 主対角線でおわり「Kết thúc tại đường chéo chính」
        /// </summary>
        /// <param name="btn"></param>
        /// <returns></returns>
        private bool isEndPrimary(Button btn)
        {
            Point point = GetChessPoint(btn);

            int countTop = 0;
            for (int i = 0; i <= point.X; i++)
            {
                if (point.X - i < 0 || point.Y - i < 0) break;
                if (Matrix[point.Y - i][point.X - i].BackgroundImage == btn.BackgroundImage) ++countTop;
                else break;
            }

            int countBottom = 0;
            for (int i = 1; i <= Cons.CHESS_BOARD_WIDTH - point.X; i++)
            {
                if (point.X + i >= Cons.CHESS_BOARD_WIDTH || point.Y + i >= Cons.CHESS_BOARD_HEIGHT) break;
                if (Matrix[point.Y + i][point.X + i].BackgroundImage == btn.BackgroundImage) ++countTop;
                else break;
            }

            return (countTop + countBottom) == 5;
        }

        /// <summary>
        /// サブ対角線でおわり「Kết thúc tại đường chéo phụ」
        /// </summary>
        /// <param name="btn"></param>
        /// <returns></returns>
        private bool isEndSub(Button btn)
        {
            Point point = GetChessPoint(btn);

            int countTop = 0;
            for (int i = 0; i <= point.X; i++)
            {
                if (point.X +i > Cons.CHESS_BOARD_WIDTH || point.Y - i < 0) break;
                if (Matrix[point.Y - i][point.X + i].BackgroundImage == btn.BackgroundImage) ++countTop;
                else break;
            }

            int countBottom = 0;
            for (int i = 1; i <= Cons.CHESS_BOARD_WIDTH - point.X; i++)
            {
                if (point.X - i < 0 || point.Y + i >= Cons.CHESS_BOARD_HEIGHT) break;
                if (Matrix[point.Y + i][point.X - i].BackgroundImage == btn.BackgroundImage) ++countTop;
                else break;
            }

            return (countTop + countBottom) == 5;
        }

        public void EndGame()
        {
            if (endedGame != null)
                endedGame(this, new EventArgs());
        }

        public bool Undo()
        {
            if (PlayTimeLine.Count <= 0) return false;

            PlayerInfo oldPoint = PlayTimeLine.Pop();
            Button btn = Matrix[oldPoint.Point.Y][oldPoint.Point.X];
            btn.BackgroundImage = null;
            
            if ((PlayTimeLine.Count <= 0)) currentPlayer = 0;
            else
            {
                oldPoint = PlayTimeLine.Peek();
                currentPlayer = oldPoint.CurrentPlayer == 1 ? 0 : 1;
            }
            ChangePlayer();

            return true;
        }

        private void Mark(Button btn)
        {
            btn.BackgroundImage = Player[CurrentPlayer].Mark;            
        }

        private void ChangePlayer()
        {
            PlayerName.Text = Player[CurrentPlayer].Name;
            PlayerMark.Image = Player[CurrentPlayer].Mark;
        }
        #endregion
    }
}
