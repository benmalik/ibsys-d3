using System;
using System.Collections.Generic;
using System.Text;

namespace Tool
{
    public class Bestellverwaltung
    {
        DataContainer data = DataContainer.Instance;
        double lagerKostenHebel = 0.06;

        public Bestellverwaltung()
        {

        }

        public void ClearData()
        {
            data.Bestellung.Clear();
        }

        public void erzeugen_bestellungenlist()
        {

            foreach (Kaufteil k in data.KaufteilList)
            {
                Boolean eilBestellung = false;

                // Nichts bestellen in Periode 0
                int[] endErgebnis = extendedWagnerWithinAlgorithm(k, false, true);

                // Normale Bestellung
                int[] ergebnis = extendedWagnerWithinAlgorithm(k, false, false);
                endErgebnis = (endErgebnis[0] < ergebnis[0]) ? endErgebnis : ergebnis;

                // Eil Bestellung + nichts bestellen in Periode 0
                ergebnis = extendedWagnerWithinAlgorithm(k, true, true);
                if (endErgebnis[0] > ergebnis[0])
                {
                    eilBestellung = true;
                    endErgebnis = ergebnis;
                }
                //endErgebnis = (endErgebnis[0] < ergebnis[0]) ? endErgebnis : ergebnis;

                // Eil Bestellung 
                // Fals Normalbestellungen zu lange dauern soll Eilbestellt werden
                ergebnis = extendedWagnerWithinAlgorithm(k, true, false);
                if (endErgebnis[0] > 100000 || endErgebnis[0] > ergebnis[0])
                {
                    eilBestellung = true;
                    endErgebnis = ergebnis;
                }
                //endErgebnis = (endErgebnis[0] > 100000 || ergebnis[0] < endErgebnis[0] ) ? ergebnis : endErgebnis;

                if (endErgebnis[1] != 0 && k.Diskontmenge != 0)
                {
                    this.data.Bestellung.Add(new Bestellposition(k, endErgebnis[1], eilBestellung));
                }
            }
        }

        /// <summary>
        /// Wagner Within Algorithmus.
        /// Lieferzeit eingenommen.
        /// Lagerkosten auf verzoegerte Lieferzeit bezogen.
        /// 
        /// Falls negativer Lagerbestand auftritt werden die kosten * 1000 genommen.
        /// So werden Eilbestellungen attraktiv negativer Lagerbestand bei Eilbestellungen * 10
        /// </summary>
        /// <param name="k">Kaufteil</param>
        /// <param name="eil">Eilbestellung</param>
        /// <param name="nichtsBestellenInPeriode1">nichtsBestellenInPeriode1</param>
        /// <returns></returns>
        private int[]  extendedWagnerWithinAlgorithm(Kaufteil k, Boolean eil, Boolean nichtsBestellenInPeriode1)
        {
            //akt. Lagerstand zu Periodenbeginn und ausstehende Lieferung
            int lagerBestand = k.Lagerstand + k.ErwarteteBestellung;
            // Spalten: 4 Perioden | Zeilen: (0) Lagerkosten + Bestellkosten, (1), (2), (3), (4) Lagerkosten
            int[,] ww = new int[4, 5];
            int[] bestPeriodOption = new int[4] { 0, 0, 0, 0 };

            // Keine Bestellung in Periode 1
            if (nichtsBestellenInPeriode1)
            {
                for (int periode = 0; periode < 4; periode++)
                {
                    double lagerKosten = lagerKostenBerechnung(k, 0, periode, 0, new int[4, 2], eil);
                    ww[periode, 4] = Convert.ToInt32(lagerKosten);
                }
            }
            else
            {
                for (int periode = 0; periode < 4; periode++)
                {
                    ww[periode, 4] = 200000;
                }
            }
            


            for (int zeitpunkt = 0; zeitpunkt < 4; zeitpunkt++)
            {
                for (int periode = zeitpunkt; periode < 4; periode++)
                {
                    // Spalten: | Zeilen: 
                    int[,] vorhergehendeBestellungen = new int[4,4];                
                    if (periode > 0)
                        vorhergehendeBestellungen = getKostenUndMenge(ww, bestPeriodOption, periode, k);
                    double bestPrePeriodCosts = (zeitpunkt > 0) ? ww[zeitpunkt - 1, bestPeriodOption[zeitpunkt - 1]] : 0;
                    double bestellKosten = eil ? k.Bestellkosten * 10 : k.Bestellkosten;
                    
                    int bestellMenge = bestellMengenBerechnung(k, zeitpunkt, periode);

                    // hole vorherige Bestellungen und Kosten
                    

                    // funktion wird bestraft wenn lager unterschritten wird
                    double lagerKosten = lagerKostenBerechnung(k, zeitpunkt, periode, bestellMenge, vorhergehendeBestellungen, eil);

                    // Kosten sind fixe Bestellkosten + lagerkosten + Kosten der besten Option der Vorperiode
                    ww[periode, zeitpunkt] = Convert.ToInt32(bestellKosten + lagerKosten + bestPrePeriodCosts);
                }
                bestPeriodOption[zeitpunkt] = findBestPeriodOption(ww, zeitpunkt);
            }

            int[,] result = getKostenUndMenge(ww, bestPeriodOption, 3, k);
            // GesamtKosten berechnen
            int gesamtKosten = ww[3, bestPeriodOption[3]];

            return new int[2] { gesamtKosten, result[0, 1] };
        }

