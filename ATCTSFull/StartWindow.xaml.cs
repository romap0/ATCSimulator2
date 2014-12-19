using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace ATCTSFull
{
    /// <summary>
    /// Логика взаимодействия для StartGameWindow.xaml
    /// </summary>
    public partial class StartWindow : Elysium.Controls.Window
    {
        public StartWindow()
        {
            InitializeComponent();
        }

        WebClient client;
        public int MaximumTraffic = 0;
        bool IsDownloaded = false;
        string FilePath = "";

        private void sldTrafficSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MaximumTraffic = (int)sldTrafficSlider.Value;
            lblMaximumTraffic.Content = (int)sldTrafficSlider.Value;
        }

        private void ckbStableTrafficFlowChecked(object sender, RoutedEventArgs e)
        {
            sldTrafficSlider.Visibility = System.Windows.Visibility.Visible;
            lblMaximumTraffic.Visibility = System.Windows.Visibility.Visible;
        }

        private void btnBackWindowClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnStartClick(object sender, RoutedEventArgs e)
        {
            btnStart.IsEnabled = false;
            cmbSectors.IsEnabled = false; 
            FilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\NWD-Group\\ATC Training Simulator Full\\Sectors\\" + cmbSectors.SelectedItem.ToString() + ".sector";
            if (!File.Exists(FilePath))
            {
                client = new WebClient();
                if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\NWD-Group\\ATC Training Simulator Full\\Sectors"))
                    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\NWD-Group\\ATC Training Simulator Full\\Sectors");
                client.DownloadFileCompleted += client_DownloadFileCompleted;
                client.DownloadFileAsync(new Uri("http://nwd-group.com/" + cmbSectors.SelectedItem.ToString() + ".sector"), FilePath);
                pgDownloadSectorFile.Visibility = Visibility.Visible;
            }
            else
            {
                RadarWindow.SectorFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\NWD-Group\\ATC Training Simulator Full\\Sectors\\" + cmbSectors.SelectedItem.ToString() + ".sector";
                this.DialogResult = true;
                this.Close();
            }
        }

        void client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            RadarWindow.SectorFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\NWD-Group\\ATC Training Simulator Full\\Sectors\\" + cmbSectors.SelectedItem.ToString() + ".sector";
            IsDownloaded = true;
            this.DialogResult = true;
            this.Close();
        }

        private void ckbStableTrafficFlow_Unchecked(object sender, RoutedEventArgs e)
        {
            sldTrafficSlider.Visibility = System.Windows.Visibility.Hidden;
            lblMaximumTraffic.Visibility = System.Windows.Visibility.Hidden;
            MaximumTraffic = 0;
        }

        private void btnBuyMore_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.google.com");
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            foreach (SectorInfo CurrentSectorInfo in UserInfo.Sectors)
                if (UserInfo.ConnectionType == UserInfo.ConnectionTypes.Online || File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\NWD-Group\\ATC Training Simulator Full\\Sectors\\" + CurrentSectorInfo.ICAO + ".sector"))
                    cmbSectors.Items.Add(CurrentSectorInfo.ICAO);
        }

        private void cmbSectors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnStart.IsEnabled = true;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (client != null && client.IsBusy && !IsDownloaded)
                File.Delete(FilePath);
               
        }

    }
}
