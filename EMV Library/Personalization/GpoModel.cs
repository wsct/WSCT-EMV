using System.Runtime.Serialization;

namespace WSCT.EMV.Personalization
{
    [DataContract]
    public class GpoModel
    {
        [DataMember]
        public string Dgi { get; set; }

        [DataMember]
        public string[] Fields { get; set; }
    }
}