using System;

using WSCT.Core;
using WSCT.EMV.Commands;
using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;
using WSCT.ISO7816;

namespace WSCT.EMV.Card
{
    /// <summary>
    /// An <c>EMVDefinitionFile</c> instance represents a PSE, ADF or DDF.
    /// </summary>
    /// <remarks>
    /// FCI is stored when obtained in <see cref="TLVData">TLVData</see> format.
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
    public class EMVDefinitionFile
    {
        #region >> Fields

        /// <summary>
        /// CardChannel used to access the smartcard.
        /// </summary>
        internal CardChannelISO7816 _cardChannel;

        /// <summary>
        /// Status word of the last "usefull" APDU.
        /// </summary>
        internal UInt16 _lastStatusWord;
        /// <summary>
        /// Name or AID of the DF.
        /// </summary>
        internal Byte[] _adfName;

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
        /// String composed of printable characters, for example <c>"1PAY.SYS.DDF01"</c>.
        /// </value>
        /// <remarks>
        /// <c>name</c> and <c>aid</c> properties are two ways to access the same internal field:
        /// don't use both without care.
        /// </remarks>
        public String Name
        {
            get { return _adfName.toString(); }
            set { _adfName = value.fromString(); }
        }

        /// <summary>
        /// Accessor to the AID of the application.
        /// </summary>
        /// <value>String interpretable as hexa numbers, for example <c>"A0 00 00 00 03 10 10"</c></value>
        /// <remarks><c>name</c> and <c>aid</c> are two ways to set the same internal field</remarks>
        public String Aid
        {
            get { return _adfName.toHexa(); }
            set { _adfName = value.fromHexa(); }
        }

        /// <summary>
        /// Accessor to the FCI in <see cref="TLVData"/> format.
        /// </summary>
        public TLVData TlvFci { get; internal set; }

        #endregion

        #region >> Delegates

        /// <summary>
        /// Delegate for event sent before execution of <see cref="EMVDefinitionFile.Select"/>.
        /// </summary>
        /// <param name="df">Caller instance</param>
        public delegate void BeforeSelectEventHandler(EMVDefinitionFile df);
        /// <summary>
        /// Delegate for event sent after execution of <see cref="EMVDefinitionFile.Select"/>.
        /// </summary>
        /// <param name="df">Caller instance</param>
        public delegate void AfterSelectEventHandler(EMVDefinitionFile df);

        #endregion

        #region >> Events

        /// <summary>
        /// Event sent before execution of <see cref="Select"/>.
        /// </summary>
        public event BeforeSelectEventHandler BeforeSelectEvent;
        /// <summary>
        /// Event sent after execution of <see cref="Select"/>.
        /// </summary>
        public event AfterSelectEventHandler AfterSelectEvent;

        #endregion

        #region >> Constructors

        /// <summary>
        /// Creates a new <see cref="EMVDefinitionFile"/> instance.
        /// </summary>
        /// <param name="cardChannel"><see cref="ICardChannel">ICardChannel</see> object to use</param>
        public EMVDefinitionFile(ICardChannel cardChannel)
        {
            _cardChannel = new CardChannelISO7816(new CardChannelTerminalTransportLayer(cardChannel));
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
            if (BeforeSelectEvent != null) BeforeSelectEvent(this);

            // Execute the SELECT
            var crp = new CommandResponsePair(new EMVSelectByNameCommand(_adfName, 0));
            crp.transmit(_cardChannel);
            _lastStatusWord = crp.rAPDU.statusWord;

            // Finally, store FCI
            if (crp.rAPDU.statusWord == 0x9000)
            {
                TlvFci = new TLVData(crp.rAPDU.udr);
            }

            if (AfterSelectEvent != null) AfterSelectEvent(this);

            return _lastStatusWord;
        }

        #endregion
    }
}
