using System;
using System.Collections.Generic;
using System.Text;

namespace Tool
{
    public class Bestellposition
    {
        private Kaufteil kaufteil;
        private int menge;
        private bool eil; 

        public Bestellposition(Kaufteil k, int menge_, bool eil_)
        {
            this.kaufteil = k;
            this.menge = menge_;
            this.eil = eil_;
        }



        public Kaufteil Kaufteil
        {
            get
            {
                return this.kaufteil;
            }
            set
            {
                this.kaufteil = value;
            }
        }

        public int Menge
        {
            get
            {
                return this.menge;
            }
            set
            {
                this.menge = value;
            }
        }

        public bool Eil
        {
            get
            {
                return this.eil;
            }
            set
            {
                this.eil = value;
            }
        }

        public int OutputEil
        {
            get
            {
                if (this.eil)
                {
                    return 4;
                }
                else
                {
                    return 5;
                }
            }
        }
    }
}
