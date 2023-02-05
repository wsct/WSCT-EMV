using System;
using System.Collections.Generic;
using WSCT.Core;
using WSCT.Core.Fluent.Helpers;
using WSCT.EMV.Commands;
using WSCT.EMV.Objects;
using WSCT.Helpers.BasicEncodingRules;
using WSCT.Helpers.Events;

namespace WSCT.EMV.Card
{
    /// <summary>
    /// A <c>PaymentSystemEnvironment</c> instance represents the PSE of an EMV Card.
    /// Allows selection of the PSE and reading the associated EF.
    /// <para>FCI, and records are stored when obtained in <see cref="TlvData">TLVData</see> format.</para>
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
    public class PaymentSystemEnvironment : EmvDefinitionFile
    {
        #region >> Fields

        private readonly List<TlvData> _tlvRecords;

        #endregion

        #region >> Properties

        /// <summary>
        /// Accessor to the records in <c>TLVData</c> format.
        /// </summary>
        public List<TlvData> TlvRecords => _tlvRecords;

        /// <summary>
        /// Accessor to AID tag location modifier. If set to <c>true</c>, AID are also searched in FCI <b>(not conform with EMV specifications)</b>.
        /// </summary>
        /// <remarks>
        /// Some specific card implementations, like VISA Contactless implementation, does not fully conform to EMV "contact" specifications and need store AID tag in the FCI.
        /// This property allows to support these cards.
        /// </remarks>
        public bool SearchTagAidInFci { get; set; }

        #endregion

        #region >> events

        /// <summary>
        /// Event sent before execution of a <see cref="Read"/>.
        /// </summary>
        public event EventHandler<EmvEventArgs> BeforeReadEvent;

        /// <summary>
        /// Event sent after execution of a <see cref="Read"/>.
        /// </summary>
        public event EventHandler<EmvEventArgs> AfterReadEvent;

        #endregion

        #region >> Constructors

        /// <summary>
        /// Initializes a new <see cref="PaymentSystemEnvironment"/> instance.
        /// </summary>
        /// <param name="cardChannel"><see cref="ICardChannel">ICardChannel</see> object to use.</param>
        public PaymentSystemEnvironment(ICardChannel cardChannel)
            : base(cardChannel)
        {
            SearchTagAidInFci = false;
            _tlvRecords = new List<TlvData>();
            Name = "1PAY.SYS.DDF01";
        }

        #endregion

        #region >> Methods

        /// <summary>
        /// Enumerates <see cref="EmvApplication">EMVApplication</see>s discovered by reading the PSE.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EmvApplication> GetApplications()
        {
            // Enumerate AID indicated in the records
            foreach (var tlvRecord in TlvRecords)
            {
                foreach (var tlvData in tlvRecord.GetTags(0x61))
                {
                    var emv = new EmvApplication(_cardChannel, tlvData);
                    yield return emv;
                }
            }

            // If AID can also be found in FCI...
            if (SearchTagAidInFci && TlvFci != null)
            {
                foreach (var tlvData in TlvFci.GetTags(0x61))
                {
                    var emv = new EmvApplication(_cardChannel, tlvData);
                    yield return emv;
                }
            }
        }

        /// <summary>
        /// Reads the file pointed by the SFI found in the FCI of the application.
        /// </summary>
        /// <returns>Last status word.</returns>
        public UInt16 Read()
        {
            BeforeReadEvent.Raise(this, new EmvEventArgs());

            var tlvSfi = TlvFci.GetTag(0x88);
            if (tlvSfi == null)
            {
                throw new Exception($"PSE: no tag 88 (sfi) found in FCI [{TlvFci}]");
            }
            var sfi = new ShortFileIdentifier(tlvSfi);

            byte recordNumber = 0;
            do
            {
                recordNumber++;
                var crp = new EMVReadRecordCommand(recordNumber, sfi.Sfi, 0)
                    .Transmit(_cardChannel)
                    .WithResponse(r => _lastStatusWord = r.StatusWord)
                    .OnStatusWord(0x9000, (_, r) => _tlvRecords.Add(new TlvData(r.Udr)));
            } while (_lastStatusWord == 0x9000);

            AfterReadEvent.Raise(this, new EmvEventArgs());

            return _lastStatusWord;
        }

        #endregion
    }
}