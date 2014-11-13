using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WSCT.EMV.CardPersonalisationConsole
{
    [DataContract]
    class DgiContainer
    {
        [DataMember]
        public string Fci { get; set; }

        [DataMember]
        public string Gpo { get; set; }

        [DataMember]
        public List<string> Records { get; set; }

        public DgiContainer()
        {
            Records = new List<string>();
        }
    }
}
