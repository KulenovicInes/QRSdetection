using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRSdetection
{
    public abstract class Ulaz
    {
        public abstract void Read(int channel);
        public abstract void Stop();
    }
}
