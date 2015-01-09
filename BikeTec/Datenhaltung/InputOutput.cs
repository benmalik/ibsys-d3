using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.IO;
using System.Diagnostics;

namespace Tool
{
    public class InputOutput
    {
        public static int GetPeriod()
        {
            return instance.AktuellePeriode;
        }

        /// Instanzieren des Datencontainers
        private static DataContainer instance = DataContainer.Instance;

        ///ReadFile Methode
        ///ließt Output Datei und füllt Datenkontainer
        public static void ReadFile()
        {
            XmlReader reader = null;
            if (instance.Xml)
            {
                reader = XmlReader.Create(instance.OpenFile);

                if (!File.Exists(instance.OpenFile))
                {
                    throw new UnknownFileException(instance.OpenFile + " existiert nicht, wählen Sie eine gültige Datei!");
                }
            }
            else if (instance.Inet)
            {
                reader = XmlReader.Create(instance.OpenFile);

            }

            bool switchWarehouseStock = false;              //Schalter für Lagereinlesen
            bool switchFutureInwardStockMovement = false;   //Schalter für zukünftigen Wareneingang
            bool switchWaitingListWPL = false;              //Schalter für Waitinglist Workstations
            bool switchWaitingListStock = false;            //Schalter für Waitinglist Stock
            bool switchOrdersInWork = false;                //Schalter für OrdersinWork
            bool switchWorkplace = false;                   //Schalter für Arbeitsplätze
            int intLastSpacePos;                            //Speichert das letzte Vorkommen eines Leerzeichens von Entry    
            int intOrderMode;                               //Bestellmodus
            Teil t;

            Arbeitsplatz ap = instance.GetArbeitsplatz(1);
            int arbeitsplatzID = 0;

            String currentXMLNode = "";

            while (reader.Read())
            {

                switch (reader.Name)
                {
                    case "results":
                        currentXMLNode = "results";
                        if (instance.AktuellePeriode == -1)
                            instance.AktuellePeriode = Convert.ToInt32(reader.GetAttribute("period")) + 1;
                        break;

                    case "warehousestock":
                        currentXMLNode = "warehousestock";
                        break;

                    case "futureinwardstockmovement":
                        currentXMLNode = "futureinwardstockmovement";
                        break;

                    case "idletimecosts":
                        currentXMLNode = "idletimecosts";
                        break;

                    case "waitinglistworkstations":
                        currentXMLNode = "waitinglistworkstations";
                        break;

                    case "waitingliststock":
                        currentXMLNode = "waitingliststock";
                        switchWaitingListStock = !switchWaitingListStock;
                        break;

                    case "ordersinwork":
                        currentXMLNode = "ordersinwork";
                        //switchWaitinglist = !switchWaitinglist;
                        switchOrdersInWork = !switchOrdersInWork;
                        break;

                    case "completedorders":
                        currentXMLNode = "completedorders";
                        //switchOrdersInWork = !switchOrdersInWork;
                        break;

                    default:
                    switch (currentXMLNode)
                    {
                        case "results":

                            Console.WriteLine("wir sind in results: "+ reader.Name);
                            break;
                        case "warehousestock":
                            if (reader.Name == "article") {
                                Console.WriteLine("wir sind in warehousestock " + reader.Name);
                                t = instance.GetTeil(Convert.ToInt32(reader.GetAttribute(0)));
                                t.Lagerstand = Convert.ToInt32(reader.GetAttribute(2));
                                t.Lagerpreis = Convert.ToDouble(reader.GetAttribute(4));
                            }
                            break;
                        case "futureinwardstockmovement":
                            if (reader.Name == "order")
                            {
                                Console.WriteLine("wir sind in futureinwardstockmovement: " + reader.Name);
                                int teilnr = Convert.ToInt32(reader.GetAttribute(3));

                                (instance.GetTeil(teilnr) as Kaufteil).ErwarteteBestellung = Convert.ToInt32(reader.GetAttribute(4)) + (instance.GetTeil(teilnr) as Kaufteil).ErwarteteBestellung;

                                Kaufteil kaufds = instance.GetTeil(teilnr) as Kaufteil;

                                if (reader.GetAttribute(1) == "Normal")
                                {
                                    intOrderMode = 5;
                                }
                                else
                                {
                                    intOrderMode = 4; //Fast
                                }

                                kaufds.addBestellung(instance.AktuellePeriode, Convert.ToInt32(reader.GetAttribute(0).Substring(0, 1)), intOrderMode, Convert.ToInt32(reader.GetAttribute(4)));
                            }
                            break;
                        case "idletimecosts":
                            if (reader.Name == "workplace")
                            {
                                Console.WriteLine("wir sind in idletimecosts: " + reader.Name);
                            }
                            break;
                        case "waitinglistworkstations":
                            if (reader.Name == "workplace")
                            {
                                try
                                {
                                    arbeitsplatzID = Convert.ToInt32(reader.GetAttribute(0));
                                }
                                catch (Exception)
                                {
                                    //nö jetzt nicht
                                }

                            }
                            if (reader.Name == "waitinglist") {
                                Console.WriteLine("wir sind in waitinglistworkstations: " + reader.Name);
                                ap = instance.GetArbeitsplatz(arbeitsplatzID);
                            //Todo:
                               // intLastSpacePos = reader.GetAttribute(4).LastIndexOf(" ") + 1;
                                //Debug.WriteLine(reader.GetAttribute(2));
                                //Debug.WriteLine(Convert.ToInt32(reader.GetAttribute(4).Substring(intLastSpacePos)));
                                //Debug.WriteLine(Convert.ToInt32(Convert.ToDouble(reader.GetAttribute(5))));
                                ap.AddWarteschlange(Convert.ToInt32(reader.GetAttribute(4)), Convert.ToInt32(reader.GetAttribute(5)));
                                //ap.AddWarteschlange(Convert.ToInt32(reader.GetAttribute(4).Substring(intLastSpacePos)), Convert.ToInt32(Convert.ToDouble(reader.GetAttribute(5))));
                            }
                            break;
                        case "waitingliststock":
                            if (reader.Name == "waitinglist")
                            {
                                Console.WriteLine("wir sind in waitingliststock: " + reader.Name);
                                intLastSpacePos = reader.GetAttribute(0).LastIndexOf(" ") + 1;
                                //Debug.WriteLine(reader.GetAttribute(0).Substring(intLastSpacePos));
                                t = instance.GetTeil(Convert.ToInt32(reader.GetAttribute(0).Substring(intLastSpacePos)));
                                //t.Lagerstand -= Convert.ToInt32(Convert.ToDouble(reader.GetAttribute(5)));
                                t.Warteschlange += Convert.ToInt32(Convert.ToDouble(reader.GetAttribute(5)));
                            }
                            break;


                         case "ordersinwork":
                            if (reader.Name == "workplace")
                            {
                                ap = instance.GetArbeitsplatz(Convert.ToInt32(reader.GetAttribute(0)));
                                intLastSpacePos = reader.GetAttribute(4).LastIndexOf(" ") + 1;
                                ap.AddAuftraegeInBearbeitung(Convert.ToInt32(reader.GetAttribute(4).Substring(intLastSpacePos)), Convert.ToInt32(reader.GetAttribute(5)), Convert.ToInt32(reader.GetAttribute(6)));

                            }
                            break;
                            
                        default:
                            Console.WriteLine("default case: " + reader.Name);
                            break;

                    }
                    break;
                }

            }

            reader.Close();

        }



