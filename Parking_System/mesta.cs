using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking_System
{
    class mesta
    {
        public string oznaka;
        public int trenutnoStanje;
        public int id;
        public int x;
        internal int y;

        public int Cols { get; internal set; }
        public int Rows { get; internal set; }
       // public static PictureBox Parent { get; internal set; }

        public override string ToString()
        {
            return oznaka.ToString();

        }
    }
}
