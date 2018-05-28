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
        #endregion

        #region Initialize
        public ChessBoardManager(Panel chessBoard)
        {
            this.ChessBoard = chessBoard;
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
                    ChessBoard.Controls.Add(btn);
                }
            }
        }       
        #endregion        
    }
}
