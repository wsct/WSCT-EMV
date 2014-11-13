using System.Runtime.Serialization;

namespace WSCT.EMV.Personalization
{
    [DataContract]
    public class AcidModel
    {
        [DataMember]
        public string Dgi { get; set; }

        [DataMember]
        public string[] Fields { get; set; }
    }
}