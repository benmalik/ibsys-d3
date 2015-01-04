using System;
using System.Collections.Generic;
using System.Text;

namespace Tool
{
    class UnknownTeilException:Exception
    {
        int nr;

        public UnknownTeilException(int n)
        {
            this.nr = n;
        }

        public int Nummer
        {
            get { return this.nr; }
            set { this.nr = value; }
        }

    }
}
