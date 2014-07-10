using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WSCT.EMV.Personalization
{
    [DataContract]
    public class RecordModel
    {
        [DataMember]
        public byte Sfi { get; set; }

        [DataMember]
        public byte Index { get; set; }

        [DataMember]
        public bool Signed { get; set; }

        [DataMember]
        public IEnumerable<string> Fields { get; set; }
    }
}