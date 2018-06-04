using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caro
{
    public class PlayerInfo
    {        
        public Point Point
        {
            get
            {
                return point;
            }

            set
            {
                point = value;
            }
        }
        private Point point;

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
        private int currentPlayer;

        public PlayerInfo(Point point, int currP)
        {
            this.Point = point;
            this.CurrentPlayer = currP;
        }
    }
}
