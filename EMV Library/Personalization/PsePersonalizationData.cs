using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WSCT.EMV.Personalization
{
    [DataContract]
    public class PsePersonalizationData
    {
        [DataMember]
        public PseRecord[] Records { get; set; }

        [JsonExtensionData]
        public IDictionary<string, JToken> UnmanagedAttributes;
    }
}
