using System;

namespace HmiExample.PlcConnectivity
{
    // ORDER OF THE PROPERTIES NEEDS TO MATCH THE DB IN THE PLC!!!!!!
    public class DB50
    {
        /// <summary>
        /// DB50.DBX0.0
        /// </summary>
        public bool BitVariable0 { get; set; }
        public bool BitVariable1 { get; set; }
        public bool BitVariable2 { get; set; }
        public bool BitVariable3 { get; set; }
        public bool BitVariable4 { get; set; }
        public bool BitVariable5 { get; set; }
        public bool BitVariable6 { get; set; }
        public bool BitVariable7 { get; set; }

        /// <summary>
        /// DB50.DBX1.0
        /// </summary>
        public bool BitVariable10 { get; set; }
        public bool BitVariable11 { get; set; }
        public bool BitVariable12 { get; set; }
        public bool BitVariable13 { get; set; }
        public bool BitVariable14 { get; set; }
        public bool BitVariable15 { get; set; }
        public bool BitVariable16 { get; set; }
        public bool BitVariable17 { get; set; }

        /// <summary>
        /// DB50.DBW2
        /// </summary>
        public short IntVariable { get; set; }

        /// <summary>
        /// DB50.DBD4
        /// </summary>
        public float RealVariable { get; set; }

        /// <summary>
        /// DB50.DBD8
        /// </summary>
        public int DIntVariable { get; set; }

        /// <summary>
        /// DB50.DBD12
        /// </summary>
        //public ushort DWordVariable { get; set; }

        [S7.Net.Types.S7DateTime(S7.Net.Types.S7DateTimeType.DT)]
        public DateTime BootedDateTime { get; set; }
    }
}
