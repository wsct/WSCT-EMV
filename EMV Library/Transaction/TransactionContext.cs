using System.Collections.Generic;
using System.Xml.Serialization;

using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Transaction
{
    /// <summary>
    /// Represents the context of a running transactions.
    /// </summary>
    [XmlRoot("transactionContext")]
    public class TransactionContext
    {
        #region >> Properties

        /// <summary>
        /// List of TLV object associated with the transaction.
        /// </summary>
        [XmlArray("tlvDataList"), XmlArrayItem("tlvData")]
        public List<TLVData> TlvDatas { get; set; }

        #endregion

        #region >> Constructors

        /// <summary>
        /// Initializes a new <see cref="TransactionContext"/> instance.
        /// </summary>
        public TransactionContext()
        {
            TlvDatas = new List<TLVData>();
        }

        #endregion
    }
}