        /// WriteInput Methode
        /// schreibt die ScsimInput.xml zum Einlesen in SCSIM
        public static void WriteInput()
        {
            CreateOrResetFile();

            //Dateianfang
            WriteFile("<input>");

            WriteVerkaufswuensche();
            WriteBestellungen();
            WriteProduktionsauftraege();
            WriteArbeitsplaetze();

            //Rest
            //WriteFile("<MarketplaceTransactions />");
            //WriteFile("<IsQualityControlEnabled>false</IsQualityControlEnabled>");

            //Dateiende
            WriteFile("</input>");
        }

        //Datei erstellen. Wenn bereits vorhanden, Inhalt loeschen
        private static void CreateOrResetFile()
        {
            StreamWriter datei;
            //        	datei = File.CreateText(DataContainer.Instance.SaveFile);
            datei = File.CreateText(DataContainer.Instance.SaveInputXML);
            datei.Close();
        }

        private static void WriteVerkaufswuensche()
        {
            WriteFile("<sellwish>");

            for (int i = 1; i < 4; ++i)
            {
                ETeil et = instance.GetTeil(i) as ETeil;

                //WriteFile("<salesWish>");
                WriteFile("<item article =\"" + i + "\" quantity =\"" + et.VerbrauchAktuell + "\" />");
                //WriteFile("<SaleQuantity>" + et.VerbrauchAktuell + "</SaleQuantity>");
                // WriteFile("<DirectSaleQuantity>" + "0" + "</DirectSaleQuantity>");
                //WriteFile("<DirectSalePrice>" + "0.0" + "</DirectSalePrice>");
                //WriteFile("<DirectSalePenalty>" + "0.0" + "</DirectSalePenalty>");
                //WriteFile("</salesWish>");
            }

            WriteFile("</sellwish>");

        }

