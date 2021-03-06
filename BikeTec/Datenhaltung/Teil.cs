using System;
using System.Collections.Generic;
using System.Text;

namespace Tool
{
    public abstract class Teil
    {
        protected int nr;
        protected string bezeichnung;
        protected int lagerstand;
        protected double lagerpreis;
        protected string verwendung;
        protected int pufferwert;
        protected int warteschlange;

        protected int verbrauchAktuell;
        protected int verbrauchPrognose1;
        protected int verbrauchPrognose2;
        protected int verbrauchPrognose3;
        protected int direktVerkaufMenge;
        protected int direktVerkaufPreis;
        protected int direktVerkaufKonventionalstrafe;

        #region getter / setter

        public Teil(int nummer, string bez)
        {
            this.nr = nummer;
            this.Lagerstand = 0;
            this.pufferwert = 0;
            this.verbrauchAktuell = 0;
            this.verbrauchPrognose1 = 0;
            this.verbrauchPrognose2 = 0;
            this.verbrauchPrognose3 = 0;
            this.direktVerkaufMenge = 0;
            this.direktVerkaufPreis = 0;
            this.direktVerkaufKonventionalstrafe = 0;
        }

        /// <summary>
        /// Gibt die nummer des Teils zurück 
        /// </summary>
        /// <value>The nummer.</value>
        public int Nummer
        {
            get
            {
                return this.nr;
            }
        }

        public double Lagerpreis
        {
            get { return this.lagerpreis; }
            set { this.lagerpreis = value; }
        }

        public int Lagerstand
        {
            get
            {
                return lagerstand;
            }
            set
            {
                lagerstand = value;
            }
        }

        public int Warteschlange
        {
            get
            {
                return warteschlange;
            }
            set
            {
                warteschlange = value;
            }
        }

        public int VerbrauchAktuell
        {

            get
            {
                return this.verbrauchAktuell;
            }
            set
            {
                this.verbrauchAktuell = value;
            }
        }

        public int DirektVerkaufMenge
        {

            get
            {
                return this.direktVerkaufMenge;
            }
            set
            {
                this.direktVerkaufMenge = value;
            }
        }

        public int DirektVerkaufPreis
        {

            get
            {
                return this.direktVerkaufPreis;
            }
            set
            {
                this.direktVerkaufPreis = value;
            }
        }
        public int DirektVerkaufKonventionalstrafe
        {

            get
            {
                return this.direktVerkaufKonventionalstrafe;
            }
            set
            {
                this.direktVerkaufKonventionalstrafe = value;
            }
        }

        public int VerbrauchPrognose1
        {
            get
            {
                return this.verbrauchPrognose1;
            }
            set
            {
                this.verbrauchPrognose1 = value;
            }
        }

        public int VerbrauchPrognose2
        {
            get
            {
                return this.verbrauchPrognose2;
            }
            set
            {
                this.verbrauchPrognose2 = value;
            }
        }

        public int VerbrauchPrognose3
        {
            get
            {
                return this.verbrauchPrognose3;
            }
            set
            {
                this.verbrauchPrognose3 = value;
            }
        }

        public int Pufferwert
        {
            get
            {
                if (pufferwert > 0)
                {
                    //auskommentiert
                 //   Console.WriteLine();
                }

                return this.pufferwert;
            }
            set
            {
                this.pufferwert = value;
            }
        }

        public string Verwendung
        {
            get { return this.verwendung; }
            set
            {
                if (value == "K" || value == "D" || value == "H" || value == "KDH")
                {
                    verwendung = value;
                }
                else
                {
                    throw new InputException("Bei dem Teil " + this.nr + " ist eine nicht zulässige Verwendung eingegeben (" + value + ")");
                }
            }
        }


        /// <summary>
        /// Gibt die nummer des Teils zurück 
        /// </summary>
        /// <value>The nummer.</value>
        public string Bezeichnung
        {
            get
            {
                return this.bezeichnung;
            }
            set
            {
                this.bezeichnung = value;
            }
        }

        #endregion

        public int GetHashcode()
        {
            return this.Nummer.GetHashCode();
        }

        public bool Equals(Teil k)
        {
            if (this.nr == k.nr)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
