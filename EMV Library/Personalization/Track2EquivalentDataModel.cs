using System;
using System.Runtime.Serialization;

namespace WSCT.EMV.Personalization
{
    [DataContract]
    public class Track2EquivalentDataModel
    {
        #region >> Fields

        private string expirationDate;
        private string serviceCode;
        private string discretionaryData;

        #endregion

        /// <summary>
        /// Primary Account Number (n, var up to 19)
        /// </summary>
        [DataMember]
        public string Pan { get; set; }

        /// <summary>
        /// Expiration Date YYMM (n 4)
        /// </summary>
        [DataMember(Name = "expiration-date")]
        public string ExpirationDate
        {
            get { return expirationDate ?? String.Empty; }
            set { expirationDate = value; }
        }

        /// <summary>
        /// Service Code (n 3)
        /// </summary>
        [DataMember(Name = "service-code")]
        public string ServiceCode
        {
            get { return serviceCode ?? String.Empty; }
            set { serviceCode = value; }
        }

        /// <summary>
        /// Discretionary Data (n, var)
        /// </summary>
        [DataMember(Name = "discretionary-data")]
        public string DiscretionaryData
        {
            get { return discretionaryData ?? String.Empty; }
            set { discretionaryData = value; }
        }

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