using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WSCT.EMV.Personalization
{
    [DataContract]
    public class FciModel
    {
        [DataMember]
        public string Dgi { get; set; }

        [DataMember]
        public IEnumerable<TagModel> Tags { get; set; }
    }
}