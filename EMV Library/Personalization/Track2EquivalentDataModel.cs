using System;
using System.Runtime.Serialization;

namespace WSCT.EMV.Personalization
{
    [DataContract]
    public class Track2EquivalentDataModel
    {
        /// <summary>
        /// Primary Account Number (n, var up to 19)
        /// </summary>
        [DataMember]
        public string Pan { get; set; } = String.Empty;

        /// <summary>
        /// Expiration Date YYMM (n 4)
        /// </summary>
        [DataMember(Name = "expiration-date")]
        public string ExpirationDate { get; set; } = String.Empty;

        /// <summary>
        /// Service Code (n 3)
        /// </summary>
        [DataMember(Name = "service-code")]
        public string ServiceCode { get; set; } = String.Empty;

        /// <summary>
        /// Discretionary Data (n, var)
        /// </summary>
        [DataMember(Name = "discretionary-data")]
        public string DiscretionaryData { get; set; } = String.Empty;

        [IgnoreDataMember]
        public string Track2EqDataFormat
        {
            get
            {
                return String.Format("{0}D{1}{2}{3}{4}",
                    Pan,
                    ExpirationDate,
                    ServiceCode,
                    DiscretionaryData,
                    (Pan.Length + DiscretionaryData.Length) % 2 == 1 ? "F" : "");
            }
        }
    }
}