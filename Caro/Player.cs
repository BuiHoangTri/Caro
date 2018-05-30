using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caro
{
    public class Player
    {
        #region Properties
        private string name; // Ctrl + R + E
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        private Image mark;
        public Image Mark
        {
            get
            {
                return mark;
            }

            set
            {
                mark = value;
            }
        }
        #endregion

        #region Initialize
        public Player(string name, Image mark)
        {
            this.Name = name;
            this.Mark = mark;
        }
        #endregion        
    }
}
