using System;
using System.Collections.Generic;
using System.Text;

using WSCT.Helpers.BasicEncodingRules;
using WSCT.ISO7816;
using WSCT.Core;
using WSCT.EMV.Commands;

namespace WSCT.EMV.Card
{
    /// <summary>
    /// A <c>PaymentSystemEnvironment</c> instance represents the PSE of an EMV Card.
    /// Allows selection of the PSE and reading the associated EF.
    /// <para>FCI, and records are stored when obtained in <see cref="TLVData">TLVData</see> format.</para>
    /// </summary>
    /// <remarks>
    /// By default, "1PAY.SYS.DDF01" is used for the name of the PSE
    /// </remarks>
    /// <example>
    ///     <code>
    /// ICardChannel cardChannel;
    /// // ... acquire here a valid cardChannel instance ...
    /// PaymentSystemEnvironment pse = new EMVApplication(cardChannel);
    /// pse.name = "1PAY.SYS.DDF01"; // optional, as this value is set by default
    /// pse.select();
    /// pse.read();
    ///     </code>
    /// </example>
    public class PaymentSystemEnvironment : EMVDefinitionFile
    {
        #region >> Fields

        private List<Helpers.BasicEncodingRules.TLVData> _tlvRecords;

        private Boolean _searchTagAIDInFCI = false;

        #endregion

        #region >> Properties

        /// <summary>
        /// Accessor to the records in <c>TLVData</c> format
        /// </summary>
        public List<Helpers.BasicEncodingRules.TLVData> tlvRecords
        {
            get { return _tlvRecords; }
        }

        /// <summary>
        /// Accessor to AID tag location modifier. If set to <c>true</c>, AID are also searched in FCI <b>(not conform with EMV specifications)</b>.
        /// </summary>
        /// <remarks>
        /// Some specific card implementations, like VISA Contactless implementation, does not fully conform to EMV "contact" specifications and need store AID tag in the FCI.
        /// This property allows to support these cards.
        /// </remarks>
        public Boolean searchTagAIDInFCI
        {
            get { return _searchTagAIDInFCI; }
            set { _searchTagAIDInFCI = value; }
        }

        #endregion

        #region >> Delegates

        /// <summary>
        /// Delegate for event sent before execution of a <see cref="read"/>
        /// </summary>
        /// <param name="pse">Caller instance</param>
        public delegate void beforeReadEventHandler(PaymentSystemEnvironment pse);
        /// <summary>
        /// Delegate for event sent after execution of a <see cref="read"/>
        /// </summary>
        /// <param name="pse">Caller instance</param>
        public delegate void afterReadEventHandler(PaymentSystemEnvironment pse);

        /// <summary>
        /// Event sent before execution of a <see cref="read"/>
        /// </summary>
        public event beforeReadEventHandler beforeReadEvent;
        /// <summary>
        /// Event sent after execution of a <see cref="read"/>
        /// </summary>
        public event afterReadEventHandler afterReadEvent;

        #endregion

        #region >> Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="cardChannel"><see cref="ICardChannel">ICardChannel</see> object to use</param>
        public PaymentSystemEnvironment(ICardChannel cardChannel)
            : base(cardChannel)
        {
            _tlvRecords = new List<TLVData>();
            name = "1PAY.SYS.DDF01";
        }

        #endregion

        #region >> Methods

        /// <summary>
        /// Enumerates <see cref="EMVApplication">EMVApplication</see>s discovered by reading the PSE
        /// </summary>
        /// <returns>IEnumerable</returns>
        public System.Collections.IEnumerable getApplications()
        {
            // Enumerate AID indicated in the records
            foreach (TLVData tlvRecord in tlvRecords)
            {
                foreach (TLVData tlvData in tlvRecord.getTags(0x61))
                {
                    EMVApplication emv = new EMVApplication(_cardChannel, tlvData);
                    yield return emv;
                }
            }

            // If AID can also be found in FCI...
            if (searchTagAIDInFCI && tlvFCI != null)
            {
                foreach (TLVData tlvData in tlvFCI.getTags(0x61))
                {
                    EMVApplication emv = new EMVApplication(_cardChannel, tlvData);
                    yield return emv;
                }
            }
        }

        /// <summary>
        /// Reads the file pointed by the SFI found in the FCI of the application
        /// </summary>
        /// <returns>Last status word</returns>
        public UInt16 read()
        {
            if (beforeReadEvent != null) beforeReadEvent(this);

            TLVData tlvSFI = _tlvFCI.getTag(0x88);
            if (tlvSFI == null)
                throw new Exception(String.Format("PSE: no tag 88 (sfi) found in FCI [{0}]", tlvFCI));
            Objects.ShortFileIdentifier sfi = new EMV.Objects.ShortFileIdentifier(tlvSFI);

            byte recordNumber = 0;
            do
            {
                recordNumber++;
                CommandResponsePair crp = new CommandResponsePair();
                crp.cAPDU = new EMVReadRecordCommand(recordNumber, sfi.sfi, 0);
                crp.transmit(_cardChannel);
                _lastStatusWord = crp.rAPDU.statusWord;
                if (crp.rAPDU.statusWord == 0x9000)
                {
                    _tlvRecords.Add(new TLVData(crp.rAPDU.udr));
                }
            } while (_lastStatusWord == 0x9000);

            if (afterReadEvent != null) afterReadEvent(this);

            return _lastStatusWord;
        }

        #endregion
    }
}
