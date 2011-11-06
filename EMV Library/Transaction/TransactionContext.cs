using System.Collections.Generic;
using System.Xml.Serialization;

using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Transaction
{
    [XmlRoot("transactionContext")]
    public class TransactionContext
    {
        #region >> Fields

        List<TLVData> _tlvDatas;

        #endregion

        #region >> Properties

        [XmlArray("tlvDataList")]
        [XmlArrayItem("tlvData")]
        public List<TLVData> tlvDatas
        {
            get { return _tlvDatas; }
            set { _tlvDatas = value; }
        }

        #endregion

        #region >> Constructors

        public TransactionContext()
        {
            _tlvDatas = new List<TLVData>();
        }

        #endregion
    }
}
