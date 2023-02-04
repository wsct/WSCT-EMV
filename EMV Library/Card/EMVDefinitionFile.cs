using System;
using WSCT.Core;
using WSCT.EMV.Commands;
using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;
using WSCT.Helpers.Events;
using WSCT.ISO7816;

namespace WSCT.EMV.Card
{
    /// <summary>
    /// An <c>EMVDefinitionFile</c> instance represents a PSE, ADF or DDF.
    /// </summary>
    /// <remarks>
    /// FCI is stored when obtained in <see cref="TlvData">TLVData</see> format.
    ///</remarks>
    /// <example>
    ///     <code>
    /// ICardChannel cardChannel;
    /// // ... acquire here a valid cardChannel instance ...
    /// EMVDefinitionFile df = new EMVApplication(cardChannel);
    /// df.aid = "A0 00 00 00 03 10 10";
    /// df.select();
    /// Console.WriteLine(df.tlvFCI);
    ///     </code>
    /// </example>
    public class EmvDefinitionFile
    {
        #region >> Fields

        /// <summary>
        /// Name or AID of the DF.
        /// </summary>
        internal byte[] _adfName;

        /// <summary>
        /// CardChannel used to access the smartcard.
        /// </summary>
        internal ICardChannel _cardChannel;

        /// <summary>
        /// Status word of the last "usefull" APDU.
        /// </summary>
        internal UInt16 _lastStatusWord;

        #endregion

        #region >> Properties

        /// <summary>
        /// Accessor to the last usefull status word.
        /// </summary>
        public UInt32 LastStatusWord
        {
            get { return _lastStatusWord; }
        }

        /// <summary>
        /// Accessor to the name of the application.
        /// </summary>
        /// <value>
        /// string composed of printable characters, for example <c>"1PAY.SYS.DDF01"</c>.
        /// </value>
        /// <remarks>
        /// <c>name</c> and <c>aid</c> properties are two ways to access the same internal field:
        /// don't use both without care.
        /// </remarks>
        public string Name
        {
            get { return _adfName.ToAsciiString(); }
            set { _adfName = value.FromString(); }
        }

        /// <summary>
        /// Accessor to the AID of the application.
        /// </summary>
        /// <value>string interpretable as hexa numbers, for example <c>"A0 00 00 00 03 10 10"</c></value>
        /// <remarks><c>name</c> and <c>aid</c> are two ways to set the same internal field</remarks>
        public string Aid
        {
            get { return _adfName.ToHexa(); }
            set { _adfName = value.FromHexa(); }
        }

        /// <summary>
        /// Accessor to the FCI in <see cref="TlvData"/> format.
        /// </summary>
        public TlvData TlvFci { get; internal set; }

        #endregion

        #region >> Events

        /// <summary>
        /// Event sent before execution of <see cref="Select"/>.
        /// </summary>
        public event EventHandler<EmvEventArgs> BeforeSelectEvent;

        /// <summary>
        /// Event sent after execution of <see cref="Select"/>.
        /// </summary>
        public event EventHandler<EmvEventArgs> AfterSelectEvent;

        #endregion

        #region >> Constructors

        /// <summary>
        /// Creates a new <see cref="EmvDefinitionFile"/> instance.
        /// </summary>
        /// <param name="cardChannel"><see cref="ICardChannel">ICardChannel</see> object to use</param>
        public EmvDefinitionFile(ICardChannel cardChannel)
        {
            _cardChannel = new CardChannelTerminalTransportLayer(cardChannel);
            TlvFci = null;
        }

        #endregion

        #region >> Methods

        /// <summary>
        /// Selects the DF by its DF Name or AID.
        /// </summary>
        /// <returns>Last status word</returns>
        public UInt16 Select()
        {
            BeforeSelectEvent.Raise(this, new EmvEventArgs());

            // Execute the SELECT
            var crp = new CommandResponsePair(new EMVSelectByNameCommand(_adfName, 0));
            crp.Transmit(_cardChannel);
            _lastStatusWord = crp.RApdu.StatusWord;

            // Finally, store FCI
            if (crp.RApdu.StatusWord == 0x9000)
            {
                TlvFci = new TlvData(crp.RApdu.Udr);
            }

            AfterSelectEvent.Raise(this, new EmvEventArgs());

            return _lastStatusWord;
        }

        #endregion
    }
}