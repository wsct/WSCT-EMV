using System.Runtime.Serialization;

namespace WSCT.EMV.Personalization
{
    [DataContract]
    public class PsePersonalizationModel
    {
        [DataMember]
        public FciModel Fci { get; set; }
    }
}
