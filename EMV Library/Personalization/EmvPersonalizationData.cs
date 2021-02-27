using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WSCT.EMV.Personalization
{
    [DataContract]
    public class EmvPersonalizationData
    {
        [JsonExtensionData]
        public IDictionary<string, JToken> UnmanagedAttributes;
    }
}
