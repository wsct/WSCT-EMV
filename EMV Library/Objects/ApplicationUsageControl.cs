using System;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents the Application Usage Control of an EMV application.
    /// </summary>
    public class ApplicationUsageControl : BinaryTlvObject
    {
        #region >> Internal constants

        private const byte DomesticCashBit = 0x80;
        private const byte InternationalCashBit = 0x40;
        private const byte DomesticGoodsBit = 0x20;
        private const byte InternationalGoodsBit = 0x10;
        private const byte DomesticServicesBit = 0x08;
        private const byte InternationalServicesBit = 0x04;
        private const byte AtmBit = 0x02;
        private const byte OtherThanAtmBit = 0x01;

        private const byte DomesticCashbackBit = 0x80;
        private const byte InternationalCashbackBit = 0x40;

        #endregion

        #region >> Properties

        /// <summary>
        /// AUC: Valid for domestic cash transactions.
        /// </summary>
        public Boolean DomesticCash
        {
            get { return (Tlv.Value[0] & DomesticCashBit) == DomesticCashBit; }
        }

        /// <summary>
        /// AUC: Valid for international cash transactions.
        /// </summary>
        public Boolean InternationalCash
        {
            get { return (Tlv.Value[0] & InternationalCashBit) == InternationalCashBit; }
        }

        /// <summary>
        /// AUC: Valid for domestic goods.
        /// </summary>
        public Boolean DomesticGoods
        {
            get { return (Tlv.Value[0] & DomesticGoodsBit) == DomesticGoodsBit; }
        }

        /// <summary>
        /// AUC: Valid for international goods.
        /// </summary>
        public Boolean InternationalGoods
        {
            get { return (Tlv.Value[0] & InternationalGoodsBit) == InternationalGoodsBit; }
        }

        /// <summary>
        /// AUC: Valid for domestic services.
        /// </summary>
        public Boolean DomesticServices
        {
            get { return (Tlv.Value[0] & DomesticServicesBit) == DomesticServicesBit; }
        }

        /// <summary>
        /// AUC: Valid for international services.
        /// </summary>
        public Boolean InternationalServices
        {
            get { return (Tlv.Value[0] & InternationalServicesBit) == InternationalServicesBit; }
        }

        /// <summary>
        /// AUC: Valid at ATMs.
        /// </summary>
        public Boolean Atm
        {
            get { return (Tlv.Value[0] & AtmBit) == AtmBit; }
        }

        /// <summary>
        /// AUC: Valid at terminals other than ATMs.
        /// </summary>
        public Boolean OtherThanAtm
        {
            get { return (Tlv.Value[0] & OtherThanAtmBit) == OtherThanAtmBit; }
        }

        /// <summary>
        /// AUC: Domestic cashback allowed.
        /// </summary>
        public Boolean DomesticCashback
        {
            get { return (Tlv.Value[1] & DomesticCashbackBit) == DomesticCashbackBit; }
        }

        /// <summary>
        /// AUC: International cashback allowed.
        /// </summary>
        public Boolean InternationalCashBack
        {
            get { return (Tlv.Value[1] & InternationalCashbackBit) == InternationalCashbackBit; }
        }

        #endregion

        #region >> Constructors

        /// <summary>
        /// Initializes a new <see cref="ApplicationUsageControl"/> instance.
        /// </summary>
        public ApplicationUsageControl()
        {
        }

        /// <summary>
        /// Initializes a new <see cref="ApplicationUsageControl"/> instance.
        /// </summary>
        /// <param name="auc">TLV AUC data</param>
        public ApplicationUsageControl(TlvData auc)
        {
            Tlv = auc;
        }

        #endregion

        #region >> Object

        /// <inheritdoc />
        public override string ToString()
        {
            var s = "";
            if (DomesticCash || InternationalCash)
            {
                s += "[cash: " + (DomesticCash ? "domestic " : "") + (InternationalCash ? "international" : "") + "] ";
            }
            if (DomesticGoods || InternationalGoods)
            {
                s += "[goods: " + (DomesticGoods ? "domestic " : "") + (InternationalGoods ? "international" : "") +
                     "] ";
            }
            if (DomesticServices || InternationalServices)
            {
                s += "[services: " + (DomesticServices ? "domestic " : "") +
                     (InternationalServices ? "international" : "") + "] ";
            }
            if (Atm || OtherThanAtm)
            {
                s += "[terminals: " + (Atm ? "ATM " : "") + (OtherThanAtm ? "other" : "") + "] ";
            }
            if (DomesticCashback || InternationalCashBack)
            {
                s += "[cashback: " + (DomesticCashback ? "domestic " : "") +
                     (InternationalCashBack ? "international" : "") + "]";
            }
            return s;
        }

        #endregion
    }
}