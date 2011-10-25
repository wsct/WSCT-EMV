using System;
using System.Collections.Generic;
using System.Text;

using WSCT.EMV;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents the Cardholder Verification Method List of an EMV application
    /// </summary>
    public class CardholderVerificationMethodList : AbstractTLVObject
    {
        #region >> Enumerations

        /// <summary>
        /// CV Rule Byte 1 (Leftmost):  Cardholder Verification Method (CVM) Codes
        /// </summary>
        public enum CVMCode
        {
            /// <summary>
            /// Fail CVM Processing
            /// </summary>
            FAIL_CVM = 0x00,
            /// <summary>
            /// Plaintext PIN verification performed by ICC
            /// </summary>
            PLAINTEXTPIN_ICC = 0x01,
            /// <summary>
            /// Enciphered PIN verified online
            /// </summary>
            ENCIPHEREDPIN_ONLINE = 0x02,
            /// <summary>
            /// Plaintext PIN verification performed by ICC and signature (paper)
            /// </summary>
            PLAINTEXTPIN_ICC_AND_SIGN = 0x03,
            /// <summary>
            /// Enciphered PIN verification performed by ICC
            /// </summary>
            ENCIPHEREDPIN_ICC = 0x04,
            /// <summary>
            ///  Enciphered PIN verification performed by ICC and signature (paper)
            /// </summary>
            ENCIPHEREDPIN_ICC_AND_SIGN = 0x05,
            /// <summary>
            /// Signature (paper)
            /// </summary>
            SIGN = 0x1E,
            /// <summary>
            /// No CVM required
            /// </summary>
            NOCVM_REQUIRED = 0x1F
        }

        /// <summary>
        /// CV Rule Byte 2 (Rightmost):  Cardholder Verification Method (CVM) Condition Codes
        /// </summary>
        public enum CVMCondition
        {
            /// <summary>
            /// Always
            /// </summary>
            ALWAYS = 0x00,
            /// <summary>
            /// If unattended cash
            /// </summary>
            UNATTENDED_CASH = 0x01,
            /// <summary>
            /// If not unattended cash and not manual cash and not purchase with cashback
            /// </summary>
            NOTCASH = 0x02,
            /// <summary>
            /// If terminal supports the CVM
            /// </summary>
            SUPPORT = 0x03,
            /// <summary>
            /// If manual cash
            /// </summary>
            CASH = 0x04,
            /// <summary>
            /// If purchase with cashback
            /// </summary>
            PURCHASE_CASHBACK = 0x05,
            /// <summary>
            /// If transaction is in the application currency  22  and is under X value
            /// </summary>
            UNDER_AMOUNTX = 0x06,
            /// <summary>
            /// If transaction is in the application currency and is over X value
            /// </summary>
            OVER_AMOUNTX = 0x07,
            /// <summary>
            /// If transaction is in the application currency and is under Y value
            /// </summary>
            UNDER_AMOUNTY = 0x08,
            /// <summary>
            /// If transaction is in the application currency and is over Y value
            /// </summary>
            OVER_AMOUNTY = 0x09
        }

        #endregion

        #region >> Class CVRule

        /// <summary>
        /// Represents a CVM
        /// </summary>
        public class CVRule
        {
            # region >> Fields

            UInt16 _cvRule;

            #endregion

            #region >> Properties

            /// <summary>
            /// CVM Code (6 less significant bytes of first byte)
            /// </summary>
            public CVMCode cvmCode
            {
                get { return (CVMCode)((_cvRule / 0x100) & 0x3F); }
            }

            /// <summary>
            /// CVM Condition (second byte)
            /// </summary>
            public CVMCondition cvmCondition
            {
                get { return (CVMCondition)(_cvRule % 0x100); }
            }

            /// <summary>
            /// If true: Apply succeeding CV Rule if this CVM is unsuccessful. If <c>false</c>: Fail cardholder verification if this CVM is unsuccessful
            /// </summary>
            public Boolean cvmTryable
            {
                get { return (_cvRule & 0x8000) == 0x8000; }
            }

            #endregion

            #region >> Constructors

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="twoBytes">Two bytes identifying the cv rule</param>
            public CVRule(Byte[] twoBytes)
                : this(twoBytes, 0)
            {
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="twoBytes">Two bytes identifying the cv rule</param>
            /// <param name="offset">Offset in the array <paramref>twoBytes</paramref></param>
            public CVRule(Byte[] twoBytes, Byte offset)
            {
                _cvRule = (UInt16)(twoBytes[offset + 0] * 0x100 + twoBytes[offset + 1]);
            }

            #endregion

            /// <inheritdoc />
            public override string ToString()
            {
                return String.Format("cvr:{0:X4} ({1}={2}+{3})", _cvRule, cvmTryable, cvmCode, cvmCondition);
            }
        }

        #endregion

        #region >> Fields

        List<CVRule> _cvRules;

        #endregion

        #region >> Properties

        /// <summary>
        /// First amount field expressed in Application Currency Code with implicit decimal point
        /// </summary>
        public UInt32 amountX
        {
            get { return (UInt32)(tlv.value[0] * 0x1000000 + tlv.value[1] * 0x10000 + tlv.value[2] * 0x100 + tlv.value[3]); }
        }

        /// <summary>
        /// Second amount field expressed in Application Currency Code with implicit decimal point
        /// </summary>
        public UInt32 amountY
        {
            get { return (UInt32)(tlv.value[4] * 0x1000000 + tlv.value[5] * 0x10000 + tlv.value[6] * 0x100 + tlv.value[7]); }
        }

        /// <summary>
        /// Accessor to Cardholder Verification Rules
        /// </summary>
        public List<CardholderVerificationMethodList.CVRule> cvRules
        {
            get
            {
                if (_cvRules == null)
                {
                    _cvRules = new List<CVRule>();
                    // CVRules starts at byte 8 in CVMList
                    Byte offset = 8;
                    while (offset < tlv.value.Length)
                    {
                        _cvRules.Add(new CVRule(tlv.value, offset));
                        offset += 2;
                    }
                }
                return _cvRules;
            }
        }

        #endregion

        #region >> Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public CardholderVerificationMethodList()
            : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cvmlData">Raw CVM List data</param>
        public CardholderVerificationMethodList(Byte[] cvmlData)
            : this()
        {
            tlv = new TLVData();
            tlv.value = cvmlData;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tlvCVM">TLVData representing the CVM list</param>
        public CardholderVerificationMethodList(TLVData tlvCVM)
            : this()
        {
            tlv = tlvCVM;
        }

        #endregion

        #region >> Methods



        #endregion

        /// <inheritdoc />
        public override string ToString()
        {
            String s = String.Format("X:{0} Y:{1}", amountX, amountY);
            foreach (CVRule cvr in cvRules)
                s += String.Format(" {0}", cvr);
            return s;
        }
    }
}
