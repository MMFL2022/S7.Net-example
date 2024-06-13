using S7.Net;
using S7NetWrapper;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace HmiExample.PlcConnectivity
{
    class Plc
    {
        #region Singleton

        // For implementation refer to: http://geekswithblogs.net/BlackRabbitCoder/archive/2010/05/19/c-system.lazylttgt-and-the-singleton-design-pattern.aspx        
        private static readonly Lazy<Plc> _instance = new Lazy<Plc>(() => new Plc());

        public static Plc Instance
        {
            get => _instance.Value;
        }

        #endregion

        #region Public properties

        public ConnectionStates ConnectionState
        {
            get => plcDriver != null ? plcDriver.ConnectionState : ConnectionStates.Offline;
        }

        public DB50 Db50 { get; set; }

        public TimeSpan CycleReadTime { get; private set; }

        #endregion

        #region Private fields

        IPlcSyncDriver plcDriver;

        System.Timers.Timer timer = new System.Timers.Timer();

        public DateTime lastReadTime;

        #endregion

        #region Constructor

        private Plc()
        {
            Db50 = new DB50();
            timer.Interval = 100; // ms
            timer.Elapsed += timer_Elapsed;
            timer.Enabled = true;
            lastReadTime = DateTime.Now;
        }

        #endregion

        #region Event handlers

        private async void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (plcDriver == null || plcDriver.ConnectionState != ConnectionStates.Online)
                return;

            timer.Enabled = false;
            CycleReadTime = DateTime.Now - lastReadTime;

            try
            {
                await RefreshTagsAsync();
            }
            finally
            {
                timer.Enabled = true;
                lastReadTime = DateTime.Now;
            }
        }

        #endregion

        #region Public methods

        public async Task ConnectAsync(string ipAddress)
        {
            if (!IsValidIp(ipAddress))
                throw new ArgumentException("Ip address is not valid");

            plcDriver = new S7NetPlcDriver(CpuType.S7300, ipAddress, 0, 2);

            await plcDriver.ConnectAsync();
        }

        public void Disconnect()
        {
            if (plcDriver == null || this.ConnectionState == ConnectionStates.Offline)
                return;

            plcDriver.Disconnect();
        }

        public async Task WriteAsync(string name, object value)
        {
            if (plcDriver == null || plcDriver.ConnectionState != ConnectionStates.Online)
                return;

            Tag tag = new Tag(name, value);
            List<Tag> tagList = new List<Tag>
            {
                tag
            };

            await plcDriver.WriteItemsAsync(tagList);
        }

        public async Task WriteAsync(List<Tag> tags)
        {
            if (plcDriver == null || plcDriver.ConnectionState != ConnectionStates.Online)
                return;

            await plcDriver.WriteItemsAsync(tags);
        }

        #endregion

        public async Task<DateTime> ReadDateTimeAsync()
        {
            return await plcDriver.ReadDateTimeAsync();
        }

        public async Task<byte> ReadStatusAsync()
        {
            return await plcDriver.ReadStatusAsync();
        }

        #region Private methods

        private bool IsValidIp(string addr)
        {
            IPAddress ip;
            bool valid = !string.IsNullOrEmpty(addr) && IPAddress.TryParse(addr, out ip);

            return valid;
        }

        private async Task RefreshTagsAsync()
        {
            await plcDriver.ReadClassAsync(Db50, 50);
        }

        #endregion
    }
}
