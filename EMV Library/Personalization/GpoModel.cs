using System.Runtime.Serialization;

namespace WSCT.EMV.Personalization
{
    [DataContract]
    public class GpoModel
    {
        [DataMember]
        public string Aip { get; set; }
    }
}