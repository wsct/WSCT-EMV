using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WSCT.EMV.Personalization
{
    [DataContract]
    public class EmvPersonalizationModel
    {
        [DataMember]
        public string Aid { get; set; }

        [DataMember]
        public uint Atc { get; set; }

        [DataMember]
        public FciModel Fci { get; set; }

        [DataMember]
        public GpoModel Gpo { get; set; }

        [DataMember]
        public LogModel Log { get; set; }

        [DataMember]
        public IEnumerable<RecordModel> Records { get; set; }
    }
}
