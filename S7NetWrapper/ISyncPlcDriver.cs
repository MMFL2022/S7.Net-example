#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace S7NetWrapper
{
    public interface IPlcSyncDriver
    {
        ConnectionStates ConnectionState { get; }

        Task ConnectAsync();

        void Disconnect();

        Task<List<Tag>> ReadItemsAsync(List<Tag> itemList);

        Task ReadClassAsync(object sourceClass, int db);

        Task WriteClassAsync(object sourceClass, int db);

        Task WriteItemsAsync(List<Tag> itemList);

        Task<DateTime> ReadDateTimeAsync();

        Task<byte> ReadStatusAsync();
    }
}
