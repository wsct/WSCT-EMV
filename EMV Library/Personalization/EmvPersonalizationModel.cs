using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WSCT.EMV.Personalization
{
    [DataContract]
    public class EmvPersonalizationModel
    {
        [DataMember]
        public FciModel Fci { get; set; }

        [DataMember]
        public GpoModel Gpo { get; set; }

        [DataMember]
        public IEnumerable<RecordModel> Records { get; set; }
    }
}
