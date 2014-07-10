using System.Runtime.Serialization;

namespace WSCT.EMV.Personalization
{
    [DataContract]
    public class TagLengthModel
    {
        [DataMember]
        public string Tag { get; set; }

        [DataMember]
        public byte Length { get; set; }
    }
}