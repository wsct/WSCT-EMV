using System;
using System.Collections.Generic;
using System.Linq;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents the Cardholder Verification Method List of an EMV application.
    /// </summary>
    public class CardholderVerificationMethodList : AbstractTlvObject
    {
        #region >> Enumerations

        /// <summary>
        /// CV Rule byte 1 (Leftmost):  Cardholder Verification Method (CVM) Codes.
        /// </summary>
        public enum CvmCode
        {
            /// <summary>
            /// Fail CVM Processing.
            /// </summary>
            FailCvm = 0x00,

            /// <summary>
            /// Plaintext PIN verification performed by ICC.
            /// </summary>
            PlaintextpinIcc = 0x01,

            /// <summary>
            /// Enciphered PIN verified online.
            /// </summary>
            EncipheredpinOnline = 0x02,

            /// <summary>
            /// Plaintext PIN verification performed by ICC and signature (paper).
            /// </summary>
            PlaintextpinIccAndSign = 0x03,

            /// <summary>
            /// Enciphered PIN verification performed by ICC.
            /// </summary>
            EncipheredpinIcc = 0x04,

            /// <summary>
            ///  Enciphered PIN verification performed by ICC and signature (paper).
            /// </summary>
            EncipheredpinIccAndSign = 0x05,

            /// <summary>
            /// Signature (paper).
            /// </summary>
            Sign = 0x1E,

            /// <summary>
            /// No CVM required.
            /// </summary>
            NocvmRequired = 0x1F
        }

        /// <summary>
        /// CV Rule byte 2 (Rightmost):  Cardholder Verification Method (CVM) Condition Codes.
        /// </summary>
        public enum CvmCondition
        {
            /// <summary>
            /// Always.
            /// </summary>
            Always = 0x00,

            /// <summary>
            /// If unattended cash.
            /// </summary>
            UnattendedCash = 0x01,

            /// <summary>
            /// If not unattended cash and not manual cash and not purchase with cashback.
            /// </summary>
            Notcash = 0x02,

            /// <summary>
            /// If terminal supports the CVM.
            /// </summary>
            Support = 0x03,

            /// <summary>
            /// If manual cash.
            /// </summary>
            Cash = 0x04,

            /// <summary>
            /// If purchase with cashback.
            /// </summary>
            PurchaseCashback = 0x05,

            /// <summary>
            /// If transaction is in the application currency  22  and is under X value.
            /// </summary>
            UnderAmountx = 0x06,

            /// <summary>
            /// If transaction is in the application currency and is over X value.
            /// </summary>
            OverAmountx = 0x07,

            /// <summary>
            /// If transaction is in the application currency and is under Y value.
            /// </summary>
            UnderAmounty = 0x08,

            /// <summary>
            /// If transaction is in the application currency and is over Y value.
            /// </summary>
            OverAmounty = 0x09
        }

        #endregion

        #region >> Class CVRule

        /// <summary>
        /// Represents a CVM.
        /// </summary>
        public class CvRule
        {
            # region >> Fields

            private readonly UInt16 _cvRule;

            #endregion

            #region >> Properties

            /// <summary>
            /// CVM Code (6 less significant bytes of first byte).
            /// </summary>
            public CvmCode CvmCode
            {
                get { return (CvmCode)((_cvRule/0x100) & 0x3F); }
            }

            /// <summary>
            /// CVM Condition (second byte).
            /// </summary>
            public CvmCondition CvmCondition
            {
                get { return (CvmCondition)(_cvRule%0x100); }
            }

            /// <summary>
            /// If true: Apply succeeding CV Rule if this CVM is unsuccessful. If <c>false</c>: Fail cardholder verification if this CVM is unsuccessful.
            /// </summary>
            public Boolean CvmTryable
            {
                get { return (_cvRule & 0x8000) == 0x8000; }
            }

            #endregion

            #region >> Constructors

            /// <summary>
            /// Initializes a new <see cref="CvRule"/> instance.
            /// </summary>
            /// <param name="twoBytes">Two bytes identifying the cv rule.</param>
            public CvRule(byte[] twoBytes)
                : this(twoBytes, 0)
            {
            }

            /// <summary>
            /// Initializes a new <see cref="CvRule"/> instance.
            /// </summary>
            /// <param name="twoBytes">Two bytes identifying the cv rule.</param>
            /// <param name="offset">Offset in the array <paramref name="twoBytes"/>.</param>
            public CvRule(byte[] twoBytes, byte offset)
            {
                _cvRule = (UInt16)(twoBytes[offset + 0]*0x100 + twoBytes[offset + 1]);
            }

            #endregion

            #region >> Object

            /// <inheritdoc />
            public override string ToString()
            {
                return String.Format("cvr:{0:X4} ({1}={2}+{3})", _cvRule, CvmTryable, CvmCode, CvmCondition);
            }

            #endregion
        }

        #endregion

        #region >> Fields

        private List<CvRule> _cvRules;

        #endregion

        #region >> Properties

        /// <summary>
        /// First amount field expressed in Application Currency Code with implicit decimal point.
        /// </summary>
        public UInt32 AmountX
        {
            get { return (UInt32)(Tlv.Value[0]*0x1000000 + Tlv.Value[1]*0x10000 + Tlv.Value[2]*0x100 + Tlv.Value[3]); }
        }

        /// <summary>
        /// Second amount field expressed in Application Currency Code with implicit decimal point.
        /// </summary>
        public UInt32 AmountY
        {
            get { return (UInt32)(Tlv.Value[4]*0x1000000 + Tlv.Value[5]*0x10000 + Tlv.Value[6]*0x100 + Tlv.Value[7]); }
        }

        /// <summary>
        /// Accessor to Cardholder Verification Rules.
        /// </summary>
        public List<CvRule> CvRules
        {
            get
            {
                if (_cvRules == null)
                {
                    _cvRules = new List<CvRule>();
                    // CVRules starts at byte 8 in CVMList
                    byte offset = 8;
                    while (offset < Tlv.Value.Length)
                    {
                        _cvRules.Add(new CvRule(Tlv.Value, offset));
                        offset += 2;
                    }
                }
                return _cvRules;
            }
        }

        #endregion

        #region >> Constructors

        /// <summary>
        /// Initializes a new <see cref="CardholderVerificationMethodList"/> instance.
        /// </summary>
        public CardholderVerificationMethodList()
        {
        }

        /// <summary>
        /// Initializes a new <see cref="CardholderVerificationMethodList"/> instance.
        /// </summary>
        /// <param name="cvmlData">Raw CVM List data.</param>
        public CardholderVerificationMethodList(byte[] cvmlData)
            : this()
        {
            Tlv = new TlvData { Value = cvmlData };
        }

        /// <summary>
        /// Initializes a new <see cref="CardholderVerificationMethodList"/> instance.
        /// </summary>
        /// <param name="tlvCvm">TLVData representing the CVM list.</param>
        public CardholderVerificationMethodList(TlvData tlvCvm)
            : this()
        {
            Tlv = tlvCvm;
        }

        #endregion

        #region >> Object

        /// <inheritdoc />
        public override string ToString()
        {
            var s = String.Format("X:{0} Y:{1}", AmountX, AmountY);
            return CvRules.Aggregate(s, (current, cvr) => current + String.Format(" {0}", cvr));
        }

        #endregion
    }
}