using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace WSCT.EMV.Personalization
{
    [DataContract]
    public class LogModel
    {
        [DataMember]
        public byte Sfi { get; set; }

        [DataMember]
        public byte Size { get; set; }

        [DataMember]
        public IEnumerable<LogColumnModel> Columns { get; set; }

        [IgnoreDataMember]
        public string LogFormat
        {
            get
            {
                return Columns.Aggregate(String.Empty, (current, column) => current + String.Format("{0}{1:X2}", column.Tag, column.Size));
            }
        }
    }
}