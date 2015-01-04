using System;
using System.Collections.Generic;
using System.Text;

namespace Tool
{
    public class Stammdaten
    {
        private static bool[] isKTeil = {false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
                                       false,false,false,false,false,true,true,true,true,true,false,true,true,false,false,false,
                                       true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,
                                       false,false,false,true,true,false,false,false,true,true,true};
        //Bestellkosten des KTeils
        public static double[] BestellkostenKTeil = {0,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,0, 50,50,50,100,50,0,75,50,0,0,
                                         0,50,75,50,75,100,50,50,75,50, 50,50,75,50,50,50,50,75,0,0, 0,50,50,0,0,0,50,50,50};
        //Preis des KTeils
        public static double[] PreisKTeil = {0,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,0, 5.00,6.50,6.50,0.06,0.06,0,0.1,1.2,0,0,
                                          0,0.75,22,0.1,1,8,1.5,1.5,1.5,2.5, 0.06,0.1,5,0.5,0.06,0.1,3.5,1.5,0,0, 0,22,0.1,0,0,0,22,0.1,0.15};
        //Verwendung des Teils
        private static string[] VerwendungTeil = {"K","D","H","K","D","H","K","D","H","K","D","H","K","D","H","KDH","KDH","K","D","H","K","D","H","KDH",
                                          "KDH","KDH","KDH","KDH","H","H","H","KDH","H","H","KDH","KDH","KDH","KDH","KDH","KDH","KDH","KDH","KDH",
                                          "KDH","KDH","KDH","KDH","KDH","K","K","K","K","K","D","D","D","D","D","KDH"};
        //Bezeichnung des Teils
        public static string[] TBez = {"Kinderfahrrad","Damenfahrrad","Herrenfahrrad","Hinterradgruppe K","Hinterradgruppe D",
                                       "Hinterradgruppe H","Vorderradgruppe K","Vorderradgruppe D","Vorderradgruppe H","Schutzblech hinten K",
                                       "Schutzblech hinten D","Schutzblech hinten H","Schutzblech vorne K","Schutzblech vorne D","Schutzblech vorne H",
                                       "Lenker","Sattel","Rahmen K","Rahmen D","Rahmen H","Kette K","Kette D","Kette H","Mutter","Scheibe",
                                       "Pedal","Schraube","Rohr","Vorderrad H","Rahmen u. Räder H","Fahrrad o. Pedal H","Farbe","Felge H",
                                       "Speiche H","Nabe","Freilauf","Gabel","Welle","Blech","Lenker","Mutter","Griff","Sattel",
                                       "Stange","Mutter","Schraube","Zahnkranz","Pedal","Vorderrad K","Rahmen u. Räder K",
                                       "Fahrrad o. Pedal K","Felge K","Speiche K","Vorderrad D","Rahmen u. Räder D","Fahrrad o. Pedal D",
                                       "Felge D","Speiche D","Schweißdraht"};
        //Diskontmenge bei KTeil
        public static int[] DiskontmengeKTeil = {0,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,0, 300,300,300,6100,3600,0,1800,4500,0,0, 
                                       0,2700,900,22000,3600,900,900,300,1800,900, 900,1800,2700,900,900,900,900,1800,0,0,
                                       0,600,22000,0,0,0,600,22000,1800};
        //Bestelldauer bei KTeil
        private static double[] BestelldauerKTeil = {0,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,0, 1.8,1.7,1.2,3.2,0.9,0,0.9,1.7,0,0, 0,2.1,1.9,1.6,2.2,1.2,1.5,1.7,1.5,1.7,
                                        0.9,1.2,2,1,1.7,0.9,1.1,1,0,0, 0,1.6,1.6,0,0,0,1.7,1.6,0.7};
        //Abweichung Bestelldauer bei KTeil
        private static double[] AbweichBestelldauerKTeil = {0,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,0, 0.4,0.4,0.2,0.3,0.2,0,0.2,0.4,0,0, 0,0.5,0.5,0.3,0.4,0.1,0.3,0.4,0.3,0.2,
                                         0.2,0.3,0.5,0.2,0.3,0.3,0.1,0.2,0,0, 0,0.4,0.2,0,0,0,0.3,0.5,0.2};
        //Schlüssel Verbrauch P1
        private static int[] TVerbrauchP1 = {0,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,0, 1,0,0,7,4,0,2,4,0,0, 0,3,0,0,4,1,1,1,2,1,
                                            1,2,1,3,1,1,1,2,0,0, 0,2,72,0,0,0,0,0,2};
        //Schlüssel Verbrauch P2
        private static int[] TVerbrauchP2 = {0,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,0, 0,1,0,7,4,0,2,5,0,0, 0,3,0,0,4,1,1,1,2,1,
                                            1,2,1,3,1,1,1,2,0,0, 0,0,0,0,0,0,2,72,2};
        //Schlüssel Verbrauch P3
        private static int[] TVerbrauchP3 = {0,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,0, 0,0,1,7,4,0,2,6,0,0, 0,3,2,72,4,1,1,1,2,1,
                                            1,2,1,3,1,1,1,2,0,0, 0,0,0,0,0,0,0,0,2};

