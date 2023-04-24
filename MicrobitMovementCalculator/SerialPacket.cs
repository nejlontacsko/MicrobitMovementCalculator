using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrobitMovementCalculator
{
    internal class SerialPacket
    {
        public int t { get; set; }
        public int s { get; set; }
        public int ID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public override string ToString()
        {
            TimeSpan span = new TimeSpan(t*TimeSpan.TicksPerMillisecond);
            string serNum = "0x" + Convert.ToString(s, 16).PadLeft(8, '0');
            return string.Join(" ", new object[] { span, serNum, ID, X, Y, Z });
        }
    }
}
