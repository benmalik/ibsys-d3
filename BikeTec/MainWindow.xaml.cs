using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Globalization;
using System.ComponentModel;
using Tool;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

namespace BikeTec
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        DataContainer dc = DataContainer.Instance;
        string fileName = "";
        static int kaufteilNummer = 0;
        bool genutzt = false;
        static bool fertig = false;
        const String MSG_KEINE_NUM_WERTE_IN_PUFFERFELD = "Einige Pufferfelder enthalten keine numerischen Zeichen!";
        const String MSG_FILENAME_INPUT_XML = "Input_Period_";

        BrushConverter bc = new BrushConverter();
        
        public MainWindow()
        {
            InitializeComponent();

            Stammdaten init = new Stammdaten();
            init.Initialisieren();
            this.DataContext = this;
            labelWILLKOMMEN.Background = (Brush)bc.ConvertFrom("#FF7FD009");
            labelINPUT.Background = null;
            labelVERKAUFSPROGNOSE.Background = null;
            labelPUFFER.Background = null;
            labelNACHBESTELLUNG.Background = null;
            labelZUSAMMENFASSUNG.Background = null;
            ChangeLanguage("de-DE");

        }
        private int m_progress;
        public int Progress
        {
            get { return m_progress; }
            set
            {
                m_progress = value;
                OnPropertyChanged("Progress");
            }
        }

        public ObservableCollection<string> Steps
        {
            get;
            set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void IncreaseButton_Click(object sender, RoutedEventArgs e)
        {
            Progress += 15;
        }

        private void DecreaseButton_Click(object sender, RoutedEventArgs e)
        {
            Progress -= 30;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (!fertig)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Die Produktionsplanung wurde nicht gespeichert! Anwendung beenden?", "Anwendung beenden", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.No)
                    e.Cancel = true;
            }
        }

        private void beendeAnwendung(object sender, RoutedEventArgs e)
        {
            this.Close();
            //System.Windows.Application.Current. close Shutdown();
        }

        /* //////////////
         * SELECT FILE
         */
        /////////////

        /*
        void Internet_OnChecked(object sender, RoutedEventArgs e) {
            FileNameTextBox.Visibility = System.Windows.Visibility.Hidden;
            buttonSelectFile.Visibility = System.Windows.Visibility.Hidden;

            dc.Inet = true;
            dc.Xml = false;

            //buttonSelectURL.Visibility = System.Windows.Visibility.Visible;
            PeriodCombo.Visibility = System.Windows.Visibility.Visible;
        }
        */

        void XML_OnChecked(object sender, RoutedEventArgs e)
        {
            FileNameTextBox.Visibility = System.Windows.Visibility.Visible;
            buttonSelectFile.Visibility = System.Windows.Visibility.Visible;

            dc.Inet = false;
            dc.Xml = true;

            //buttonSelectURL.Visibility = System.Windows.Visibility.Hidden;
            PeriodCombo.Visibility = System.Windows.Visibility.Hidden;
        }

        void selectFile(object sender, RoutedEventArgs e)
        {

            this.IncreaseButton_Click(sender, e);

            outputGrid.Visibility = System.Windows.Visibility.Hidden;

            welcomeText.Visibility = System.Windows.Visibility.Hidden;
            ForecastGrid.Visibility = System.Windows.Visibility.Hidden;
            DragDropGrid.Visibility = System.Windows.Visibility.Visible;

            labelWILLKOMMEN.Background = null;
            labelINPUT.Background = (Brush)bc.ConvertFrom("#FF7FD009");
            labelVERKAUFSPROGNOSE.Background = null;
            labelPUFFER.Background = null;
            labelNACHBESTELLUNG.Background = null;
            labelZUSAMMENFASSUNG.Background = null;

        }

        private void btnExportFile(object sender, RoutedEventArgs args)
        {
            Microsoft.Win32.SaveFileDialog slg = new Microsoft.Win32.SaveFileDialog();

            // Set filter for file extension and default file extension
            slg.CheckPathExists = true;
            slg.DefaultExt = ".xml";
            slg.FileName = MSG_FILENAME_INPUT_XML + InputOutput.GetPeriod();
            slg.Filter = "Result file (.xml)|*.xml";

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = slg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                string filename = slg.FileName;
                FileNameTextBox_Export.Text = filename;

                dc.SaveInputXML = filename;

                //InputOutput.ReadFile();

                //buttonInputForecast.Visibility = System.Windows.Visibility.Visible;

                InputOutput.WriteInput();

                Process.Start("notepad.exe", filename); // System.Windows.Forms.Application.StartupPath + "//input.xml");

                //TODO: wenn etwas geändert wurde, dann fertig auf false setzen!
                fertig = true;
            }
        }

        /*
         * Anbindung nicht mehr möglich
        private void buttonBrowseURL(object sender, RoutedEventArgs args)
        {
            String per = PeriodCombo.Text;
            if (!genutzt)
            {
                dc.OpenFile = "http://www.iwi.hs-karlsruhe.de/scs/data/output/207_6_" + per + "result.xml";

                Internet.IsEnabled = false;
                XML.IsEnabled = false;
                PeriodCombo.IsEnabled = false;



                InputOutput.ReadFile();

                buttonInputForecast.Visibility = System.Windows.Visibility.Visible;

                System.Windows.MessageBox.Show("Datei von SCSIM-Plattform eingelesen!","Datei erfolgreich gelesen");
            }
            else { }
            genutzt = true;
            
        }
        */
        private void buttonBrowseFile(object sender, RoutedEventArgs args)
        {
            if (!genutzt)
            {
                // Create OpenFileDialog
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

                // Set filter for file extension and default file extension
                dlg.DefaultExt = ".xml";
                dlg.Filter = "Result file (.xml)|*.xml";

                // Display OpenFileDialog by calling ShowDialog method
                Nullable<bool> result = dlg.ShowDialog();

                // Get the selected file name and display in a TextBox
                if (result == true)
                {
                    // Open document
                    string filename = dlg.FileName;
                    FileNameTextBox.Text = filename;

                    dc.OpenFile = filename;
                    InputOutput.ReadFile();

                    buttonInputForecast.Visibility = System.Windows.Visibility.Visible;

                    System.Windows.MessageBox.Show("Die XML-Datei wurde erfolgreich eingelesen.", "Info");
                }
            }
            else { }
            genutzt = true;
        }

        private void ehDragOver(object sender, System.Windows.DragEventArgs args)
        {
            // As an arbitrary design decision, we only want to deal with a single file.
            if (IsSingleFile(args) != null) args.Effects = System.Windows.DragDropEffects.Copy;
            else args.Effects = System.Windows.DragDropEffects.None;

            // Mark the event as handled, so TextBox's native DragOver handler is not called.
            args.Handled = true;
        }

        private void ehDrop(object sender, System.Windows.DragEventArgs args)
        {
            // Mark the event as handled, so TextBox's native Drop handler is not called.
            args.Handled = true;

            fileName = IsSingleFile(args);
            if (fileName == null) return;

            // Set the window title to the loaded file.
            this.Title = "File Loaded: " + fileName;

            dc.OpenFile = fileName;

            InputOutput.ReadFile();

            buttonInputForecast.Visibility = System.Windows.Visibility.Visible;

        }

        // If the data object in args is a single file, this method will return the filename.
        // Otherwise, it returns null.
        private string IsSingleFile(System.Windows.DragEventArgs args)
        {
            // Check for files in the hovering data object.
            if (args.Data.GetDataPresent(System.Windows.DataFormats.FileDrop, true))
            {
                string[] fileNames = args.Data.GetData(System.Windows.DataFormats.FileDrop, true) as string[];
                // Check fo a single file or folder.
                if (fileNames.Length == 1)
                {
                    // Check for a file (a directory will return false).
                    if (File.Exists(fileNames[0]))
                    {
                        // At this point we know there is a single file.
                        return fileNames[0];
                    }
                }
            }
            return null;
        }


        /* //////////////
         * Back Buttons
         */
        /////////////
        void backSelectFile(object sender, RoutedEventArgs e)
        {
            Progress = 15;
            welcomeText.Visibility = System.Windows.Visibility.Visible;
            DragDropGrid.Visibility = System.Windows.Visibility.Hidden;
            labelWILLKOMMEN.Background = (Brush)bc.ConvertFrom("#FF7FD009");
            labelINPUT.Background = null;
            labelVERKAUFSPROGNOSE.Background = null;
            labelPUFFER.Background = null;
            labelNACHBESTELLUNG.Background = null;
            labelZUSAMMENFASSUNG.Background = null;
        }

        void backForecast(object sender, RoutedEventArgs e)
        {
            this.DecreaseButton_Click(sender, e);
            this.selectFile(sender, e);
        }

        void backBuffer(object sender, RoutedEventArgs e)
        {
            this.DecreaseButton_Click(sender, e);
            this.inputForecast(sender, e);
        }

        void backArticleOrder(object sender, RoutedEventArgs e)
        {
            this.DecreaseButton_Click(sender, e);
            this.inputButtonBuffer(sender, e);
        }

        void backOutput(object sender, RoutedEventArgs e)
        {
            this.DecreaseButton_Click(sender, e);
            this.inputOrder(sender, e);
        }

        void backErgebnis(object sender, RoutedEventArgs e)
        {
            ForecastGrid.Visibility = System.Windows.Visibility.Hidden;
            ArticleOrderGrid.Visibility = System.Windows.Visibility.Visible;
            bufferGrid.Visibility = System.Windows.Visibility.Hidden;
            ergebnisGrid.Visibility = System.Windows.Visibility.Hidden;

            outputGrid.Visibility = System.Windows.Visibility.Hidden;
            labelWILLKOMMEN.Background = null;
            labelINPUT.Background = null;
            labelVERKAUFSPROGNOSE.Background = null;
            labelPUFFER.Background = null;
            labelNACHBESTELLUNG.Background = (Brush)bc.ConvertFrom("#FF7FD009");
            labelZUSAMMENFASSUNG.Background = null;
        }

        /* //////////////
         * INPUT FORECAST
         */
        /////////////
        void inputForecast(object sender, RoutedEventArgs e)
        {
            this.IncreaseButton_Click(sender, e);

            DragDropGrid.Visibility = System.Windows.Visibility.Hidden;
            bufferGrid.Visibility = System.Windows.Visibility.Hidden;
            ForecastGrid.Visibility = System.Windows.Visibility.Visible;

            labelWILLKOMMEN.Background = null;
            labelINPUT.Background = null;
            labelVERKAUFSPROGNOSE.Background = (Brush)bc.ConvertFrom("#FF7FD009");
            labelPUFFER.Background = null;
            labelNACHBESTELLUNG.Background = null;
            labelZUSAMMENFASSUNG.Background = null;

            //TODO: new Prognoseeingabe().Show();
        }



        /* //////////////
         * INPUT BUFFER
         */
        /////////////
        void inputButtonBuffer(object sender, RoutedEventArgs e)
        {
            this.IncreaseButton_Click(sender, e);

            ForecastGrid.Visibility = System.Windows.Visibility.Hidden;
            ArticleOrderGrid.Visibility = System.Windows.Visibility.Hidden;
            bufferGrid.Visibility = System.Windows.Visibility.Visible;
            labelWILLKOMMEN.Background = null;
            labelINPUT.Background = null;
            labelVERKAUFSPROGNOSE.Background = null;
            labelPUFFER.Background = (Brush)bc.ConvertFrom("#FF7FD009");
            labelNACHBESTELLUNG.Background = null;
            labelZUSAMMENFASSUNG.Background = null;

            berechneVerbrauch();
        }

        void weiterButtonErgebnis(object sender, RoutedEventArgs e)
        {
            //this.IncreaseButton_Click(sender, e);

            ForecastGrid.Visibility = System.Windows.Visibility.Hidden;
            ArticleOrderGrid.Visibility = System.Windows.Visibility.Hidden;
            bufferGrid.Visibility = System.Windows.Visibility.Hidden;
            outputGrid.Visibility = System.Windows.Visibility.Hidden;

            ergebnisGrid.Visibility = System.Windows.Visibility.Visible;
            labelWILLKOMMEN.Background = null;
            labelINPUT.Background = null;
            labelVERKAUFSPROGNOSE.Background = null;
            labelPUFFER.Background = null;
            labelNACHBESTELLUNG.Background = null;
            labelZUSAMMENFASSUNG.Background = (Brush)bc.ConvertFrom("#FF7FD009");

            //fillChart();

            //berechneVerbrauch();
        }

        //private void fillChart()
        //{
        //    String periodstarttext = "Periode " + (dc.AktuellePeriode - 1);
        //    String periodendtext = "Periode " + dc.AktuellePeriode;

        //    ((System.Windows.Controls.DataVisualization.Charting.LineSeries)mcChart.Series[0]).ItemsSource =
        //new KeyValuePair<String, double>[]{
        //new KeyValuePair<String,double>(periodstarttext, dc.lagerwertStart),
        //new KeyValuePair<String,double>(periodendtext, dc.minLagerwertEnd) };

        //    ((System.Windows.Controls.DataVisualization.Charting.LineSeries)mcChart.Series[1]).ItemsSource =
        //new KeyValuePair<String, double>[]{
        //new KeyValuePair<String,double>(periodstarttext, dc.lagerwertStart),
        //new KeyValuePair<String,double>(periodendtext, dc.normalLagerwertEnd) };

        //    ((System.Windows.Controls.DataVisualization.Charting.LineSeries)mcChart.Series[2]).ItemsSource =
        //new KeyValuePair<String, double>[]{
        //new KeyValuePair<String,double>(periodstarttext, dc.lagerwertStart),
        //new KeyValuePair<String,double>(periodendtext, dc.maxLagerwertEnd) };

        //    double durchschnittEnd = (dc.minLagerwertEnd + dc.normalLagerwertEnd + dc.maxLagerwertEnd) / 3.0;
        //    /*
        //                ((System.Windows.Controls.DataVisualization.Charting.LineSeries)mcChart.Series[3]).ItemsSource =
        //            new KeyValuePair<String, double>[]{
        //            new KeyValuePair<String,double>(periodstarttext, dc.lagerwertStart),
        //            new KeyValuePair<String,double>(periodendtext, durchschnittEnd) };
        //                */
        //}

        private void berechneVerbrauch()
        {
            dc.GetTeil(1).VerbrauchAktuell = Convert.ToInt32(comboBox1.Text);
            dc.GetTeil(1).VerbrauchPrognose1 = Convert.ToInt32(comboBox4.Text);
            dc.GetTeil(1).VerbrauchPrognose2 = Convert.ToInt32(comboBox7.Text);
            dc.GetTeil(1).VerbrauchPrognose3 = Convert.ToInt32(comboBox10.Text);

            dc.GetTeil(2).VerbrauchAktuell = Convert.ToInt32(comboBox2.Text);
            dc.GetTeil(2).VerbrauchPrognose1 = Convert.ToInt32(comboBox5.Text);
            dc.GetTeil(2).VerbrauchPrognose2 = Convert.ToInt32(comboBox8.Text);
            dc.GetTeil(2).VerbrauchPrognose3 = Convert.ToInt32(comboBox11.Text);

            dc.GetTeil(3).VerbrauchAktuell = Convert.ToInt32(comboBox3.Text);
            dc.GetTeil(3).VerbrauchPrognose1 = Convert.ToInt32(comboBox6.Text);
            dc.GetTeil(3).VerbrauchPrognose2 = Convert.ToInt32(comboBox9.Text);
            dc.GetTeil(3).VerbrauchPrognose3 = Convert.ToInt32(comboBox12.Text);
        }

        /* //////////////
         * INPUT NACHBESTELLUNG
        */
        /////////////
        void inputOrder(object sender, RoutedEventArgs e)
        {
            if (!checkInput())
            {
                System.Windows.MessageBox.Show(MSG_KEINE_NUM_WERTE_IN_PUFFERFELD);
                return;
            }

            this.IncreaseButton_Click(sender, e);

            bufferGrid.Visibility = System.Windows.Visibility.Hidden;
            outputGrid.Visibility = System.Windows.Visibility.Hidden;
            ArticleOrderGrid.Visibility = System.Windows.Visibility.Visible;
            labelWILLKOMMEN.Background = null;
            labelINPUT.Background = null;
            labelVERKAUFSPROGNOSE.Background = null;
            labelPUFFER.Background = null;
            labelNACHBESTELLUNG.Background = (Brush)bc.ConvertFrom("#FF7FD009");
            labelZUSAMMENFASSUNG.Background = null;
        }

        bool checkInput()
        {
            try
            {
                dc.SetPuffer(1, Convert.ToInt32(comboBox100.Text));
                dc.SetPuffer(2, Convert.ToInt32(comboBox200.Text));
                dc.SetPuffer(3, Convert.ToInt32(comboBox300.Text));

                dc.SetPuffer(51, Convert.ToInt32(comboBox101.Text));
                dc.SetPuffer(50, Convert.ToInt32(comboBox102.Text));
                dc.SetPuffer(4, Convert.ToInt32(comboBox103.Text));
                dc.SetPuffer(10, Convert.ToInt32(comboBox104.Text));
                dc.SetPuffer(49, Convert.ToInt32(comboBox105.Text));
                dc.SetPuffer(7, Convert.ToInt32(comboBox106.Text));
                dc.SetPuffer(13, Convert.ToInt32(comboBox107.Text));
                dc.SetPuffer(18, Convert.ToInt32(comboBox108.Text));

                dc.SetPuffer(56, Convert.ToInt32(comboBox201.Text));
                dc.SetPuffer(55, Convert.ToInt32(comboBox202.Text));
                dc.SetPuffer(5, Convert.ToInt32(comboBox203.Text));
                dc.SetPuffer(11, Convert.ToInt32(comboBox204.Text));
                dc.SetPuffer(54, Convert.ToInt32(comboBox205.Text));
                dc.SetPuffer(8, Convert.ToInt32(comboBox206.Text));
                dc.SetPuffer(14, Convert.ToInt32(comboBox207.Text));
                dc.SetPuffer(19, Convert.ToInt32(comboBox208.Text));

                dc.SetPuffer(31, Convert.ToInt32(comboBox301.Text));
                dc.SetPuffer(30, Convert.ToInt32(comboBox302.Text));
                dc.SetPuffer(6, Convert.ToInt32(comboBox303.Text));
                dc.SetPuffer(12, Convert.ToInt32(comboBox304.Text));
                dc.SetPuffer(29, Convert.ToInt32(comboBox305.Text));
                dc.SetPuffer(9, Convert.ToInt32(comboBox306.Text));
                dc.SetPuffer(15, Convert.ToInt32(comboBox307.Text));
                dc.SetPuffer(20, Convert.ToInt32(comboBox308.Text));

                dc.SetPuffer(26, Convert.ToInt32(comboBox401.Text));
                dc.SetPuffer(16, Convert.ToInt32(comboBox402.Text));
                dc.SetPuffer(17, Convert.ToInt32(comboBox403.Text));
            }
            catch (FormatException fe)
            {
                return false;
            }
            return true;
        }

        void calculateOrder(object sender, RoutedEventArgs e)
        {
            dc.ClearData();
            clearTextBoxesOrderData();

            //nachträglich eingebaut (SS)
            dc.SetPuffer(1, Convert.ToInt32(comboBox100.Text));
            dc.SetPuffer(2, Convert.ToInt32(comboBox200.Text));
            dc.SetPuffer(3, Convert.ToInt32(comboBox300.Text));

            dc.SetPuffer(51, Convert.ToInt32(comboBox101.Text));
            dc.SetPuffer(50, Convert.ToInt32(comboBox102.Text));
            dc.SetPuffer(4, Convert.ToInt32(comboBox103.Text));
            dc.SetPuffer(10, Convert.ToInt32(comboBox104.Text));
            dc.SetPuffer(49, Convert.ToInt32(comboBox105.Text));
            dc.SetPuffer(7, Convert.ToInt32(comboBox106.Text));
            dc.SetPuffer(13, Convert.ToInt32(comboBox107.Text));
            dc.SetPuffer(18, Convert.ToInt32(comboBox108.Text));

            dc.SetPuffer(56, Convert.ToInt32(comboBox201.Text));
            dc.SetPuffer(55, Convert.ToInt32(comboBox202.Text));
            dc.SetPuffer(5, Convert.ToInt32(comboBox203.Text));
            dc.SetPuffer(11, Convert.ToInt32(comboBox204.Text));
            dc.SetPuffer(54, Convert.ToInt32(comboBox205.Text));
            dc.SetPuffer(8, Convert.ToInt32(comboBox206.Text));
            dc.SetPuffer(14, Convert.ToInt32(comboBox207.Text));
            dc.SetPuffer(19, Convert.ToInt32(comboBox208.Text));

            dc.SetPuffer(31, Convert.ToInt32(comboBox301.Text));
            dc.SetPuffer(30, Convert.ToInt32(comboBox302.Text));
            dc.SetPuffer(6, Convert.ToInt32(comboBox303.Text));
            dc.SetPuffer(12, Convert.ToInt32(comboBox304.Text));
            dc.SetPuffer(29, Convert.ToInt32(comboBox305.Text));
            dc.SetPuffer(9, Convert.ToInt32(comboBox306.Text));
            dc.SetPuffer(15, Convert.ToInt32(comboBox307.Text));
            dc.SetPuffer(20, Convert.ToInt32(comboBox308.Text));

            dc.SetPuffer(26, Convert.ToInt32(comboBox401.Text));
            dc.SetPuffer(16, Convert.ToInt32(comboBox402.Text));
            dc.SetPuffer(17, Convert.ToInt32(comboBox403.Text));

            berechneVerbrauch();

            dc.Optimieren();
            dc = DataContainer.Instance;

            foreach (Bestellposition bp in dc.Bestellung)
            {
                fillTextBoxesWithOrderData(bp);
            }
            buttonValidation.Visibility = System.Windows.Visibility.Visible;
        }


        void showButtonValidation(object sender, RoutedEventArgs e)
        {
            this.IncreaseButton_Click(sender, e);

            ArticleOrderGrid.Visibility = System.Windows.Visibility.Hidden;
            ergebnisGrid.Visibility = System.Windows.Visibility.Visible;

            labelWILLKOMMEN.Background = null;
            labelINPUT.Background = null;
            labelVERKAUFSPROGNOSE.Background = null;
            labelPUFFER.Background = null;
            labelNACHBESTELLUNG.Background = null;
            labelZUSAMMENFASSUNG.Background = (Brush)bc.ConvertFrom("#FF7FD009");

            writeUserOdersToDcOrderList();

            dc.SetPuffer(1, Convert.ToInt32(comboBox100.Text));
            dc.SetPuffer(2, Convert.ToInt32(comboBox200.Text));
            dc.SetPuffer(3, Convert.ToInt32(comboBox300.Text));

            dc.SetPuffer(51, Convert.ToInt32(comboBox101.Text));
            dc.SetPuffer(50, Convert.ToInt32(comboBox102.Text));
            dc.SetPuffer(4, Convert.ToInt32(comboBox103.Text));
            dc.SetPuffer(10, Convert.ToInt32(comboBox104.Text));
            dc.SetPuffer(49, Convert.ToInt32(comboBox105.Text));
            dc.SetPuffer(7, Convert.ToInt32(comboBox106.Text));
            dc.SetPuffer(13, Convert.ToInt32(comboBox107.Text));
            dc.SetPuffer(18, Convert.ToInt32(comboBox108.Text));

            dc.SetPuffer(56, Convert.ToInt32(comboBox201.Text));
            dc.SetPuffer(55, Convert.ToInt32(comboBox202.Text));
            dc.SetPuffer(5, Convert.ToInt32(comboBox203.Text));
            dc.SetPuffer(11, Convert.ToInt32(comboBox204.Text));
            dc.SetPuffer(54, Convert.ToInt32(comboBox205.Text));
            dc.SetPuffer(8, Convert.ToInt32(comboBox206.Text));
            dc.SetPuffer(14, Convert.ToInt32(comboBox207.Text));
            dc.SetPuffer(19, Convert.ToInt32(comboBox208.Text));

            dc.SetPuffer(31, Convert.ToInt32(comboBox301.Text));
            dc.SetPuffer(30, Convert.ToInt32(comboBox302.Text));
            dc.SetPuffer(6, Convert.ToInt32(comboBox303.Text));
            dc.SetPuffer(12, Convert.ToInt32(comboBox304.Text));
            dc.SetPuffer(29, Convert.ToInt32(comboBox305.Text));
            dc.SetPuffer(9, Convert.ToInt32(comboBox306.Text));
            dc.SetPuffer(15, Convert.ToInt32(comboBox307.Text));
            dc.SetPuffer(20, Convert.ToInt32(comboBox308.Text));

            dc.SetPuffer(26, Convert.ToInt32(comboBox401.Text));
            dc.SetPuffer(16, Convert.ToInt32(comboBox402.Text));
            dc.SetPuffer(17, Convert.ToInt32(comboBox403.Text));

            double bestellkosten = 0.0;
            double materialkosten = 0.0;
            double[] kteilliste = new double[60];

            foreach (Bestellposition bp in dc.Bestellung)
            {
                if (bp.Menge > 0)
                {
                    //Materialkosten
                    if (bp.Menge >= Stammdaten.DiskontmengeKTeil[bp.Kaufteil.Nummer - 1])
                    {
                        materialkosten += 0.9 * (Stammdaten.PreisKTeil[bp.Kaufteil.Nummer - 1]) * bp.Menge;
                    }
                    else
                    {
                        materialkosten += (Stammdaten.PreisKTeil[bp.Kaufteil.Nummer - 1]) * bp.Menge;
                    }

                    // Bestellkosten
                    if (bp.Eil == true)
                    {
                        kteilliste[bp.Kaufteil.Nummer - 1] = (Stammdaten.BestellkostenKTeil[bp.Kaufteil.Nummer - 1]) * 10;
                    }
                    else
                    {
                        kteilliste[bp.Kaufteil.Nummer - 1] = Stammdaten.BestellkostenKTeil[bp.Kaufteil.Nummer - 1];
                    }

                }
            }

            foreach (double ktl in kteilliste)
            {
                bestellkosten += ktl;

            }

            double benoetigteZeit = 0;
            double zurVerfuegungStehendeZeit = 0;

            for (int i = 1; i <= 15; i++)
            {
                if (i != 5)
                {
                    benoetigteZeit += dc.GetArbeitsplatz(i).BenoetigteZeit;
                    zurVerfuegungStehendeZeit += dc.GetArbeitsplatz(i).ZuVerfuegungStehendeZeit;
                }
            }

            double auslastung = Math.Round(benoetigteZeit * 100 / zurVerfuegungStehendeZeit, 2);


            textBlockBestellkostenWert.Text = Convert.ToString(bestellkosten);
            textBlockMaterialkostenWert.Text = Convert.ToString(materialkosten);
            textBlockAuslastungWert.Text = Convert.ToString(auslastung);


            //Textblöcke Grafik

            S1.Text = Convert.ToString(dc.GetArbeitsplatz(1).Schichten + " / " + dc.GetArbeitsplatz(1).UeberMin);
            S2.Text = Convert.ToString(dc.GetArbeitsplatz(2).Schichten + " / " + dc.GetArbeitsplatz(2).UeberMin);
            S3.Text = Convert.ToString(dc.GetArbeitsplatz(3).Schichten + " / " + dc.GetArbeitsplatz(3).UeberMin);
            S4.Text = Convert.ToString(dc.GetArbeitsplatz(4).Schichten + " / " + dc.GetArbeitsplatz(4).UeberMin);
            S6A.Text = Convert.ToString(dc.GetArbeitsplatz(6).Schichten + " / " + dc.GetArbeitsplatz(6).UeberMin);
            S6B.Text = Convert.ToString(dc.GetArbeitsplatz(6).Schichten + " / " + dc.GetArbeitsplatz(6).UeberMin);
            S7A.Text = Convert.ToString(dc.GetArbeitsplatz(7).Schichten + " / " + dc.GetArbeitsplatz(7).UeberMin);
            S7B.Text = Convert.ToString(dc.GetArbeitsplatz(7).Schichten + " / " + dc.GetArbeitsplatz(7).UeberMin);
            S7C.Text = Convert.ToString(dc.GetArbeitsplatz(7).Schichten + " / " + dc.GetArbeitsplatz(7).UeberMin);
            S7D.Text = Convert.ToString(dc.GetArbeitsplatz(7).Schichten + " / " + dc.GetArbeitsplatz(7).UeberMin);
            S8A.Text = Convert.ToString(dc.GetArbeitsplatz(8).Schichten + " / " + dc.GetArbeitsplatz(8).UeberMin);
            S8B.Text = Convert.ToString(dc.GetArbeitsplatz(8).Schichten + " / " + dc.GetArbeitsplatz(8).UeberMin);
            S8C.Text = Convert.ToString(dc.GetArbeitsplatz(8).Schichten + " / " + dc.GetArbeitsplatz(8).UeberMin);
            S9A.Text = Convert.ToString(dc.GetArbeitsplatz(9).Schichten + " / " + dc.GetArbeitsplatz(9).UeberMin);
            S9B.Text = Convert.ToString(dc.GetArbeitsplatz(9).Schichten + " / " + dc.GetArbeitsplatz(9).UeberMin);
            S9C.Text = Convert.ToString(dc.GetArbeitsplatz(9).Schichten + " / " + dc.GetArbeitsplatz(9).UeberMin);
            S10A.Text = Convert.ToString(dc.GetArbeitsplatz(10).Schichten + " / " + dc.GetArbeitsplatz(10).UeberMin);
            S10B.Text = Convert.ToString(dc.GetArbeitsplatz(10).Schichten + " / " + dc.GetArbeitsplatz(10).UeberMin);
            S11A.Text = Convert.ToString(dc.GetArbeitsplatz(11).Schichten + " / " + dc.GetArbeitsplatz(11).UeberMin);
            S11B.Text = Convert.ToString(dc.GetArbeitsplatz(11).Schichten + " / " + dc.GetArbeitsplatz(11).UeberMin);
            S12A.Text = Convert.ToString(dc.GetArbeitsplatz(12).Schichten + " / " + dc.GetArbeitsplatz(12).UeberMin);
            S12B.Text = Convert.ToString(dc.GetArbeitsplatz(12).Schichten + " / " + dc.GetArbeitsplatz(12).UeberMin);
            S13A.Text = Convert.ToString(dc.GetArbeitsplatz(13).Schichten + " / " + dc.GetArbeitsplatz(13).UeberMin);
            S13B.Text = Convert.ToString(dc.GetArbeitsplatz(13).Schichten + " / " + dc.GetArbeitsplatz(13).UeberMin);
            S14.Text = Convert.ToString(dc.GetArbeitsplatz(14).Schichten + " / " + dc.GetArbeitsplatz(14).UeberMin);
            S15A.Text = Convert.ToString(dc.GetArbeitsplatz(15).Schichten + " / " + dc.GetArbeitsplatz(15).UeberMin);
            S15B.Text = Convert.ToString(dc.GetArbeitsplatz(15).Schichten + " / " + dc.GetArbeitsplatz(15).UeberMin);


            ToolTipService.SetToolTip(S1, (dc.GetArbeitsplatz(1).BenoetigteZeit * 100) / dc.GetArbeitsplatz(1).ZuVerfuegungStehendeZeit + "% Auslastung (" + dc.GetArbeitsplatz(1).BenoetigteZeit + " Min. von " + dc.GetArbeitsplatz(1).ZuVerfuegungStehendeZeit + " Min.)");
            ToolTipService.SetToolTip(S2, (dc.GetArbeitsplatz(2).BenoetigteZeit * 100) / dc.GetArbeitsplatz(2).ZuVerfuegungStehendeZeit + "% Auslastung (" + dc.GetArbeitsplatz(2).BenoetigteZeit + " Min. von " + dc.GetArbeitsplatz(2).ZuVerfuegungStehendeZeit + " Min.)");
            ToolTipService.SetToolTip(S3, (dc.GetArbeitsplatz(3).BenoetigteZeit * 100) / dc.GetArbeitsplatz(3).ZuVerfuegungStehendeZeit + "% Auslastung (" + dc.GetArbeitsplatz(3).BenoetigteZeit + " Min. von " + dc.GetArbeitsplatz(3).ZuVerfuegungStehendeZeit + " Min.)");
            ToolTipService.SetToolTip(S4, (dc.GetArbeitsplatz(4).BenoetigteZeit * 100) / dc.GetArbeitsplatz(4).ZuVerfuegungStehendeZeit + "% Auslastung (" + dc.GetArbeitsplatz(4).BenoetigteZeit + " Min. von " + dc.GetArbeitsplatz(4).ZuVerfuegungStehendeZeit + " Min.)");
            ToolTipService.SetToolTip(S6A, (dc.GetArbeitsplatz(6).BenoetigteZeit * 100) / dc.GetArbeitsplatz(6).ZuVerfuegungStehendeZeit + "% Auslastung (" + dc.GetArbeitsplatz(6).BenoetigteZeit + " Min. von " + dc.GetArbeitsplatz(6).ZuVerfuegungStehendeZeit + " Min.)");
            ToolTipService.SetToolTip(S6B, (dc.GetArbeitsplatz(6).BenoetigteZeit * 100) / dc.GetArbeitsplatz(6).ZuVerfuegungStehendeZeit + "% Auslastung (" + dc.GetArbeitsplatz(6).BenoetigteZeit + " Min. von " + dc.GetArbeitsplatz(6).ZuVerfuegungStehendeZeit + " Min.)");
            ToolTipService.SetToolTip(S7A, (dc.GetArbeitsplatz(7).BenoetigteZeit * 100) / dc.GetArbeitsplatz(7).ZuVerfuegungStehendeZeit + "% Auslastung (" + dc.GetArbeitsplatz(7).BenoetigteZeit + " Min. von " + dc.GetArbeitsplatz(7).ZuVerfuegungStehendeZeit + " Min.)");
            ToolTipService.SetToolTip(S7B, (dc.GetArbeitsplatz(7).BenoetigteZeit * 100) / dc.GetArbeitsplatz(7).ZuVerfuegungStehendeZeit + "% Auslastung (" + dc.GetArbeitsplatz(7).BenoetigteZeit + " Min. von " + dc.GetArbeitsplatz(7).ZuVerfuegungStehendeZeit + " Min.)");
            ToolTipService.SetToolTip(S7C, (dc.GetArbeitsplatz(7).BenoetigteZeit * 100) / dc.GetArbeitsplatz(7).ZuVerfuegungStehendeZeit + "% Auslastung (" + dc.GetArbeitsplatz(7).BenoetigteZeit + " Min. von " + dc.GetArbeitsplatz(7).ZuVerfuegungStehendeZeit + " Min.)");
            ToolTipService.SetToolTip(S7D, (dc.GetArbeitsplatz(7).BenoetigteZeit * 100) / dc.GetArbeitsplatz(7).ZuVerfuegungStehendeZeit + "% Auslastung (" + dc.GetArbeitsplatz(7).BenoetigteZeit + " Min. von " + dc.GetArbeitsplatz(7).ZuVerfuegungStehendeZeit + " Min.)");
            ToolTipService.SetToolTip(S8A, (dc.GetArbeitsplatz(8).BenoetigteZeit * 100) / dc.GetArbeitsplatz(8).ZuVerfuegungStehendeZeit + "% Auslastung (" + dc.GetArbeitsplatz(8).BenoetigteZeit + " Min. von " + dc.GetArbeitsplatz(8).ZuVerfuegungStehendeZeit + " Min.)");
            ToolTipService.SetToolTip(S8B, (dc.GetArbeitsplatz(8).BenoetigteZeit * 100) / dc.GetArbeitsplatz(8).ZuVerfuegungStehendeZeit + "% Auslastung (" + dc.GetArbeitsplatz(8).BenoetigteZeit + " Min. von " + dc.GetArbeitsplatz(8).ZuVerfuegungStehendeZeit + " Min.)");
            ToolTipService.SetToolTip(S8C, (dc.GetArbeitsplatz(8).BenoetigteZeit * 100) / dc.GetArbeitsplatz(8).ZuVerfuegungStehendeZeit + "% Auslastung (" + dc.GetArbeitsplatz(8).BenoetigteZeit + " Min. von " + dc.GetArbeitsplatz(8).ZuVerfuegungStehendeZeit + " Min.)");
            ToolTipService.SetToolTip(S9A, (dc.GetArbeitsplatz(9).BenoetigteZeit * 100) / dc.GetArbeitsplatz(9).ZuVerfuegungStehendeZeit + "% Auslastung (" + dc.GetArbeitsplatz(9).BenoetigteZeit + " Min. von " + dc.GetArbeitsplatz(9).ZuVerfuegungStehendeZeit + " Min.)");
            ToolTipService.SetToolTip(S9B, (dc.GetArbeitsplatz(9).BenoetigteZeit * 100) / dc.GetArbeitsplatz(9).ZuVerfuegungStehendeZeit + "% Auslastung (" + dc.GetArbeitsplatz(9).BenoetigteZeit + " Min. von " + dc.GetArbeitsplatz(9).ZuVerfuegungStehendeZeit + " Min.)");
            ToolTipService.SetToolTip(S9C, (dc.GetArbeitsplatz(9).BenoetigteZeit * 100) / dc.GetArbeitsplatz(9).ZuVerfuegungStehendeZeit + "% Auslastung (" + dc.GetArbeitsplatz(9).BenoetigteZeit + " Min. von " + dc.GetArbeitsplatz(9).ZuVerfuegungStehendeZeit + " Min.)");
            ToolTipService.SetToolTip(S10A, (dc.GetArbeitsplatz(10).BenoetigteZeit * 100) / dc.GetArbeitsplatz(10).ZuVerfuegungStehendeZeit + "% Auslastung (" + dc.GetArbeitsplatz(10).BenoetigteZeit + " Min. von " + dc.GetArbeitsplatz(10).ZuVerfuegungStehendeZeit + " Min.)");
            ToolTipService.SetToolTip(S10B, (dc.GetArbeitsplatz(10).BenoetigteZeit * 100) / dc.GetArbeitsplatz(10).ZuVerfuegungStehendeZeit + "% Auslastung (" + dc.GetArbeitsplatz(10).BenoetigteZeit + " Min. von " + dc.GetArbeitsplatz(10).ZuVerfuegungStehendeZeit + " Min.)");
            ToolTipService.SetToolTip(S11A, (dc.GetArbeitsplatz(11).BenoetigteZeit * 100) / dc.GetArbeitsplatz(11).ZuVerfuegungStehendeZeit + "% Auslastung (" + dc.GetArbeitsplatz(11).BenoetigteZeit + " Min. von " + dc.GetArbeitsplatz(11).ZuVerfuegungStehendeZeit + " Min.)");
            ToolTipService.SetToolTip(S11B, (dc.GetArbeitsplatz(11).BenoetigteZeit * 100) / dc.GetArbeitsplatz(11).ZuVerfuegungStehendeZeit + "% Auslastung (" + dc.GetArbeitsplatz(11).BenoetigteZeit + " Min. von " + dc.GetArbeitsplatz(11).ZuVerfuegungStehendeZeit + " Min.)");
            ToolTipService.SetToolTip(S12A, (dc.GetArbeitsplatz(12).BenoetigteZeit * 100) / dc.GetArbeitsplatz(12).ZuVerfuegungStehendeZeit + "% Auslastung (" + dc.GetArbeitsplatz(12).BenoetigteZeit + " Min. von " + dc.GetArbeitsplatz(12).ZuVerfuegungStehendeZeit + " Min.)");
            ToolTipService.SetToolTip(S12B, (dc.GetArbeitsplatz(12).BenoetigteZeit * 100) / dc.GetArbeitsplatz(12).ZuVerfuegungStehendeZeit + "% Auslastung (" + dc.GetArbeitsplatz(12).BenoetigteZeit + " Min. von " + dc.GetArbeitsplatz(12).ZuVerfuegungStehendeZeit + " Min.)");
            ToolTipService.SetToolTip(S13A, (dc.GetArbeitsplatz(13).BenoetigteZeit * 100) / dc.GetArbeitsplatz(13).ZuVerfuegungStehendeZeit + "% Auslastung (" + dc.GetArbeitsplatz(13).BenoetigteZeit + " Min. von " + dc.GetArbeitsplatz(13).ZuVerfuegungStehendeZeit + " Min.)");
            ToolTipService.SetToolTip(S13B, (dc.GetArbeitsplatz(13).BenoetigteZeit * 100) / dc.GetArbeitsplatz(13).ZuVerfuegungStehendeZeit + "% Auslastung (" + dc.GetArbeitsplatz(13).BenoetigteZeit + " Min. von " + dc.GetArbeitsplatz(13).ZuVerfuegungStehendeZeit + " Min.)");
            ToolTipService.SetToolTip(S14, (dc.GetArbeitsplatz(14).BenoetigteZeit * 100) / dc.GetArbeitsplatz(14).ZuVerfuegungStehendeZeit + "% Auslastung (" + dc.GetArbeitsplatz(14).BenoetigteZeit + " Min. von " + dc.GetArbeitsplatz(14).ZuVerfuegungStehendeZeit + " Min.)");
            ToolTipService.SetToolTip(S15A, (dc.GetArbeitsplatz(15).BenoetigteZeit * 100) / dc.GetArbeitsplatz(15).ZuVerfuegungStehendeZeit + "% Auslastung (" + dc.GetArbeitsplatz(15).BenoetigteZeit + " Min. von " + dc.GetArbeitsplatz(15).ZuVerfuegungStehendeZeit + " Min.)");
            ToolTipService.SetToolTip(S15B, (dc.GetArbeitsplatz(15).BenoetigteZeit * 100) / dc.GetArbeitsplatz(15).ZuVerfuegungStehendeZeit + "% Auslastung (" + dc.GetArbeitsplatz(15).BenoetigteZeit + " Min. von " + dc.GetArbeitsplatz(15).ZuVerfuegungStehendeZeit + " Min.)");

            E13.Text = Convert.ToString("E13: " + (dc.GetTeil(13) as ETeil).Produktionsmenge);
            ToolTipService.SetToolTip(E13, Stammdaten.TBez.GetValue(13 - 1));
            E14.Text = Convert.ToString("E14: " + (dc.GetTeil(14) as ETeil).Produktionsmenge);
            ToolTipService.SetToolTip(E14, Stammdaten.TBez.GetValue(14 - 1));
            E15.Text = Convert.ToString("E15: " + (dc.GetTeil(15) as ETeil).Produktionsmenge);
            ToolTipService.SetToolTip(E15, Stammdaten.TBez.GetValue(15 - 1));

            E18.Text = Convert.ToString("E18: " + (dc.GetTeil(18) as ETeil).Produktionsmenge);
            ToolTipService.SetToolTip(E18, Stammdaten.TBez.GetValue(18 - 1));
            E19.Text = Convert.ToString("E19: " + (dc.GetTeil(19) as ETeil).Produktionsmenge);
            ToolTipService.SetToolTip(E19, Stammdaten.TBez.GetValue(19 - 1));
            E20.Text = Convert.ToString("E20: " + (dc.GetTeil(20) as ETeil).Produktionsmenge);
            ToolTipService.SetToolTip(E20, Stammdaten.TBez.GetValue(20 - 1));

            E7.Text = Convert.ToString("E7: " + (dc.GetTeil(7) as ETeil).Produktionsmenge);
            ToolTipService.SetToolTip(E7, Stammdaten.TBez.GetValue(7 - 1));
            E8.Text = Convert.ToString("E8: " + (dc.GetTeil(8) as ETeil).Produktionsmenge);
            ToolTipService.SetToolTip(E8, Stammdaten.TBez.GetValue(8 - 1));
            E9.Text = Convert.ToString("E9: " + (dc.GetTeil(9) as ETeil).Produktionsmenge);
            ToolTipService.SetToolTip(E9, Stammdaten.TBez.GetValue(9 - 1));

            E29.Text = Convert.ToString("E29: " + (dc.GetTeil(29) as ETeil).Produktionsmenge);
            ToolTipService.SetToolTip(E29, Stammdaten.TBez.GetValue(29 - 1));
            E49.Text = Convert.ToString("E49: " + (dc.GetTeil(49) as ETeil).Produktionsmenge);
            ToolTipService.SetToolTip(E49, Stammdaten.TBez.GetValue(49 - 1));
            E54.Text = Convert.ToString("E54: " + (dc.GetTeil(54) as ETeil).Produktionsmenge);
            ToolTipService.SetToolTip(E54, Stammdaten.TBez.GetValue(54 - 1));

            E4.Text = Convert.ToString("E4: " + (dc.GetTeil(4) as ETeil).Produktionsmenge);
            ToolTipService.SetToolTip(E4, Stammdaten.TBez.GetValue(4 - 1));
            E5.Text = Convert.ToString("E5: " + (dc.GetTeil(5) as ETeil).Produktionsmenge);
            ToolTipService.SetToolTip(E5, Stammdaten.TBez.GetValue(5 - 1));
            E6.Text = Convert.ToString("E6: " + (dc.GetTeil(6) as ETeil).Produktionsmenge);
            ToolTipService.SetToolTip(E6, Stammdaten.TBez.GetValue(6 - 1));

            E10.Text = Convert.ToString("E10: " + (dc.GetTeil(10) as ETeil).Produktionsmenge);
            ToolTipService.SetToolTip(E10, Stammdaten.TBez.GetValue(10 - 1));
            E11.Text = Convert.ToString("E11: " + (dc.GetTeil(11) as ETeil).Produktionsmenge);
            ToolTipService.SetToolTip(E11, Stammdaten.TBez.GetValue(11 - 1));
            E12.Text = Convert.ToString("E12: " + (dc.GetTeil(12) as ETeil).Produktionsmenge);
            ToolTipService.SetToolTip(E12, Stammdaten.TBez.GetValue(12 - 1));

            E17.Text = Convert.ToString("E17: " + (dc.GetTeil(17) as ETeil).Produktionsmenge);
            ToolTipService.SetToolTip(E17, Stammdaten.TBez.GetValue(17 - 1));

            E16.Text = Convert.ToString("E16: " + (dc.GetTeil(16) as ETeil).Produktionsmenge);
            ToolTipService.SetToolTip(E16, Stammdaten.TBez.GetValue(16 - 1));

            E30.Text = Convert.ToString("E30: " + (dc.GetTeil(30) as ETeil).Produktionsmenge);
            ToolTipService.SetToolTip(E30, Stammdaten.TBez.GetValue(30 - 1));
            E50.Text = Convert.ToString("E50: " + (dc.GetTeil(50) as ETeil).Produktionsmenge);
            ToolTipService.SetToolTip(E50, Stammdaten.TBez.GetValue(50 - 1));
            E55.Text = Convert.ToString("E55: " + (dc.GetTeil(55) as ETeil).Produktionsmenge);
            ToolTipService.SetToolTip(E55, Stammdaten.TBez.GetValue(55 - 1));

            E51.Text = Convert.ToString("E51: " + (dc.GetTeil(51) as ETeil).Produktionsmenge);
            ToolTipService.SetToolTip(E51, Stammdaten.TBez.GetValue(51 - 1));
            E56.Text = Convert.ToString("E56: " + (dc.GetTeil(56) as ETeil).Produktionsmenge);
            ToolTipService.SetToolTip(E56, Stammdaten.TBez.GetValue(56 - 1));
            E31.Text = Convert.ToString("E31: " + (dc.GetTeil(31) as ETeil).Produktionsmenge);
            ToolTipService.SetToolTip(E31, Stammdaten.TBez.GetValue(31 - 1));

            E26.Text = Convert.ToString("E26: " + (dc.GetTeil(26) as ETeil).Produktionsmenge);
            ToolTipService.SetToolTip(E26, Stammdaten.TBez.GetValue(26 - 1));

            P1.Text = Convert.ToString("P1: " + (dc.GetTeil(1) as ETeil).Produktionsmenge);
            ToolTipService.SetToolTip(P1, Stammdaten.TBez.GetValue(1 - 1));
            P2.Text = Convert.ToString("P2: " + (dc.GetTeil(2) as ETeil).Produktionsmenge);
            ToolTipService.SetToolTip(P2, Stammdaten.TBez.GetValue(2 - 1));
            P3.Text = Convert.ToString("P3: " + (dc.GetTeil(3) as ETeil).Produktionsmenge);
            ToolTipService.SetToolTip(P3, Stammdaten.TBez.GetValue(3 - 1));

        }



        /*//////////////
        * OUTPUT
        */
        /////////////
        /*void createOutputFile(object sender, RoutedEventArgs e)
        {
            InputOutput.WriteInput();

            Process.Start("notepad.exe", System.Windows.Forms.Application.StartupPath + "//input.xml");
        }*/

        private void clearTextBoxesOrderData()
        {
            textBoxK21.Text = null;
            comboBoxK21.Text = "Normal";
            textBoxK22.Text = null;
            comboBoxK22.Text = "Normal";
            textBoxK23.Text = null;
            comboBoxK23.Text = "Normal";
            textBoxK24.Text = null;
            comboBoxK24.Text = "Normal";
            textBoxK25.Text = null;
            comboBoxK25.Text = "Normal";
            textBoxK27.Text = null;
            comboBoxK27.Text = "Normal";
            textBoxK28.Text = null;
            comboBoxK28.Text = "Normal";
            textBoxK32.Text = null;
            comboBoxK32.Text = "Normal";
            textBoxK33.Text = null;
            comboBoxK33.Text = "Normal";
            textBoxK34.Text = null;
            comboBoxK34.Text = "Normal";
            textBoxK35.Text = null;
            comboBoxK35.Text = "Normal";
            textBoxK36.Text = null;
            comboBoxK36.Text = "Normal";
            textBoxK37.Text = null;
            comboBoxK37.Text = "Normal";
            textBoxK38.Text = null;
            comboBoxK38.Text = "Normal";
            textBoxK39.Text = null;
            comboBoxK39.Text = "Normal";
            textBoxK40.Text = null;
            comboBoxK40.Text = "Normal";
            textBoxK41.Text = null;
            comboBoxK41.Text = "Normal";
            textBoxK42.Text = null;
            comboBoxK42.Text = "Normal";
            textBoxK43.Text = null;
            comboBoxK43.Text = "Normal";
            textBoxK44.Text = null;
            comboBoxK44.Text = "Normal";
            textBoxK45.Text = null;
            comboBoxK45.Text = "Normal";
            textBoxK46.Text = null;
            comboBoxK46.Text = "Normal";
            textBoxK47.Text = null;
            comboBoxK47.Text = "Normal";
            textBoxK48.Text = null;
            comboBoxK48.Text = "Normal";
            textBoxK52.Text = null;
            comboBoxK52.Text = "Normal";
            textBoxK53.Text = null;
            comboBoxK53.Text = "Normal";
            textBoxK57.Text = null;
            comboBoxK57.Text = "Normal";
            textBoxK58.Text = null;
            comboBoxK58.Text = "Normal";
            textBoxK59.Text = null;
            comboBoxK59.Text = "Normal";
        }

        /// <summary>
        /// setzt die erechneten Bestellmengen in die Textfelder auf der GUI (Aufruf kommt von calculateOrder())
        /// </summary>
        /// <param name="bp"></param>
        private void fillTextBoxesWithOrderData(Bestellposition bp)
        {
            switch (bp.Kaufteil.Nummer)
            {
                case 21:
                    textBoxK21.Text = bp.Menge.ToString();
                    comboBoxK21.Text = resolveOrderTypeForComboBox(bp.Eil);
                    break;
                case 22:
                    textBoxK22.Text = bp.Menge.ToString();
                    comboBoxK22.Text = resolveOrderTypeForComboBox(bp.Eil);
                    break;
                case 23:
                    textBoxK23.Text = bp.Menge.ToString();
                    comboBoxK23.Text = resolveOrderTypeForComboBox(bp.Eil);
                    break;
                case 24:
                    textBoxK24.Text = bp.Menge.ToString();
                    comboBoxK24.Text = resolveOrderTypeForComboBox(bp.Eil);
                    break;
                case 25:
                    textBoxK25.Text = bp.Menge.ToString();
                    comboBoxK25.Text = resolveOrderTypeForComboBox(bp.Eil);
                    break;
                case 27:
                    textBoxK27.Text = bp.Menge.ToString();
                    comboBoxK27.Text = resolveOrderTypeForComboBox(bp.Eil);
                    break;
                case 28:
                    textBoxK28.Text = bp.Menge.ToString();
                    comboBoxK28.Text = resolveOrderTypeForComboBox(bp.Eil);
                    break;
                case 32:
                    textBoxK32.Text = bp.Menge.ToString();
                    comboBoxK32.Text = resolveOrderTypeForComboBox(bp.Eil);
                    break;
                case 33:
                    textBoxK33.Text = bp.Menge.ToString();
                    comboBoxK33.Text = resolveOrderTypeForComboBox(bp.Eil);
                    break;
                case 34:
                    textBoxK34.Text = bp.Menge.ToString();
                    comboBoxK34.Text = resolveOrderTypeForComboBox(bp.Eil);
                    break;
                case 35:
                    textBoxK35.Text = bp.Menge.ToString();
                    comboBoxK35.Text = resolveOrderTypeForComboBox(bp.Eil);
                    break;
                case 36:
                    textBoxK36.Text = bp.Menge.ToString();
                    comboBoxK36.Text = resolveOrderTypeForComboBox(bp.Eil);
                    break;
                case 37:
                    textBoxK37.Text = bp.Menge.ToString();
                    comboBoxK37.Text = resolveOrderTypeForComboBox(bp.Eil);
                    break;
                case 38:
                    textBoxK38.Text = bp.Menge.ToString();
                    comboBoxK38.Text = resolveOrderTypeForComboBox(bp.Eil);
                    break;
                case 39:
                    textBoxK39.Text = bp.Menge.ToString();
                    comboBoxK39.Text = resolveOrderTypeForComboBox(bp.Eil);
                    break;
                case 40:
                    textBoxK40.Text = bp.Menge.ToString();
                    comboBoxK40.Text = resolveOrderTypeForComboBox(bp.Eil);
                    break;
                case 41:
                    textBoxK41.Text = bp.Menge.ToString();
                    comboBoxK41.Text = resolveOrderTypeForComboBox(bp.Eil);
                    break;
                case 42:
                    textBoxK42.Text = bp.Menge.ToString();
                    comboBoxK42.Text = resolveOrderTypeForComboBox(bp.Eil);
                    break;
                case 43:
                    textBoxK43.Text = bp.Menge.ToString();
                    comboBoxK43.Text = resolveOrderTypeForComboBox(bp.Eil);
                    break;
                case 44:
                    textBoxK44.Text = bp.Menge.ToString();
                    comboBoxK44.Text = resolveOrderTypeForComboBox(bp.Eil);
                    break;
                case 45:
                    textBoxK45.Text = bp.Menge.ToString();
                    comboBoxK45.Text = resolveOrderTypeForComboBox(bp.Eil);
                    break;
                case 46:
                    textBoxK46.Text = bp.Menge.ToString();
                    comboBoxK46.Text = resolveOrderTypeForComboBox(bp.Eil);
                    break;
                case 47:
                    textBoxK47.Text = bp.Menge.ToString();
                    comboBoxK47.Text = resolveOrderTypeForComboBox(bp.Eil);
                    break;
                case 48:
                    textBoxK48.Text = bp.Menge.ToString();
                    comboBoxK48.Text = resolveOrderTypeForComboBox(bp.Eil);
                    break;
                case 52:
                    textBoxK52.Text = bp.Menge.ToString();
                    comboBoxK52.Text = resolveOrderTypeForComboBox(bp.Eil);
                    break;
                case 53:
                    textBoxK53.Text = bp.Menge.ToString();
                    comboBoxK53.Text = resolveOrderTypeForComboBox(bp.Eil);
                    break;
                case 57:
                    textBoxK57.Text = bp.Menge.ToString();
                    comboBoxK57.Text = resolveOrderTypeForComboBox(bp.Eil);
                    break;
                case 58:
                    textBoxK58.Text = bp.Menge.ToString();
                    comboBoxK58.Text = resolveOrderTypeForComboBox(bp.Eil);
                    break;
                case 59:
                    textBoxK59.Text = bp.Menge.ToString();
                    comboBoxK59.Text = resolveOrderTypeForComboBox(bp.Eil);
                    break;
            }
        }

        private string resolveOrderTypeForComboBox(Boolean eil)
        {
            return eil ? "Schnell" : "Normal";
        }

        private void writeUserOdersToDcOrderList()
        {
            if (textBoxK21.Text != null && textBoxK21.Text != "")
            {
                writeUserDataToDc(21, textBoxK21.Text, comboBoxK21.Text);
            }
            if (textBoxK22.Text != null && textBoxK22.Text != "")
            {
                writeUserDataToDc(22, textBoxK22.Text, comboBoxK22.Text);
            }
            if (textBoxK23.Text != null && textBoxK23.Text != "")
            {
                writeUserDataToDc(23, textBoxK23.Text, comboBoxK23.Text);
            }
            if (textBoxK24.Text != null && textBoxK24.Text != "")
            {
                writeUserDataToDc(24, textBoxK24.Text, comboBoxK24.Text);
            }
            if (textBoxK25.Text != null && textBoxK25.Text != "")
            {
                writeUserDataToDc(25, textBoxK25.Text, comboBoxK25.Text);
            }
            if (textBoxK27.Text != null && textBoxK27.Text != "")
            {
                writeUserDataToDc(27, textBoxK27.Text, comboBoxK27.Text);
            }
            if (textBoxK28.Text != null && textBoxK28.Text != "")
            {
                writeUserDataToDc(28, textBoxK28.Text, comboBoxK28.Text);
            }
            if (textBoxK32.Text != null && textBoxK32.Text != "")
            {
                writeUserDataToDc(32, textBoxK32.Text, comboBoxK32.Text);
            }
            if (textBoxK33.Text != null && textBoxK33.Text != "")
            {
                writeUserDataToDc(33, textBoxK33.Text, comboBoxK33.Text);
            } if (textBoxK34.Text != null && textBoxK34.Text != "")
            {
                writeUserDataToDc(34, textBoxK34.Text, comboBoxK34.Text);
            } if (textBoxK35.Text != null && textBoxK35.Text != "")
            {
                writeUserDataToDc(35, textBoxK35.Text, comboBoxK35.Text);
            }
            if (textBoxK36.Text != null && textBoxK36.Text != "")
            {
                writeUserDataToDc(36, textBoxK36.Text, comboBoxK36.Text);
            }
            if (textBoxK37.Text != null && textBoxK37.Text != "")
            {
                writeUserDataToDc(37, textBoxK37.Text, comboBoxK37.Text);
            }
            if (textBoxK38.Text != null && textBoxK38.Text != "")
            {
                writeUserDataToDc(38, textBoxK38.Text, comboBoxK38.Text);
            }
            if (textBoxK39.Text != null && textBoxK39.Text != "")
            {
                writeUserDataToDc(39, textBoxK39.Text, comboBoxK39.Text);
            }
            if (textBoxK40.Text != null && textBoxK40.Text != "")
            {
                writeUserDataToDc(40, textBoxK40.Text, comboBoxK40.Text);
            }
            if (textBoxK41.Text != null && textBoxK41.Text != "")
            {
                writeUserDataToDc(41, textBoxK41.Text, comboBoxK41.Text);
            }
            if (textBoxK42.Text != null && textBoxK42.Text != "")
            {
                writeUserDataToDc(42, textBoxK42.Text, comboBoxK42.Text);
            }
            if (textBoxK43.Text != null && textBoxK43.Text != "")
            {
                writeUserDataToDc(43, textBoxK43.Text, comboBoxK43.Text);
            }
            if (textBoxK44.Text != null && textBoxK44.Text != "")
            {
                writeUserDataToDc(44, textBoxK44.Text, comboBoxK44.Text);
            } if (textBoxK45.Text != null && textBoxK45.Text != "")
            {
                writeUserDataToDc(45, textBoxK45.Text, comboBoxK45.Text);
            } if (textBoxK46.Text != null && textBoxK46.Text != "")
            {
                writeUserDataToDc(46, textBoxK46.Text, comboBoxK46.Text);
            } if (textBoxK47.Text != null && textBoxK47.Text != "")
            {
                writeUserDataToDc(47, textBoxK47.Text, comboBoxK47.Text);
            } if (textBoxK48.Text != null && textBoxK48.Text != "")
            {
                writeUserDataToDc(48, textBoxK48.Text, comboBoxK48.Text);
            } if (textBoxK52.Text != null && textBoxK52.Text != "")
            {
                writeUserDataToDc(52, textBoxK52.Text, comboBoxK52.Text);
            } if (textBoxK53.Text != null && textBoxK53.Text != "")
            {
                writeUserDataToDc(53, textBoxK53.Text, comboBoxK53.Text);
            } if (textBoxK57.Text != null && textBoxK57.Text != "")
            {
                writeUserDataToDc(57, textBoxK57.Text, comboBoxK57.Text);
            } if (textBoxK58.Text != null && textBoxK58.Text != "")
            {
                writeUserDataToDc(58, textBoxK58.Text, comboBoxK58.Text);
            } if (textBoxK59.Text != null && textBoxK59.Text != "")
            {
                writeUserDataToDc(59, textBoxK59.Text, comboBoxK59.Text);
            }
        }

        private void writeUserDataToDc(int KaufteilNumber, string value, String orderType)
        {
            try
            {
                int menge = Convert.ToInt32(value);
                Boolean typeOfOrder = (orderType.Equals("Schnell")) ? true : false;
                kaufteilNummer = KaufteilNumber;
                Bestellposition bp = dc.Bestellung.Find(BestellungExists);
                if (null == bp)
                {
                    Kaufteil k = dc.KaufteilList.Find(getKaufteil);
                    bp = new Bestellposition(k, 0, typeOfOrder);
                    dc.AddBestellposition(bp);
                }
                bp.Menge = menge;
                bp.Eil = typeOfOrder;
            }
            catch (Exception e)
            {
            }



        }

        private static bool BestellungExists(Bestellposition bp)
        {
            if (bp.Kaufteil.Nummer == kaufteilNummer)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool getKaufteil(Kaufteil k)
        {
            if (k.Nummer == kaufteilNummer)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void button_DE_click(object sender, MouseButtonEventArgs e)
        {
            ChangeLanguage("de-DE");
        }

        private void button_EN_click(object sender, MouseButtonEventArgs e)
        {
            ChangeLanguage("en-US");
        }
        /// <summary>
        /// Sprache wechseln
        /// </summary>
        /// <param name="culture">specific culture</param>
        public void ChangeLanguage(string culture)
        {
            // Alle Woerterbuecher finden   
            List<ResourceDictionary> dictionaryList = new List<ResourceDictionary>();
            foreach (ResourceDictionary dictionary in System.Windows.Application.Current.Resources.MergedDictionaries)
            {
                dictionaryList.Add(dictionary);
            }

            // Woerterbuch waehlen
            string requestedCulture = string.Format("Datenpräsentation/cultures/CultResource.{0}.xaml", culture);
            ResourceDictionary resourceDictionary = dictionaryList.FirstOrDefault(d => d.Source.OriginalString == requestedCulture);
            if (resourceDictionary == null)
            {
                // Wenn das gewuenschte Woerterbuch nicht gefunden wird,
                // lade Standard-Woerterbuch
                requestedCulture = "Datenpräsentation/cultures/CultResource.xaml";
                resourceDictionary = dictionaryList.FirstOrDefault(d => d.Source.OriginalString == requestedCulture);
            }

            // Altes Woerterbuch loeschen und Neues hinzufuegen       
            if (resourceDictionary != null)
            {
                System.Windows.Application.Current.Resources.MergedDictionaries.Remove(resourceDictionary);
                System.Windows.Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
            }

            // Hauptthread ueber neues Culture informieren
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(culture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        }



        private void de_button(object sender, RoutedEventArgs e)
        {
            ChangeLanguage("de-DE");
        }

        private void en_button(object sender, RoutedEventArgs e)
        {
            ChangeLanguage("en-US");
        }
    }
}
