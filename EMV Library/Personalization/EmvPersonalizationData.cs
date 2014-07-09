using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WSCT.EMV.Personalization
{
    [DataContract]
    public class EmvPersonalizationData
    {
        [DataMember]
        public string Aid { get; set; }

        [DataMember]
        public string Label { get; set; }

        [DataMember]
        public string PriorityIndicator { get; set; }

        [DataMember]
        public IEnumerable<string> LanguagePreference { get; set; }

        [JsonExtensionData]
        public IDictionary<string, JToken> UnmanagedAttributes;
    }
}
