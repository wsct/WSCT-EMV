using System;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents the Terminal Verification Result, Issuer Action Code or Terminal Action Code of an EMV application.
    /// </summary>
    public class TerminalVerificationResult : BinaryTLVObject
    {
        #region >> Nested Enums

        /// <summary>
        /// Bit signification of TVR first byte.
        /// </summary>
        public enum Byte1
        {
            /// <summary>
            /// 
            /// </summary>
            OfflineDataAuth = 0x80,

            /// <summary>
            /// 
            /// </summary>
            StaticDataAuthentication = 0x40,

            /// <summary>
            /// 
            /// </summary>
            IccData = 0x20,

            /// <summary>
            /// 
            /// </summary>
            TerminalExceptionFile = 0x10,

            /// <summary>
            /// 
            /// </summary>
            DynamicDataAuthentication = 0x08,

            /// <summary>
            /// 
            /// </summary>
            CombinedDataAuthentication = 0x04
        };

        /// <summary>
        /// Bit signification of TVR second byte.
        /// </summary>
        public enum Byte2
        {
            /// <summary>
            /// 
            /// </summary>
            IccTerminalVersion = 0x80,

            /// <summary>
            /// 
            /// </summary>
            ExpiredApplication = 0x40,

            /// <summary>
            /// 
            /// </summary>
            NotYetEffectiveApplication = 0x20,

            /// <summary>
            /// 
            /// </summary>
            ServiceNotAllowed = 0x10,

            /// <summary>
            /// 
            /// </summary>
            NewCard = 0x08
        }

        /// <summary>
        /// Bit signification of TVR third byte.
        /// </summary>
        public enum Byte3
        {
            /// <summary>
            /// 
            /// </summary>
            CardholderVerificationFailed = 0x80,

            /// <summary>
            /// 
            /// </summary>
            UnrecognisedCvm = 0x40,

            /// <summary>
            /// 
            /// </summary>
            PinTrylimit = 0x20,

            /// <summary>
            /// 
            /// </summary>
            PinpadError = 0x10,

            /// <summary>
            /// 
            /// </summary>
            PINNotentered = 0x08,

            /// <summary>
            /// 
            /// </summary>
            OnlinepinEntered = 0x04
        }

        /// <summary>
        /// Bit signification of fourth TVR byte.
        /// </summary>
        public enum Byte4
        {
            /// <summary>
            /// 
            /// </summary>
            TransactionFloorLimit = 0x80,

            /// <summary>
            /// 
            /// </summary>
            LowerOfflineLimit = 0x40,

            /// <summary>
            /// 
            /// </summary>
            UpperOfflineLimit = 0x20,

            /// <summary>
            /// 
            /// </summary>
            RandomlySelectedOnline = 0x10,

            /// <summary>
            /// 
            /// </summary>
            MerchantForcedOnline = 0x08
        }

        /// <summary>
        /// Bit signification of fifth TVR byte.
        /// </summary>
        public enum Byte5
        {
            /// <summary>
            /// 
            /// </summary>
            DefaultTdolUsed = 0x80,
            /// <summary>
            /// 
            /// </summary>
            IssuerAuthenticationFailed = 0x40,
            /// <summary>
            /// 
            /// </summary>
            ScriptFailedBeforeGenerateAc = 0x20,
            /// <summary>
            /// 
            /// </summary>
            ScriptFailedAfterGenerateAc = 0x10
        }

        #endregion

        #region >> Properties

        /// <summary>
        /// TVR: Offline data authentication was not performed.
        /// </summary>
        public Boolean OfflineDataAuthenticationNotPerformed
        {
            get { return Test(0, (int)Byte1.OfflineDataAuth); }
            set { Force(0, (int)Byte1.OfflineDataAuth, value); }
        }

        /// <summary>
        /// TVR: SDA failed.
        /// </summary>
        public Boolean SdaFailed
        {
            get { return Test(0, (int)Byte1.StaticDataAuthentication); }
            set { Force(0, (int)Byte1.StaticDataAuthentication, value); }
        }

        /// <summary>
        /// TVR: ICC data missing.
        /// </summary>
        public Boolean IccDataMissing
        {
            get { return Test(0, (int)Byte1.IccData); }
            set { Force(0, (int)Byte1.IccData, value); }
        }

        /// <summary>
        /// TVR: Card appears on terminal exception file.
        /// </summary>
        public Boolean TerminalExceptionFile
        {
            get { return Test(0, (int)Byte1.TerminalExceptionFile); }
            set { Force(0, (int)Byte1.TerminalExceptionFile, value); }
        }

        /// <summary>
        /// TVR: DDA failed.
        /// </summary>
        public Boolean DdaFailed
        {
            get { return Test(0, (int)Byte1.DynamicDataAuthentication); }
            set { Force(0, (int)Byte1.DynamicDataAuthentication, value); }
        }

        /// <summary>
        /// TVR: CDA failed.
        /// </summary>
        public Boolean CdaFailed
        {
            get { return Test(0, (int)Byte1.CombinedDataAuthentication); }
            set { Force(0, (int)Byte1.CombinedDataAuthentication, value); }
        }

        /// <summary>
        /// TVR: ICC and terminal have different application versions.
        /// </summary>
        public Boolean IccAndTerminalVersionsDifferent
        {
            get { return Test(1, (int)Byte2.IccTerminalVersion); }
            set { Force(1, (int)Byte2.IccTerminalVersion, value); }
        }

        /// <summary>
        /// TVR: Expired application.
        /// </summary>
        public Boolean ExpiredApplication
        {
            get { return Test(1, (int)Byte2.ExpiredApplication); }
            set { Force(1, (int)Byte2.ExpiredApplication, value); }
        }

        /// <summary>
        /// TVR: Application not yet effective.
        /// </summary>
        public Boolean NotYetEffectiveApplication
        {
            get { return Test(1, (int)Byte2.NotYetEffectiveApplication); }
            set { Force(1, (int)Byte2.NotYetEffectiveApplication, value); }
        }

        /// <summary>
        /// TVR: Requested service not allowed for card product.
        /// </summary>
        public Boolean ServiceNotAllowed
        {
            get { return Test(1, (int)Byte2.ServiceNotAllowed); }
            set { Force(1, (int)Byte2.ServiceNotAllowed, value); }
        }

        /// <summary>
        /// TVR: New card.
        /// </summary>
        public Boolean NewCard
        {
            get { return Test(1, (int)Byte2.NewCard); }
            set { Force(1, (int)Byte2.NewCard, value); }
        }

        /// <summary>
        /// TVR: Cardholder verification was not successful.
        /// </summary>
        public Boolean CardholderVerificationFailed
        {
            get { return Test(2, (int)Byte3.CardholderVerificationFailed); }
            set { Force(2, (int)Byte3.CardholderVerificationFailed, value); }
        }

        /// <summary>
        /// TVR: Unrecognised CVM.
        /// </summary>
        public Boolean UnrecognisedCvm
        {
            get { return Test(2, (int)Byte3.UnrecognisedCvm); }
            set { Force(2, (int)Byte3.UnrecognisedCvm, value); }
        }

        /// <summary>
        /// TVR: PIN Try limit exceeded.
        /// </summary>
        public Boolean PINTryLimitExceeded
        {
            get { return Test(2, (int)Byte3.PinTrylimit); }
            set { Force(2, (int)Byte3.PinTrylimit, value); }
        }

        /// <summary>
        /// TVR: PIN entry required and PIN pad not present or not working.
        /// </summary>
        public Boolean PinpadError
        {
            get { return Test(2, (int)Byte3.PinpadError); }
            set { Force(2, (int)Byte3.PinpadError, value); }
        }

        /// <summary>
        /// TVR: PIN entry required, PIN pad present, but PIN was not entered.
        /// </summary>
        public Boolean PINNotEntered
        {
            get { return Test(2, (int)Byte3.PINNotentered); }
            set { Force(2, (int)Byte3.PINNotentered, value); }
        }

        /// <summary>
        /// TVR: Online PIN entered.
        /// </summary>
        public Boolean OnlinePinEntered
        {
            get { return Test(2, (int)Byte3.OnlinepinEntered); }
            set { Force(2, (int)Byte3.OnlinepinEntered, value); }
        }

        /// <summary>
        /// TVR: Transaction exceeds floor limit.
        /// </summary>
        public Boolean TransactionExceedFloorLimit
        {
            get { return Test(3, (int)Byte4.TransactionFloorLimit); }
            set { Force(3, (int)Byte4.TransactionFloorLimit, value); }
        }

        /// <summary>
        /// TVR: Lower consecutive offline limit exceeded.
        /// </summary>
        public Boolean LowerConsecutiveOfflineLimitExceeded
        {
            get { return Test(3, (int)Byte4.LowerOfflineLimit); }
            set { Force(3, (int)Byte4.LowerOfflineLimit, value); }
        }

        /// <summary>
        /// TVR: Upper consecutive offline limite exceeded.
        /// </summary>
        public Boolean UpperConsecutiveOfflineLimitExceeded
        {
            get { return Test(3, (int)Byte4.UpperOfflineLimit); }
            set { Force(3, (int)Byte4.UpperOfflineLimit, value); }
        }

        /// <summary>
        /// TVR: Transaction selected randomly for online processing.
        /// </summary>
        public Boolean TransactionRandomlySelectedOnline
        {
            get { return Test(3, (int)Byte4.RandomlySelectedOnline); }
            set { Force(3, (int)Byte4.RandomlySelectedOnline, value); }
        }

        /// <summary>
        /// TVR: Merchant forced transaction online.
        /// </summary>
        public Boolean MerchantForcedTransactionOnline
        {
            get { return Test(3, (int)Byte4.MerchantForcedOnline); }
            set { Force(3, (int)Byte4.MerchantForcedOnline, value); }
        }

        /// <summary>
        /// TVR: Default TDOL used.
        /// </summary>
        public Boolean DefaultTdolUsed
        {
            get { return Test(4, (int)Byte5.DefaultTdolUsed); }
            set { Force(4, (int)Byte5.DefaultTdolUsed, value); }
        }

        /// <summary>
        /// Issuer authentication failed.
        /// </summary>
        public Boolean IssuerAuthenticationFailed
        {
            get { return Test(4, (int)Byte5.IssuerAuthenticationFailed); }
            set { Force(4, (int)Byte5.IssuerAuthenticationFailed, value); }
        }

        /// <summary>
        /// Script processing failed before final GENERATE AC.
        /// </summary>
        public Boolean ScriptProcessingFailedBeforeGenerateAC
        {
            get { return Test(4, (int)Byte5.ScriptFailedBeforeGenerateAc); }
            set { Force(4, (int)Byte5.ScriptFailedBeforeGenerateAc, value); }
        }

        /// <summary>
        /// Script processing failed after final GENERATE AC.
        /// </summary>
        public Boolean ScriptProcessingFailedAfterGenerateAC
        {
            get { return Test(4, (int)Byte5.ScriptFailedAfterGenerateAc); }
            set { Force(4, (int)Byte5.ScriptFailedAfterGenerateAc, value); }
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
        /// <param name="tlvTvr">TLV TVR data.</param>
        public TerminalVerificationResult(TLVData tlvTvr)
            : this()
        {
            tlv = tlvTvr;
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