        private double lagerKostenBerechnung(Kaufteil k, int zeitpunkt, int periode, int bestellMenge, int[,] vorhergehendeBestellungen, Boolean eil) 
        {
            int[] periodenBedarf = new int[4] { k.VerbrauchAktuell, k.VerbrauchPrognose1, k.VerbrauchPrognose2, k.VerbrauchPrognose3 };
            double lagerBestand = k.Lagerstand + k.ErwarteteBestellung;

            
            // Verbrauch vom Bestand abziehen
            for (int i = 0; i < zeitpunkt; i++)
            {
                lagerBestand -= periodenBedarf[i];
            }

            // einkommenden Bestellungen
            int[] bestellungen = new int[10];
            int lieferZeit = (int) Math.Round(k.Abweichung_lieferdauer + k.Lieferdauer);
            if (eil)
            {
                lieferZeit = (int)Math.Round(k.Lieferdauer / 2);
            }

            for (int period = 0; period < 4; period++) {
                bestellungen[period + lieferZeit] = vorhergehendeBestellungen[period, 1];
            }
            //bestellungen[(periode + lieferZeit)] += bestellMenge;

            double lagerKosten = 0;
            for (int i = zeitpunkt; i <= periode; i++)
            {
                // udpate Lagerbestand mit einkommenden Bestellungen
                for (int vorZeitpunktBestellungen = 0; vorZeitpunktBestellungen < zeitpunkt; vorZeitpunktBestellungen++)
                    lagerBestand += bestellungen[vorZeitpunktBestellungen];
                lagerBestand += bestellungen[i];

                if (lagerBestand - (periodenBedarf[i]) > 0)
                {
                    lagerKosten += (lagerBestand - (periodenBedarf[i] / 2)) * lagerKostenHebel * k.Preis;
                    lagerBestand -= periodenBedarf[i];
                }
                else
                {
                    lagerKosten += 100000;
                }
            }
            return lagerKosten;
        }

        private int bestellMengenBerechnung(Kaufteil k, int zeitpunkt, int periode)
        {
            int[] periodenBedarf = new int[4] { k.VerbrauchAktuell, k.VerbrauchPrognose1, k.VerbrauchPrognose2, k.VerbrauchPrognose3 };

            int bestellMenge = 0;
            for (int _periode = zeitpunkt; _periode <= periode; _periode++)
            {
                bestellMenge += periodenBedarf[_periode];
            } 
            return bestellMenge;
        }

        private int findBestPeriodOption(int[,] ww, int period)
        {
            int bestOption = 4;
            // keine Bestellung als standard Wert
            double costValue = ww[period,4];

            for (int i = 0; i <= period; i++)
            {
                if (ww[period,i] < costValue){
                    bestOption = i;
                    costValue = ww[period, i];
                }
            }
            return bestOption;
        }

        // 0: Kosten
        // 1: Menge
        private int[,] getKostenUndMenge(int[,] ww, int[] bestOptions, int periode, Kaufteil k)
        {
            int[,] ergebnis = new int[4,2];
            int bestellZeitPunkt = findBestPeriodOption(ww, periode);
            int[] periodenBedarf = new int[4] { k.VerbrauchAktuell, k.VerbrauchPrognose1, k.VerbrauchPrognose2, k.VerbrauchPrognose3 };

            int lastStep = periode;
            while (bestellZeitPunkt > 0)
            {
                if (ww[periode - 1, bestellZeitPunkt] == 0)
                {
                    int bestellMenge = bestellMengenBerechnung(k, bestellZeitPunkt, lastStep);
                    ergebnis[periode, 1] = bestellMenge;
                    ergebnis[periode, 0] = ww[lastStep, bestellZeitPunkt];
                    bestellZeitPunkt = bestOptions[periode - 1];
                    lastStep = periode - 1;
                }
                periode = periode - 1;

                // fals keine Bestellung das sinnvollste ist
                if (bestellZeitPunkt == 4) {
                    ergebnis[0,1] = 0;
                    break;
                }       
            }

            // Menge berechnen fals etwas bestellt wird
            if (bestellZeitPunkt != 4)
            {
                int bestellMenge = bestellMengenBerechnung(k, bestellZeitPunkt, periode);
                ergebnis[0,1] = bestellMenge;
            }

            // Kosten auslesen
            ergebnis[0,0] = ww[periode, bestellZeitPunkt];

            return ergebnis;
        }
    }

}