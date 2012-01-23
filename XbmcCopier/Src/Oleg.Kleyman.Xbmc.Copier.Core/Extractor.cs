using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oleg.Kleyman.Xbmc.Copier.Core
{
    public abstract class Extractor
    {
        public abstract void Extract(string target, string destination);
    }
}