        DataContainer instance = DataContainer.Instance;

        public void Initialisieren()
        {
            for (int i = 0; i < 59; i++)
            {
                if (isKTeil[i])
                {
                    instance.NewTeil(i + 1, TBez[i], PreisKTeil[i], BestellkostenKTeil[i], BestelldauerKTeil[i], AbweichBestelldauerKTeil[i], DiskontmengeKTeil[i], 0, VerwendungTeil[i]);
                }
                else
                {
                    instance.NewTeil(i + 1, TBez[i], 0, VerwendungTeil[i]);
                }
            }

            for (int c = 1; c < 16; c++)
            {
                if (c != 5)
                {
                    instance.NeuArbeitsplatz(new Arbeitsplatz(c));
                }

            }
            this.InitEteil();
            this.InitializeArbPl();
        }


        /// <summary>
        /// InitEteil Methode
        /// Hilfsmethode - Initialisiert die Graphen für die ETeile
        /// </summary>
        public void InitEteil()
        {
            (instance.GetTeil(16) as ETeil).AddBestandteil(24, 1);
            (instance.GetTeil(16) as ETeil).AddBestandteil(28, 1);
            (instance.GetTeil(16) as ETeil).AddBestandteil(40, 1);
            (instance.GetTeil(16) as ETeil).AddBestandteil(41, 1);
            (instance.GetTeil(16) as ETeil).AddBestandteil(42, 2);
            (instance.GetTeil(17) as ETeil).AddBestandteil(43, 1);
            (instance.GetTeil(17) as ETeil).AddBestandteil(44, 1);
            (instance.GetTeil(17) as ETeil).AddBestandteil(45, 1);
            (instance.GetTeil(17) as ETeil).AddBestandteil(46, 1);
            (instance.GetTeil(26) as ETeil).AddBestandteil(47, 1);
            (instance.GetTeil(26) as ETeil).AddBestandteil(44, 2);
            (instance.GetTeil(26) as ETeil).AddBestandteil(48, 2);

            (instance.GetTeil(2) as ETeil).AddBestandteil(22, 1);
            (instance.GetTeil(2) as ETeil).AddBestandteil(24, 1);
            (instance.GetTeil(2) as ETeil).AddBestandteil(27, 1);
            (instance.GetTeil(2) as ETeil).AddBestandteil(26, 1);
            (instance.GetTeil(2) as ETeil).AddBestandteil(56, 1);

            (instance.GetTeil(56) as ETeil).AddBestandteil(24, 1);
            (instance.GetTeil(56) as ETeil).AddBestandteil(27, 1);
            (instance.GetTeil(56) as ETeil).AddBestandteil(55, 1);
            (instance.GetTeil(56) as ETeil).AddBestandteil(16, 1);
            (instance.GetTeil(56) as ETeil).AddBestandteil(17, 1);

            (instance.GetTeil(55) as ETeil).AddBestandteil(24, 2);
            (instance.GetTeil(55) as ETeil).AddBestandteil(25, 2);
            (instance.GetTeil(55) as ETeil).AddBestandteil(5, 1);
            (instance.GetTeil(55) as ETeil).AddBestandteil(11, 1);
            (instance.GetTeil(55) as ETeil).AddBestandteil(54, 1);
            (instance.GetTeil(11) as ETeil).AddBestandteil(32, 1);
            (instance.GetTeil(11) as ETeil).AddBestandteil(39, 1);
            (instance.GetTeil(5) as ETeil).AddBestandteil(35, 2);
            (instance.GetTeil(5) as ETeil).AddBestandteil(36, 1);
            (instance.GetTeil(5) as ETeil).AddBestandteil(57, 1);
            (instance.GetTeil(5) as ETeil).AddBestandteil(58, 36);
            (instance.GetTeil(54) as ETeil).AddBestandteil(24, 2);
            (instance.GetTeil(54) as ETeil).AddBestandteil(25, 2);
            (instance.GetTeil(54) as ETeil).AddBestandteil(8, 1);
            (instance.GetTeil(54) as ETeil).AddBestandteil(14, 1);
            (instance.GetTeil(54) as ETeil).AddBestandteil(19, 1);
            (instance.GetTeil(8) as ETeil).AddBestandteil(35, 2);
            (instance.GetTeil(8) as ETeil).AddBestandteil(37, 1);
            (instance.GetTeil(8) as ETeil).AddBestandteil(38, 1);
            (instance.GetTeil(8) as ETeil).AddBestandteil(57, 1);
            (instance.GetTeil(8) as ETeil).AddBestandteil(58, 36);
            (instance.GetTeil(19) as ETeil).AddBestandteil(28, 4);
            (instance.GetTeil(19) as ETeil).AddBestandteil(32, 1);
            (instance.GetTeil(19) as ETeil).AddBestandteil(59, 2);
            (instance.GetTeil(14) as ETeil).AddBestandteil(32, 1);
            (instance.GetTeil(14) as ETeil).AddBestandteil(39, 1);

            (instance.GetTeil(1) as ETeil).AddBestandteil(21, 1);
            (instance.GetTeil(1) as ETeil).AddBestandteil(24, 1);
            (instance.GetTeil(1) as ETeil).AddBestandteil(27, 1);
            (instance.GetTeil(1) as ETeil).AddBestandteil(26, 1);
            (instance.GetTeil(1) as ETeil).AddBestandteil(51, 1);

            (instance.GetTeil(51) as ETeil).AddBestandteil(16, 1);
            (instance.GetTeil(51) as ETeil).AddBestandteil(17, 1);
            (instance.GetTeil(51) as ETeil).AddBestandteil(50, 1);
            (instance.GetTeil(51) as ETeil).AddBestandteil(24, 1);
            (instance.GetTeil(51) as ETeil).AddBestandteil(27, 1);
            (instance.GetTeil(50) as ETeil).AddBestandteil(24, 2);
            (instance.GetTeil(50) as ETeil).AddBestandteil(25, 2);
            (instance.GetTeil(50) as ETeil).AddBestandteil(4, 1);
            (instance.GetTeil(50) as ETeil).AddBestandteil(10, 1);
            (instance.GetTeil(50) as ETeil).AddBestandteil(49, 1);

            (instance.GetTeil(10) as ETeil).AddBestandteil(32, 1);
            (instance.GetTeil(10) as ETeil).AddBestandteil(39, 1);
            (instance.GetTeil(4) as ETeil).AddBestandteil(35, 2);
            (instance.GetTeil(4) as ETeil).AddBestandteil(36, 1);
            (instance.GetTeil(4) as ETeil).AddBestandteil(52, 1);
            (instance.GetTeil(4) as ETeil).AddBestandteil(53, 36);
            (instance.GetTeil(49) as ETeil).AddBestandteil(24, 2);
            (instance.GetTeil(49) as ETeil).AddBestandteil(25, 2);
            (instance.GetTeil(49) as ETeil).AddBestandteil(7, 1);
            (instance.GetTeil(49) as ETeil).AddBestandteil(13, 1);
            (instance.GetTeil(49) as ETeil).AddBestandteil(18, 1);
            (instance.GetTeil(7) as ETeil).AddBestandteil(35, 2);
            (instance.GetTeil(7) as ETeil).AddBestandteil(37, 1);
            (instance.GetTeil(7) as ETeil).AddBestandteil(38, 1);
            (instance.GetTeil(7) as ETeil).AddBestandteil(52, 1);
            (instance.GetTeil(7) as ETeil).AddBestandteil(53, 36);
            (instance.GetTeil(18) as ETeil).AddBestandteil(28, 3);
            (instance.GetTeil(18) as ETeil).AddBestandteil(32, 1);
            (instance.GetTeil(18) as ETeil).AddBestandteil(59, 2);
            (instance.GetTeil(13) as ETeil).AddBestandteil(32, 1);
            (instance.GetTeil(13) as ETeil).AddBestandteil(39, 1);

            (instance.GetTeil(3) as ETeil).AddBestandteil(23, 1);
            (instance.GetTeil(3) as ETeil).AddBestandteil(24, 1);
            (instance.GetTeil(3) as ETeil).AddBestandteil(27, 1);
            (instance.GetTeil(3) as ETeil).AddBestandteil(26, 1);
            (instance.GetTeil(3) as ETeil).AddBestandteil(31, 1);

            (instance.GetTeil(31) as ETeil).AddBestandteil(24, 1);
            (instance.GetTeil(31) as ETeil).AddBestandteil(27, 1);
            (instance.GetTeil(31) as ETeil).AddBestandteil(16, 1);
            (instance.GetTeil(31) as ETeil).AddBestandteil(17, 1);
            (instance.GetTeil(31) as ETeil).AddBestandteil(30, 1);
            (instance.GetTeil(30) as ETeil).AddBestandteil(24, 2);
            (instance.GetTeil(30) as ETeil).AddBestandteil(25, 2);
            (instance.GetTeil(30) as ETeil).AddBestandteil(6, 1);
            (instance.GetTeil(30) as ETeil).AddBestandteil(12, 1);
            (instance.GetTeil(30) as ETeil).AddBestandteil(29, 1);
            (instance.GetTeil(12) as ETeil).AddBestandteil(32, 1);
            (instance.GetTeil(12) as ETeil).AddBestandteil(39, 1);
            (instance.GetTeil(6) as ETeil).AddBestandteil(35, 2);
            (instance.GetTeil(6) as ETeil).AddBestandteil(36, 1);
            (instance.GetTeil(6) as ETeil).AddBestandteil(33, 1);
            (instance.GetTeil(6) as ETeil).AddBestandteil(34, 36);
            (instance.GetTeil(29) as ETeil).AddBestandteil(24, 2);
            (instance.GetTeil(29) as ETeil).AddBestandteil(25, 2);
            (instance.GetTeil(29) as ETeil).AddBestandteil(9, 1);
            (instance.GetTeil(29) as ETeil).AddBestandteil(15, 1);
            (instance.GetTeil(29) as ETeil).AddBestandteil(20, 1);
            (instance.GetTeil(9) as ETeil).AddBestandteil(35, 2);
            (instance.GetTeil(9) as ETeil).AddBestandteil(37, 1);
            (instance.GetTeil(9) as ETeil).AddBestandteil(38, 1);
            (instance.GetTeil(9) as ETeil).AddBestandteil(33, 1);
            (instance.GetTeil(9) as ETeil).AddBestandteil(34, 36);
            (instance.GetTeil(20) as ETeil).AddBestandteil(28, 5);
            (instance.GetTeil(20) as ETeil).AddBestandteil(32, 1);
            (instance.GetTeil(20) as ETeil).AddBestandteil(59, 2);
            (instance.GetTeil(15) as ETeil).AddBestandteil(32, 1);
            (instance.GetTeil(15) as ETeil).AddBestandteil(39, 1);

            (instance.GetTeil(26) as ETeil).AddArbeitsplatz(15);
            (instance.GetTeil(26) as ETeil).AddArbeitsplatz(7);
            (instance.GetTeil(16) as ETeil).AddArbeitsplatz(6);
            (instance.GetTeil(16) as ETeil).AddArbeitsplatz(14);
            (instance.GetTeil(17) as ETeil).AddArbeitsplatz(15);

            (instance.GetTeil(2) as ETeil).AddArbeitsplatz(4);
            (instance.GetTeil(56) as ETeil).AddArbeitsplatz(3);
            (instance.GetTeil(55) as ETeil).AddArbeitsplatz(2);
            (instance.GetTeil(11) as ETeil).AddArbeitsplatz(9);
            (instance.GetTeil(11) as ETeil).AddArbeitsplatz(7);
            (instance.GetTeil(11) as ETeil).AddArbeitsplatz(8);
            (instance.GetTeil(11) as ETeil).AddArbeitsplatz(12);
            (instance.GetTeil(11) as ETeil).AddArbeitsplatz(13);
            (instance.GetTeil(5) as ETeil).AddArbeitsplatz(10);
            (instance.GetTeil(5) as ETeil).AddArbeitsplatz(11);
            (instance.GetTeil(54) as ETeil).AddArbeitsplatz(1);
            (instance.GetTeil(8) as ETeil).AddArbeitsplatz(10);
            (instance.GetTeil(8) as ETeil).AddArbeitsplatz(11);
            (instance.GetTeil(19) as ETeil).AddArbeitsplatz(6);
            (instance.GetTeil(19) as ETeil).AddArbeitsplatz(8);
            (instance.GetTeil(19) as ETeil).AddArbeitsplatz(7);
            (instance.GetTeil(19) as ETeil).AddArbeitsplatz(9);
            (instance.GetTeil(14) as ETeil).AddArbeitsplatz(13);
            (instance.GetTeil(14) as ETeil).AddArbeitsplatz(12);
            (instance.GetTeil(14) as ETeil).AddArbeitsplatz(8);
            (instance.GetTeil(14) as ETeil).AddArbeitsplatz(7);
            (instance.GetTeil(14) as ETeil).AddArbeitsplatz(9);

            (instance.GetTeil(1) as ETeil).AddArbeitsplatz(4);
            (instance.GetTeil(51) as ETeil).AddArbeitsplatz(3);
            (instance.GetTeil(50) as ETeil).AddArbeitsplatz(2);
            (instance.GetTeil(10) as ETeil).AddArbeitsplatz(9);
            (instance.GetTeil(10) as ETeil).AddArbeitsplatz(7);
            (instance.GetTeil(10) as ETeil).AddArbeitsplatz(8);
            (instance.GetTeil(10) as ETeil).AddArbeitsplatz(12);
            (instance.GetTeil(10) as ETeil).AddArbeitsplatz(13);
            (instance.GetTeil(4) as ETeil).AddArbeitsplatz(10);
            (instance.GetTeil(4) as ETeil).AddArbeitsplatz(11);
            (instance.GetTeil(49) as ETeil).AddArbeitsplatz(1);
            (instance.GetTeil(7) as ETeil).AddArbeitsplatz(10);
            (instance.GetTeil(7) as ETeil).AddArbeitsplatz(11);
            (instance.GetTeil(18) as ETeil).AddArbeitsplatz(6);
            (instance.GetTeil(18) as ETeil).AddArbeitsplatz(8);
            (instance.GetTeil(18) as ETeil).AddArbeitsplatz(7);
            (instance.GetTeil(18) as ETeil).AddArbeitsplatz(9);
            (instance.GetTeil(13) as ETeil).AddArbeitsplatz(13);
            (instance.GetTeil(13) as ETeil).AddArbeitsplatz(12);
            (instance.GetTeil(13) as ETeil).AddArbeitsplatz(8);
            (instance.GetTeil(13) as ETeil).AddArbeitsplatz(7);
            (instance.GetTeil(13) as ETeil).AddArbeitsplatz(9);

            (instance.GetTeil(3) as ETeil).AddArbeitsplatz(4);
            (instance.GetTeil(31) as ETeil).AddArbeitsplatz(3);
            (instance.GetTeil(30) as ETeil).AddArbeitsplatz(2);
            (instance.GetTeil(12) as ETeil).AddArbeitsplatz(9);
            (instance.GetTeil(12) as ETeil).AddArbeitsplatz(7);
            (instance.GetTeil(12) as ETeil).AddArbeitsplatz(8);
            (instance.GetTeil(12) as ETeil).AddArbeitsplatz(12);
            (instance.GetTeil(12) as ETeil).AddArbeitsplatz(13);
            (instance.GetTeil(6) as ETeil).AddArbeitsplatz(10);
            (instance.GetTeil(6) as ETeil).AddArbeitsplatz(11);
            (instance.GetTeil(29) as ETeil).AddArbeitsplatz(1);
            (instance.GetTeil(9) as ETeil).AddArbeitsplatz(10);
            (instance.GetTeil(9) as ETeil).AddArbeitsplatz(11);
            (instance.GetTeil(20) as ETeil).AddArbeitsplatz(6);
            (instance.GetTeil(20) as ETeil).AddArbeitsplatz(8);
            (instance.GetTeil(20) as ETeil).AddArbeitsplatz(7);
            (instance.GetTeil(20) as ETeil).AddArbeitsplatz(9);
            (instance.GetTeil(15) as ETeil).AddArbeitsplatz(13);
            (instance.GetTeil(15) as ETeil).AddArbeitsplatz(12);
            (instance.GetTeil(15) as ETeil).AddArbeitsplatz(8);
            (instance.GetTeil(15) as ETeil).AddArbeitsplatz(7);
            (instance.GetTeil(15) as ETeil).AddArbeitsplatz(9);
        }

