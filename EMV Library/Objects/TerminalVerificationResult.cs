using System;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents the Terminal Verification Result, Issuer Action Code or Terminal Action Code of an EMV application.
    /// </summary>
    public class TerminalVerificationResult : BinaryTLVObject
    {
        #region >> Internal constants

        const byte OFFLINE_DATA_AUTH = 0x80;
        const byte SDA = 0x40;
        const byte ICCDATA = 0x20;
        const byte TERMINALEXCEPTIONFILE = 0x10;
        const byte DDA = 0x08;
        const byte CDA = 0x04;

        const byte ICCTERMINALVERSION = 0x80;
        const byte EXPIREDAPPLICATION = 0x40;
        const byte NOTYETEFFECTIVEAPPLICATION = 0x20;
        const byte SERVICENOTALLOWED = 0x10;
        const byte NEWCARD = 0x08;

        const byte CARDHOLDERVERIFICATIONFAILED = 0x80;
        const byte UNRECOGNISED_CVM = 0x40;
        const byte PINTRYLIMIT = 0x20;
        const byte PINPAD_ERROR = 0x10;
        const byte PIN_NOTENTERED = 0x08;
        const byte ONLINEPIN_ENTERED = 0x04;

        const byte TRANSACTION_FLOORLIMIT = 0x80;
        const byte LOWER_OFFLINELIMIT = 0x40;
        const byte UPPER_OFFLINELIMIT = 0x20;
        const byte RANDOMLYSELECTED_ONLINE = 0x10;
        const byte MERCHANT_FORCEDONLINE = 0x08;

        const byte DEFAULT_TDOL_USED = 0x80;
        const byte ISSUER_AUTHENTICATION_FAILED = 0x40;
        const byte SCRIPT_FAILED_BEFORE_GENERATEAC = 0x20;
        const byte SCRIPT_FAILED_AFTER_GENERATEAC = 0x10;

        #endregion

        #region >> Properties

        /// <summary>
        /// TVR: Offline data authentication was not performed.
        /// </summary>
        public Boolean OfflineDataAuthenticationNotPerformed
        {
            get { return Test(0, OFFLINE_DATA_AUTH); }
            set { Force(0, OFFLINE_DATA_AUTH, value); }
        }

        /// <summary>
        /// TVR: SDA failed.
        /// </summary>
        public Boolean SdaFailed
        {
            get { return Test(0, SDA); }
            set { Force(0, SDA, value); }
        }

        /// <summary>
        /// TVR: ICC data missing.
        /// </summary>
        public Boolean IccDataMissing
        {
            get { return Test(0, ICCDATA); }
            set { Force(0, ICCDATA, value); }
        }

        /// <summary>
        /// TVR: Card appears on terminal exception file.
        /// </summary>
        public Boolean TerminalExceptionFile
        {
            get { return Test(0, TERMINALEXCEPTIONFILE); }
            set { Force(0, TERMINALEXCEPTIONFILE, value); }
        }

        /// <summary>
        /// TVR: DDA failed.
        /// </summary>
        public Boolean DdaFailed
        {
            get { return Test(0, DDA); }
            set { Force(0, DDA, value); }
        }

        /// <summary>
        /// TVR: CDA failed.
        /// </summary>
        public Boolean CdaFailed
        {
            get { return Test(0, CDA); }
            set { Force(0, CDA, value); }
        }

        /// <summary>
        /// TVR: ICC and terminal have different application versions.
        /// </summary>
        public Boolean IccAndTerminalVersionsDifferent
        {
            get { return Test(1, ICCTERMINALVERSION); }
            set { Force(1, ICCTERMINALVERSION, value); }
        }

        /// <summary>
        /// TVR: Expired application.
        /// </summary>
        public Boolean ExpiredApplication
        {
            get { return Test(1, EXPIREDAPPLICATION); }
            set { Force(1, EXPIREDAPPLICATION, value); }
        }

        /// <summary>
        /// TVR: Application not yet effective.
        /// </summary>
        public Boolean NotYetEffectiveApplication
        {
            get { return Test(1, NOTYETEFFECTIVEAPPLICATION); }
            set { Force(1, NOTYETEFFECTIVEAPPLICATION, value); }
        }

        /// <summary>
        /// TVR: Requested service not allowed for card product.
        /// </summary>
        public Boolean ServiceNotAllowed
        {
            get { return Test(1, SERVICENOTALLOWED); }
            set { Force(1, SERVICENOTALLOWED, value); }
        }

        /// <summary>
        /// TVR: New card.
        /// </summary>
        public Boolean NewCard
        {
            get { return Test(1, NEWCARD); }
            set { Force(1, NEWCARD, value); }
        }

        /// <summary>
        /// TVR: Cardholder verification was not successful.
        /// </summary>
        public Boolean CardholderVerificationFailed
        {
            get { return Test(2, CARDHOLDERVERIFICATIONFAILED); }
            set { Force(2, CARDHOLDERVERIFICATIONFAILED, value); }
        }

        /// <summary>
        /// TVR: Unrecognised CVM.
        /// </summary>
        public Boolean UnrecognisedCvm
        {
            get { return Test(2, UNRECOGNISED_CVM); }
            set { Force(2, UNRECOGNISED_CVM, value); }
        }

        /// <summary>
        /// TVR: PIN Try limit exceeded.
        /// </summary>
        public Boolean PINTryLimitExceeded
        {
            get { return Test(2, PINTRYLIMIT); }
            set { Force(2, PINTRYLIMIT, value); }
        }

        /// <summary>
        /// TVR: PIN entry required and PIN pad not present or not working.
        /// </summary>
        public Boolean PinpadError
        {
            get { return Test(2, PINPAD_ERROR); }
            set { Force(2, PINPAD_ERROR, value); }
        }

        /// <summary>
        /// TVR: PIN entry required, PIN pad present, but PIN was not entered.
        /// </summary>
        public Boolean PINNotEntered
        {
            get { return Test(2, PIN_NOTENTERED); }
            set { Force(2, PIN_NOTENTERED, value); }
        }

        /// <summary>
        /// TVR: Online PIN entered.
        /// </summary>
        public Boolean OnlinePinEntered
        {
            get { return Test(2, ONLINEPIN_ENTERED); }
            set { Force(2, ONLINEPIN_ENTERED, value); }
        }

        /// <summary>
        /// TVR: Transaction exceeds floor limit.
        /// </summary>
        public Boolean TransactionExceedFloorLimit
        {
            get { return Test(3, TRANSACTION_FLOORLIMIT); }
            set { Force(3, TRANSACTION_FLOORLIMIT, value); }
        }

        /// <summary>
        /// TVR: Lower consecutive offline limit exceeded.
        /// </summary>
        public Boolean LowerConsecutiveOfflineLimitExceeded
        {
            get { return Test(3, LOWER_OFFLINELIMIT); }
            set { Force(3, LOWER_OFFLINELIMIT, value); }
        }

        /// <summary>
        /// TVR: Upper consecutive offline limite exceeded.
        /// </summary>
        public Boolean UpperConsecutiveOfflineLimitExceeded
        {
            get { return Test(3, UPPER_OFFLINELIMIT); }
            set { Force(3, UPPER_OFFLINELIMIT, value); }
        }

        /// <summary>
        /// TVR: Transaction selected randomly for online processing.
        /// </summary>
        public Boolean TransactionRandomlySelectedOnline
        {
            get { return Test(3, RANDOMLYSELECTED_ONLINE); }
            set { Force(3, RANDOMLYSELECTED_ONLINE, value); }
        }

        /// <summary>
        /// TVR: Merchant forced transaction online.
        /// </summary>
        public Boolean MerchantForcedTransactionOnline
        {
            get { return Test(3, MERCHANT_FORCEDONLINE); }
            set { Force(3, MERCHANT_FORCEDONLINE, value); }
        }

        /// <summary>
        /// TVR: Default TDOL used.
        /// </summary>
        public Boolean DefaultTdolUsed
        {
            get { return Test(4, DEFAULT_TDOL_USED); }
            set { Force(4, DEFAULT_TDOL_USED, value); }
        }

        /// <summary>
        /// Issuer authentication failed.
        /// </summary>
        public Boolean IssuerAuthenticationFailed
        {
            get { return Test(4, ISSUER_AUTHENTICATION_FAILED); }
            set { Force(4, ISSUER_AUTHENTICATION_FAILED, value); }
        }

        /// <summary>
        /// Script processing failed before final GENERATE AC.
        /// </summary>
        public Boolean ScriptProcessingFailedBeforeGenerateAC
        {
            get { return Test(4, SCRIPT_FAILED_BEFORE_GENERATEAC); }
            set { Force(4, SCRIPT_FAILED_BEFORE_GENERATEAC, value); }
        }

        /// <summary>
        /// Script processing failed after final GENERATE AC.
        /// </summary>
        public Boolean ScriptProcessingFailedAfterGenerateAC
        {
            get { return Test(4, SCRIPT_FAILED_AFTER_GENERATEAC); }
            set { Force(4, SCRIPT_FAILED_AFTER_GENERATEAC, value); }
        }

        #endregion

        #region >> Constructors

        /// <summary>
        /// Initializes a new <see cref="TerminalVerificationResult"/> instance.
        /// </summary>
        public TerminalVerificationResult()
        {
            tlv = new TLVData();
        }

        /// <summary>
        /// Initializes a new <see cref="TerminalVerificationResult"/> instance.
        /// </summary>
        /// <param name="tlvTVR">TLV TVR data.</param>
        public TerminalVerificationResult(TLVData tlvTVR)
            : this()
        {
            tlv = tlvTVR;
        }

        #endregion

        #region >> Methods

        Boolean Test(byte byteNumber, byte bit)
        {
            if (tlv.value.Length == 0)
            {
                tlv.value = new Byte[5];
            }

            return (tlv.value[byteNumber] & bit) == bit;
        }

        void Force(byte byteNumber, byte bit, Boolean value)
        {
            if (tlv.value.Length == 0)
            {
                tlv.value = new Byte[5];
            }
            if (value)
            {
                tlv.value[byteNumber] |= bit;
            }
            else
            {
                tlv.value[byteNumber] &= (byte)(~bit);
            }
        }

        #endregion
    }
}
