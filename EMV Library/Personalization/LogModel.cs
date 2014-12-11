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
    }
}