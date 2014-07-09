using System.Runtime.Serialization;

namespace WSCT.EMV.Personalization
{
    [DataContract]
    public class LogColumnModel
    {
        [DataMember]
        public string Tag { get; set; }

        [DataMember]
        public byte Size { get; set; }
    }
}