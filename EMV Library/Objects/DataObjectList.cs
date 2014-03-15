using System;
using System.Collections.Generic;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents a Data Object List (CDOL, DDOL, PDOL, TDOL) of an EMV application
    /// </summary>
    public class DataObjectList : BinaryTLVObject
    {
        /// <summary>
        /// Represents identification informations for one file in the AFL
        /// </summary>
        public class DataObjectDefinition : IFormattable
        {
            /// <inheritdoc cref="TLVData.tag"/>
            public UInt32 tag;
            /// <inheritdoc cref="TLVData.length"/>
            public UInt32 length;
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="tag">Tag of the DO</param>
            /// <param name="length">Length of the DO</param>
            public DataObjectDefinition(UInt32 tag, UInt32 length)
            {
                this.tag = tag;
                this.length = length;
            }

            #region IFormattable Membres
            /// <inheritdoc />
            public string ToString(string format, IFormatProvider formatProvider)
            {
                if (format != null && format != "")
                {
                    TLVData tlv = new TLVData();
                    tlv.tag = tag;
                    tlv.length = length;
                    return tlv.ToString(format, formatProvider);
                }
                else
                    return ToString();
            }
            #endregion

            /// <inheritdoc />
            public override string ToString()
            {
                TLVData tlv = new TLVData();
                tlv.tag = tag;
                tlv.length = length;
                return String.Format("({0:T}/{0:L})", tlv);
            }
        }

        #region >> Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public DataObjectList()
            : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dolData">Raw DOL data</param>
        public DataObjectList(Byte[] dolData)
            : this()
        {
            tlv = new TLVData();
            tlv.value = dolData;
        }

        #endregion

        #region >> Methods

        /// <summary>
        /// Enumerates DO 
        /// </summary>
        /// <returns><see cref="DataObjectList.DataObjectDefinition"/> instances</returns>
        public System.Collections.IEnumerable getDataObjectDefinitions()
        {
            UInt32 offset = 0;
            TLVData tlvParser = new TLVData();
            while (offset < tlv.value.Length)
            {
                offset = tlvParser.parseT(tlv.value, offset);
                offset = tlvParser.parseL(tlv.value, offset);
                yield return new DataObjectDefinition(tlvParser.tag, tlvParser.length);
            }
        }

        /// <summary>
        /// Parses an array of Bytes as a sequence of value, using the format defined by the <see cref="DataObjectList"/>
        /// </summary>
        /// <param name="data">Array of Bytes to be parsed</param>
        /// <returns>The list of <see cref="TLVData"/> objects obtained after parsing.</returns>
        public List<TLVData> parseRawData(Byte[] data)
        {
            List<TLVData> tlvDataList = new List<TLVData>();

            UInt32 offset = 0;
            foreach (DataObjectDefinition dod in getDataObjectDefinitions())
            {
                TLVData tlvData = new TLVData();
                tlvData.tag = dod.tag;
                tlvData.length = dod.length;
                tlvData.value = new Byte[dod.length];
                Array.Copy(data, offset, tlvData.value, 0, dod.length);
                tlvDataList.Add(tlvData);

                offset += dod.length;
            }

            return tlvDataList;
        }

        /// <summary>
        /// Builds a raw Byte[] as the concatenation of tag values defined by the DOL
        /// </summary>
        /// <remarks>
        /// If a DOD needs a tag not found in <paramref name="tlvData"/>, an exception is raised.
        /// </remarks>
        /// <param name="tlvData">TLV data available, to be used to search the tag values</param>
        /// <returns>The Byte[] built</returns>
        public Byte[] buildData(List<TLVData> tlvData)
        {
            UInt32 length = 0;
            // Compute total length of final array
            foreach (DataObjectDefinition dod in getDataObjectDefinitions())
                length += dod.length;
            // Initialize final array
            Byte[] data = new Byte[length];
            // Build content
            UInt32 offset = 0;
            foreach (DataObjectDefinition dod in getDataObjectDefinitions())
            {
                // Search the object
                Boolean tagFound = false;
                foreach (TLVData tlv in tlvData)
                {
                    if (tlv != null && tlv.tag == dod.tag)
                    {
                        Array.Copy(tlv.value, 0, data, offset, dod.length);
                        tagFound = true;
                    }
                }
                if (!tagFound)
                    throw new Exception(String.Format("DataObjectList.buildData(): tag '{0:T}' not found in available TLVData (DOL: {1}]", dod, this));
                offset += dod.length;
            }
            return data;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            String s = "";
            foreach (DataObjectDefinition dataObject in getDataObjectDefinitions())
                s += String.Format("{0} ", dataObject);
            return s;
        }

        #endregion

    }
}