        public void InitializeArbPl()
        {
            DataContainer dc = DataContainer.Instance;
            dc.GetArbeitsplatz(1).AddWerkzeit(49, 6);
            dc.GetArbeitsplatz(1).AddWerkzeit(54, 6);
            dc.GetArbeitsplatz(1).AddWerkzeit(29, 6);
            dc.GetArbeitsplatz(1).AddRuestzeit(49, 20);
            dc.GetArbeitsplatz(1).AddRuestzeit(54, 20);
            dc.GetArbeitsplatz(1).AddRuestzeit(29, 20);
            dc.GetArbeitsplatz(1).AnzRuestung = 4;

            dc.GetArbeitsplatz(2).AddWerkzeit(50, 5);
            dc.GetArbeitsplatz(2).AddWerkzeit(55, 5);
            dc.GetArbeitsplatz(2).AddWerkzeit(30, 5);
            dc.GetArbeitsplatz(2).AddRuestzeit(50, 30);
            dc.GetArbeitsplatz(2).AddRuestzeit(55, 30);
            dc.GetArbeitsplatz(2).AddRuestzeit(30, 20);
            dc.GetArbeitsplatz(2).AnzRuestung = 5;

            dc.GetArbeitsplatz(3).AddWerkzeit(51, 5);
            dc.GetArbeitsplatz(3).AddWerkzeit(56, 6);
            dc.GetArbeitsplatz(3).AddWerkzeit(31, 6);
            dc.GetArbeitsplatz(3).AddRuestzeit(51, 20);
            dc.GetArbeitsplatz(3).AddRuestzeit(56, 20);
            dc.GetArbeitsplatz(3).AddRuestzeit(31, 20);
            dc.GetArbeitsplatz(3).AnzRuestung = 3;

            dc.GetArbeitsplatz(4).AddWerkzeit(1, 6);
            dc.GetArbeitsplatz(4).AddWerkzeit(2, 7);
            dc.GetArbeitsplatz(4).AddWerkzeit(3, 7);
            dc.GetArbeitsplatz(4).AddRuestzeit(1, 30);
            dc.GetArbeitsplatz(4).AddRuestzeit(2, 20);
            dc.GetArbeitsplatz(4).AddRuestzeit(3, 30);
            dc.GetArbeitsplatz(4).AnzRuestung = 4;

            dc.GetArbeitsplatz(6).AddWerkzeit(16, 2);
            dc.GetArbeitsplatz(6).AddWerkzeit(18, 3);
            dc.GetArbeitsplatz(6).AddWerkzeit(19, 3);
            dc.GetArbeitsplatz(6).AddWerkzeit(20, 3);
            dc.GetArbeitsplatz(6).AddRuestzeit(16, 15);
            dc.GetArbeitsplatz(6).AddRuestzeit(18, 15);
            dc.GetArbeitsplatz(6).AddRuestzeit(19, 15);
            dc.GetArbeitsplatz(6).AddRuestzeit(20, 15);
            dc.GetArbeitsplatz(6).NaechsterArbeitsplatz[16] = 14;
            dc.GetArbeitsplatz(6).NaechsterArbeitsplatz[18] = 8;
            dc.GetArbeitsplatz(6).NaechsterArbeitsplatz[19] = 8;
            dc.GetArbeitsplatz(6).NaechsterArbeitsplatz[20] = 8;
            dc.GetArbeitsplatz(6).AnzRuestung = 4;

            dc.GetArbeitsplatz(7).AddWerkzeit(13, 2);
            dc.GetArbeitsplatz(7).AddWerkzeit(18, 2);
            dc.GetArbeitsplatz(7).AddWerkzeit(26, 2);
            dc.GetArbeitsplatz(7).AddWerkzeit(10, 2);
            dc.GetArbeitsplatz(7).AddWerkzeit(14, 2);
            dc.GetArbeitsplatz(7).AddWerkzeit(19, 2);
            dc.GetArbeitsplatz(7).AddWerkzeit(11, 2);
            dc.GetArbeitsplatz(7).AddWerkzeit(15, 2);
            dc.GetArbeitsplatz(7).AddWerkzeit(20, 2);
            dc.GetArbeitsplatz(7).AddWerkzeit(12, 2);
            dc.GetArbeitsplatz(7).AddRuestzeit(13, 20);
            dc.GetArbeitsplatz(7).AddRuestzeit(18, 20);
            dc.GetArbeitsplatz(7).AddRuestzeit(26, 30);
            dc.GetArbeitsplatz(7).AddRuestzeit(10, 20);
            dc.GetArbeitsplatz(7).AddRuestzeit(14, 20);
            dc.GetArbeitsplatz(7).AddRuestzeit(19, 20);
            dc.GetArbeitsplatz(7).AddRuestzeit(11, 20);
            dc.GetArbeitsplatz(7).AddRuestzeit(15, 20);
            dc.GetArbeitsplatz(7).AddRuestzeit(20, 20);
            dc.GetArbeitsplatz(7).AddRuestzeit(12, 20);
            dc.GetArbeitsplatz(7).NaechsterArbeitsplatz[26] = 15;
            dc.GetArbeitsplatz(7).NaechsterArbeitsplatz[13] = 9;
            dc.GetArbeitsplatz(7).NaechsterArbeitsplatz[18] = 9;
            dc.GetArbeitsplatz(7).NaechsterArbeitsplatz[19] = 9;
            dc.GetArbeitsplatz(7).NaechsterArbeitsplatz[20] = 9;
            dc.GetArbeitsplatz(7).NaechsterArbeitsplatz[10] = 9;
            dc.GetArbeitsplatz(7).NaechsterArbeitsplatz[14] = 9;
            dc.GetArbeitsplatz(7).NaechsterArbeitsplatz[11] = 9;
            dc.GetArbeitsplatz(7).NaechsterArbeitsplatz[15] = 9;
            dc.GetArbeitsplatz(7).NaechsterArbeitsplatz[12] = 9;
            dc.GetArbeitsplatz(7).AnzRuestung = 20;


            dc.GetArbeitsplatz(8).AddWerkzeit(13, 1);
            dc.GetArbeitsplatz(8).AddWerkzeit(18, 3);
            dc.GetArbeitsplatz(8).AddWerkzeit(10, 1);
            dc.GetArbeitsplatz(8).AddWerkzeit(14, 2);
            dc.GetArbeitsplatz(8).AddWerkzeit(19, 3);
            dc.GetArbeitsplatz(8).AddWerkzeit(11, 2);
            dc.GetArbeitsplatz(8).AddWerkzeit(15, 2);
            dc.GetArbeitsplatz(8).AddWerkzeit(20, 3);
            dc.GetArbeitsplatz(8).AddWerkzeit(12, 2);
            dc.GetArbeitsplatz(8).AddRuestzeit(13, 15);
            dc.GetArbeitsplatz(8).AddRuestzeit(18, 20);
            dc.GetArbeitsplatz(8).AddRuestzeit(10, 15);
            dc.GetArbeitsplatz(8).AddRuestzeit(14, 15);
            dc.GetArbeitsplatz(8).AddRuestzeit(19, 25);
            dc.GetArbeitsplatz(8).AddRuestzeit(11, 15);
            dc.GetArbeitsplatz(8).AddRuestzeit(15, 15);
            dc.GetArbeitsplatz(8).AddRuestzeit(20, 20);
            dc.GetArbeitsplatz(8).AddRuestzeit(12, 15);
            dc.GetArbeitsplatz(8).NaechsterArbeitsplatz[13] = 7;
            dc.GetArbeitsplatz(8).NaechsterArbeitsplatz[18] = 7;
            dc.GetArbeitsplatz(8).NaechsterArbeitsplatz[19] = 7;
            dc.GetArbeitsplatz(8).NaechsterArbeitsplatz[20] = 7;
            dc.GetArbeitsplatz(8).NaechsterArbeitsplatz[10] = 7;
            dc.GetArbeitsplatz(8).NaechsterArbeitsplatz[14] = 7;
            dc.GetArbeitsplatz(8).NaechsterArbeitsplatz[11] = 7;
            dc.GetArbeitsplatz(8).NaechsterArbeitsplatz[15] = 7;
            dc.GetArbeitsplatz(8).NaechsterArbeitsplatz[12] = 7;
            dc.GetArbeitsplatz(8).AnzRuestung = 18;


            dc.GetArbeitsplatz(9).AddWerkzeit(13, 3);
            dc.GetArbeitsplatz(9).AddWerkzeit(18, 2);
            dc.GetArbeitsplatz(9).AddWerkzeit(10, 3);
            dc.GetArbeitsplatz(9).AddWerkzeit(14, 3);
            dc.GetArbeitsplatz(9).AddWerkzeit(19, 2);
            dc.GetArbeitsplatz(9).AddWerkzeit(11, 3);
            dc.GetArbeitsplatz(9).AddWerkzeit(15, 3);
            dc.GetArbeitsplatz(9).AddWerkzeit(20, 2);
            dc.GetArbeitsplatz(9).AddWerkzeit(12, 3);
            dc.GetArbeitsplatz(9).AddRuestzeit(13, 15);
            dc.GetArbeitsplatz(9).AddRuestzeit(18, 15);
            dc.GetArbeitsplatz(9).AddRuestzeit(10, 15);
            dc.GetArbeitsplatz(9).AddRuestzeit(14, 15);
            dc.GetArbeitsplatz(9).AddRuestzeit(19, 20);
            dc.GetArbeitsplatz(9).AddRuestzeit(11, 15);
            dc.GetArbeitsplatz(9).AddRuestzeit(15, 15);
            dc.GetArbeitsplatz(9).AddRuestzeit(20, 15);
            dc.GetArbeitsplatz(9).AddRuestzeit(12, 15);
            dc.GetArbeitsplatz(9).AnzRuestung = 11;


            dc.GetArbeitsplatz(10).AddWerkzeit(7, 4);
            dc.GetArbeitsplatz(10).AddWerkzeit(4, 4);
            dc.GetArbeitsplatz(10).AddWerkzeit(8, 4);
            dc.GetArbeitsplatz(10).AddWerkzeit(5, 4);
            dc.GetArbeitsplatz(10).AddWerkzeit(9, 4);
            dc.GetArbeitsplatz(10).AddWerkzeit(6, 4);
            dc.GetArbeitsplatz(10).AddRuestzeit(7, 20);
            dc.GetArbeitsplatz(10).AddRuestzeit(4, 20);
            dc.GetArbeitsplatz(10).AddRuestzeit(8, 20);
            dc.GetArbeitsplatz(10).AddRuestzeit(5, 20);
            dc.GetArbeitsplatz(10).AddRuestzeit(9, 20);
            dc.GetArbeitsplatz(10).AddRuestzeit(6, 20);
            dc.GetArbeitsplatz(10).NaechsterArbeitsplatz[7] = 11;
            dc.GetArbeitsplatz(10).NaechsterArbeitsplatz[4] = 11;
            dc.GetArbeitsplatz(10).NaechsterArbeitsplatz[8] = 11;
            dc.GetArbeitsplatz(10).NaechsterArbeitsplatz[5] = 11;
            dc.GetArbeitsplatz(10).NaechsterArbeitsplatz[9] = 11;
            dc.GetArbeitsplatz(10).NaechsterArbeitsplatz[6] = 11;
            dc.GetArbeitsplatz(10).AnzRuestung = 6;


            dc.GetArbeitsplatz(11).AddWerkzeit(7, 3);
            dc.GetArbeitsplatz(11).AddWerkzeit(4, 3);
            dc.GetArbeitsplatz(11).AddWerkzeit(8, 3);
            dc.GetArbeitsplatz(11).AddWerkzeit(5, 3);
            dc.GetArbeitsplatz(11).AddWerkzeit(9, 3);
            dc.GetArbeitsplatz(11).AddWerkzeit(6, 3);
            dc.GetArbeitsplatz(11).AddRuestzeit(7, 20);
            dc.GetArbeitsplatz(11).AddRuestzeit(4, 10);
            dc.GetArbeitsplatz(11).AddRuestzeit(8, 20);
            dc.GetArbeitsplatz(11).AddRuestzeit(5, 10);
            dc.GetArbeitsplatz(11).AddRuestzeit(9, 20);
            dc.GetArbeitsplatz(11).AddRuestzeit(6, 20);
            dc.GetArbeitsplatz(11).AnzRuestung = 6;

            dc.GetArbeitsplatz(12).AddWerkzeit(13, 3);
            dc.GetArbeitsplatz(12).AddWerkzeit(10, 3);
            dc.GetArbeitsplatz(12).AddWerkzeit(14, 3);
            dc.GetArbeitsplatz(12).AddWerkzeit(11, 3);
            dc.GetArbeitsplatz(12).AddWerkzeit(15, 3);
            dc.GetArbeitsplatz(12).AddWerkzeit(12, 3);
            dc.GetArbeitsplatz(12).AddRuestzeit(13, 0);
            dc.GetArbeitsplatz(12).AddRuestzeit(10, 0);
            dc.GetArbeitsplatz(12).AddRuestzeit(14, 0);
            dc.GetArbeitsplatz(12).AddRuestzeit(11, 0);
            dc.GetArbeitsplatz(12).AddRuestzeit(15, 0);
            dc.GetArbeitsplatz(12).AddRuestzeit(12, 0);
            dc.GetArbeitsplatz(12).NaechsterArbeitsplatz[13] = 8;
            dc.GetArbeitsplatz(12).NaechsterArbeitsplatz[10] = 8;
            dc.GetArbeitsplatz(12).NaechsterArbeitsplatz[14] = 8;
            dc.GetArbeitsplatz(12).NaechsterArbeitsplatz[11] = 8;
            dc.GetArbeitsplatz(12).NaechsterArbeitsplatz[15] = 8;
            dc.GetArbeitsplatz(12).NaechsterArbeitsplatz[12] = 8;
            dc.GetArbeitsplatz(12).AnzRuestung = 6;


            dc.GetArbeitsplatz(13).AddWerkzeit(13, 2);
            dc.GetArbeitsplatz(13).AddWerkzeit(10, 2);
            dc.GetArbeitsplatz(13).AddWerkzeit(14, 2);
            dc.GetArbeitsplatz(13).AddWerkzeit(11, 2);
            dc.GetArbeitsplatz(13).AddWerkzeit(15, 2);
            dc.GetArbeitsplatz(13).AddWerkzeit(12, 2);
            dc.GetArbeitsplatz(13).AddRuestzeit(13, 0);
            dc.GetArbeitsplatz(13).AddRuestzeit(10, 0);
            dc.GetArbeitsplatz(13).AddRuestzeit(14, 0);
            dc.GetArbeitsplatz(13).AddRuestzeit(11, 0);
            dc.GetArbeitsplatz(13).AddRuestzeit(15, 0);
            dc.GetArbeitsplatz(13).AddRuestzeit(12, 0);
            dc.GetArbeitsplatz(13).NaechsterArbeitsplatz[13] = 12;
            dc.GetArbeitsplatz(13).NaechsterArbeitsplatz[10] = 12;
            dc.GetArbeitsplatz(13).NaechsterArbeitsplatz[14] = 12;
            dc.GetArbeitsplatz(13).NaechsterArbeitsplatz[11] = 12;
            dc.GetArbeitsplatz(13).NaechsterArbeitsplatz[15] = 12;
            dc.GetArbeitsplatz(13).NaechsterArbeitsplatz[12] = 12;
            dc.GetArbeitsplatz(13).AnzRuestung = 6;

            dc.GetArbeitsplatz(14).AddWerkzeit(16, 3);
            dc.GetArbeitsplatz(14).AddRuestzeit(16, 0);
            dc.GetArbeitsplatz(14).AnzRuestung = 1;

            dc.GetArbeitsplatz(15).AddWerkzeit(17, 3);
            dc.GetArbeitsplatz(15).AddWerkzeit(26, 3);
            dc.GetArbeitsplatz(15).AddRuestzeit(17, 15);
            dc.GetArbeitsplatz(15).AddRuestzeit(26, 15);
            dc.GetArbeitsplatz(15).AnzRuestung = 2;
        }

    }
}
