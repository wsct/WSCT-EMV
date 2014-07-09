using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WSCT.EMV.Personalization
{
    [DataContract]
    public class TagModel
    {
        [DataMember]
        public string Tag { get; set; }

        [DataMember]
        public IEnumerable<TagModel> Fields { get; set; }
    }
}