        private static void WriteBestellungen()
        {
            //Bestellungen
            WriteFile("<orderlist>");

            foreach (Bestellposition bp in instance.Bestellung)
            {
                //WriteFile("<ItemOrder>");
                //String bestellart = bp.OutputEil ==5 ? "normal" : "fast";
                WriteFile("<order article =\"" + bp.Kaufteil.Nummer
                    + "\" quantity =\"" + bp.Menge
                    + "\" modus =\"" + bp.OutputEil
                    + "\" />");
                //WriteFile("<Quantity>" + bp.Menge + "</Quantity>");

                /*if (bp.OutputEil == 5)
                {
                    WriteFile("<Supplier>" + "Normal" + "</Supplier>");
                }
                else
                {
                    WriteFile("<Supplier>" + "Fast" + "</Supplier>");
                }*/
                //WriteFile("</ItemOrder>");
            }

            WriteFile("</orderlist>");
        }

        private static void WriteProduktionsauftraege()
        {
            //Produktionsaufträge
            WriteFile("<productionlist>");

            foreach (int z in instance.Reihenfolge)
            {
                if (z > 0)
                {
                    ETeil et = instance.GetTeil(z) as ETeil;

                    if (et.Produktionsmenge > 0)
                    {
                        //WriteFile("<ProductionOrder>");
                        WriteFile("<production article =\"" + et.Nummer
                            + "\" quantity =\"" + Convert.ToInt32(et.Produktionsmenge / 2)
                            + "\" />");
                        //WriteFile("<Quantity>" + et.Produktionsmenge + "</Quantity>");
                        //WriteFile("<Quantity>" + Convert.ToInt32(et.Produktionsmenge / 2) + "</Quantity>");
                    }
                }
            }
            WriteFile("</productionlist>");
        }

        //Arbeitsplatz Ueberstunden und Schichten
        private static void WriteArbeitsplaetze()
        {
            WriteFile("<workingtimelist>");

            for (int i = 1; i <= 15; i++)
            {
                if (i == 5)
                {
                    //Ueberspringe des AP 5, da dieser nicht existiert
                    i++;
                }

                //WriteFile("<WorkplaceShift>");
                WriteFile("<workingtime station =\"" + i
                    + "\" shift = \"" + instance.GetArbeitsplatz(i).Schichten
                    + "\" overtime =\"" + instance.GetArbeitsplatz(i).UeberMin
                    + "\"/>");
                //WriteFile("<Shifts>" + instance.GetArbeitsplatz(i).Schichten + "</Shifts>");
                //WriteFile("<OvertimeInMinutes>" + instance.GetArbeitsplatz(i).UeberMin + "</OvertimeInMinutes>");
                //WriteFile("</WorkplaceShift>");
            }
            WriteFile("</workingtimelist>");
        }

        /// <summary>
        /// WriteFile Methode
        /// Hilfsmethode zum schreiben von Zeilen in eine Datei
        /// </summary>
        /// <param name="Inhalt">Inhalt der Datei</param>
        /// <param name="Name">Dateiname</param>
        private static void WriteFile(string Inhalt)
        {
            StreamWriter datei;
            //        	datei = File.AppendText(DataContainer.Instance.SaveFile);
            datei = File.AppendText(DataContainer.Instance.SaveInputXML);
            datei.WriteLine(Inhalt);
            datei.Close();
        }
    }
}
