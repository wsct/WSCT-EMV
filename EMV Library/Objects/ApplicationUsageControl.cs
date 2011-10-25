using System;
using System.Collections.Generic;
using System.Text;

using WSCT.EMV.Tags;
using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents the Application Usage Control of an EMV application
    /// </summary>
    public class ApplicationUsageControl : BinaryTLVObject
    {

        #region >> Internal constants

        const byte DOMESTIC_CASH = 0x80;
        const byte INTERNATIONAL_CASH = 0x40;
        const byte DOMESTIC_GOODS = 0x20;
        const byte INTERNATIONAL_GOODS = 0x10;
        const byte DOMESTIC_SERVICES = 0x08;
        const byte INTERNATIONAL_SERVICES = 0x04;
        const byte ATM = 0x02;
        const byte OTHERTHANATM = 0x01;

        const byte DOMESTIC_CASHBACK = 0x80;
        const byte INTERNATIONAL_CASHBACK = 0x40;

        #endregion

        #region >> Properties

        /// <summary>
        /// AUC: Valid for domestic cash transactions.
        /// </summary>
        public Boolean domesticCash
        {
            get { return (tlv.value[0] & DOMESTIC_CASH) == DOMESTIC_CASH; }
        }

        /// <summary>
        /// AUC: Valid for international cash transactions.
        /// </summary>
        public Boolean internationalCash
        {
            get { return (tlv.value[0] & INTERNATIONAL_GOODS) == INTERNATIONAL_GOODS; }
        }

        /// <summary>
        /// AUC: Valid for domestic goods.
        /// </summary>
        public Boolean domesticGoods
        {
            get { return (tlv.value[0] & DOMESTIC_GOODS) == DOMESTIC_GOODS; }
        }

        /// <summary>
        /// AUC: Valid for international goods
        /// </summary>
        public Boolean internationalGoods
        {
            get { return (tlv.value[0] & INTERNATIONAL_GOODS) == INTERNATIONAL_GOODS; }
        }

        /// <summary>
        /// AUC: Valid for domestic services
        /// </summary>
        public Boolean domesticServices
        {
            get { return (tlv.value[0] & DOMESTIC_SERVICES) == DOMESTIC_SERVICES; }
        }

        /// <summary>
        /// AUC: Valid for international services
        /// </summary>
        public Boolean internationalServices
        {
            get { return (tlv.value[0] & INTERNATIONAL_SERVICES) == INTERNATIONAL_SERVICES; }
        }

        /// <summary>
        /// AUC: Valid at ATMs
        /// </summary>
        public Boolean atm
        {
            get { return (tlv.value[0] & ATM) == ATM; }
        }

        /// <summary>
        /// AUC: Valid at terminals other than ATMs
        /// </summary>
        public Boolean otherThanATM
        {
            get { return (tlv.value[0] & OTHERTHANATM) == OTHERTHANATM; }
        }

        /// <summary>
        /// AUC: Domestic cashback allowed
        /// </summary>
        public Boolean domesticCashback
        {
            get { return (tlv.value[1] & DOMESTIC_CASHBACK) == DOMESTIC_CASHBACK; }
        }

        /// <summary>
        /// AUC: International cashback allowed
        /// </summary>
        public Boolean internationalCashBack
        {
            get { return (tlv.value[1] & INTERNATIONAL_CASHBACK) == INTERNATIONAL_CASHBACK; }
        }

        #endregion

        #region >> Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public ApplicationUsageControl()
            : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="auc">TLV AUC data</param>
        public ApplicationUsageControl(TLVData auc)
            : base()
        {
            tlv = auc;
        }

        #endregion

        /// <inheritdoc />
        public override string ToString()
        {
            String s = "";
            if (domesticCash || internationalCash)
                s += "[cash: " + (domesticCash ? "domestic " : "") + (internationalCash ? "international" : "") + "] ";
            if (domesticGoods || internationalGoods)
                s += "[goods: " + (domesticGoods ? "domestic " : "") + (internationalGoods ? "international" : "") + "] ";
            if (domesticServices || internationalServices)
                s += "[services: " + (domesticServices ? "domestic " : "") + (internationalServices ? "international" : "") + "] ";
            if (atm || otherThanATM)
                s += "[terminals: " + (atm ? "ATM " : "") + (otherThanATM ? "other" : "") + "] ";
            if (domesticCashback || internationalCashBack)
                s += "[cashback: " + (domesticCashback ? "domestic " : "") + (internationalCashBack ? "international" : "") + "]";
            return s;
        }

    }
}
