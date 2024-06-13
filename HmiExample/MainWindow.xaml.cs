#region Using
using HmiExample.PlcConnectivity;
using S7NetWrapper;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using HmiExample.Properties;
#endregion

namespace HmiExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromMilliseconds(100);            
            timer.Tick += timer_Tick;
            timer.IsEnabled = true;
            txtIpAddress.Text = Settings.Default.IpAddress;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            btnConnect.IsEnabled = Plc.Instance.ConnectionState == ConnectionStates.Offline;
            btnDisconnect.IsEnabled = Plc.Instance.ConnectionState != ConnectionStates.Offline;
            lblConnectionState.Text = Plc.Instance.ConnectionState.ToString();
            ledMachineInRun.Fill = Plc.Instance.Db50.BitVariable0 ? Brushes.Green : Brushes.Gray;
            lblSpeed.Content = Plc.Instance.Db50.IntVariable;
            lblTemperature.Content = Plc.Instance.Db50.RealVariable;
            lblAutomaticSpeed.Content = Plc.Instance.Db50.DIntVariable;
            //lblSetDwordVariable.Content = Plc.Instance.Db50.DWordVariable;
            lblBootedDateTime.Content = Plc.Instance.Db50.BootedDateTime.ToString();
            // statusbar
            lblReadTime.Text = Plc.Instance.CycleReadTime.TotalMilliseconds.ToString(CultureInfo.InvariantCulture);
        }

        private async void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {                
                await Plc.Instance.ConnectAsync(txtIpAddress.Text);
                Settings.Default.IpAddress = txtIpAddress.Text;
                Settings.Default.Save();

                var plcDateTime = await Plc.Instance.ReadDateTimeAsync();
                lblDateTime.Text = plcDateTime.ToString();

                var plcStatus = await Plc.Instance.ReadStatusAsync();
                Console.WriteLine(plcStatus.ToString());
            }
            catch(Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void btnDisconnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Plc.Instance.Disconnect();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        /// <summary>
        /// Writes a bit to 1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Plc.Instance.WriteAsync(PlcTags.BitVariable, 1);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        /// <summary>
        /// Writes a bit to 1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private bool bit2 = false;
        private void btnStart2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bit2 = !bit2;
                Plc.Instance.WriteAsync(PlcTags.BitVariable1, bit2);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        /// <summary>
        /// Writes a bit to 1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private bool bit3 = false;
        private void btnStart3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bit3 = !bit3;
                Plc.Instance.WriteAsync(PlcTags.BitVariable2, bit3);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        /// <summary>
        /// Writes a bit to 0
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Plc.Instance.WriteAsync(PlcTags.BitVariable, 0);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void txtSetRealVariable_TextChanged(object sender, TextChangedEventArgs e)
        {
            double realVar;
            bool canConvert = Double.TryParse(txtSetTemperature.Text, out realVar);
            if (canConvert)
            {
                Plc.Instance.WriteAsync(PlcTags.DoubleVariable, realVar);
            }
        }

        private void txtSetWordVariable_TextChanged(object sender, TextChangedEventArgs e)
        {
            short wordVar;
            bool canConvert = short.TryParse(txtSetSpeed.Text, out wordVar);
            if (canConvert)
            {
                Plc.Instance.WriteAsync(PlcTags.IntVariable, wordVar);
            }
        }

        private void txtSetDIntVariable_TextChanged(object sender, TextChangedEventArgs e)
        {
            int dintVar;
            bool canConvert = int.TryParse(txtSetAutomaticSpeed.Text, out dintVar);
            if (canConvert)
            {
                Plc.Instance.WriteAsync(PlcTags.DIntVariable, dintVar);
            }
        }

        private void txtSetSetDwordVariable_TextChanged(object sender, TextChangedEventArgs e)
        {
            ushort dwordVar;
            bool canConvert = ushort.TryParse(txtSetDwordVariable.Text, out dwordVar);
            if (canConvert)
            {
                Plc.Instance.WriteAsync(PlcTags.DwordVariable, dwordVar);
            }
        }
    }
}
