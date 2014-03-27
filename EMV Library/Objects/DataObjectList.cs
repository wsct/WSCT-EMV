using System;
using System.Collections.Generic;
using System.Linq;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents a Data Object List (CDOL, DDOL, PDOL, TDOL) of an EMV application.
    /// </summary>
    public class DataObjectList : BinaryTlvObject
    {
        #region >> Nested Class

        /// <summary>
        /// Represents identification informations for one file in the AFL.
        /// </summary>
        public class DataObjectDefinition : IFormattable
        {
            /// <inheritdoc cref="TlvData.Length"/>
            public UInt32 Length;

            /// <inheritdoc cref="TlvData.Tag"/>
            public UInt32 Tag;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="tag">Tag of the DO.</param>
            /// <param name="length">Length of the DO.</param>
            public DataObjectDefinition(UInt32 tag, UInt32 length)
            {
                Tag = tag;
                Length = length;
            }

            #region IFormattable Membres

            /// <inheritdoc />
            public string ToString(string format, IFormatProvider formatProvider)
            {
                if (!string.IsNullOrEmpty(format))
                {
                    var tlv = new TlvData { Tag = Tag, Length = Length };
                    return tlv.ToString(format, formatProvider);
                }

                return ToString();
            }

            #endregion

            #region >> Object

            /// <inheritdoc />
            public override string ToString()
            {
                var tlv = new TlvData { Tag = Tag, Length = Length };
                return String.Format("({0:T}/{0:L})", tlv);
            }

            #endregion
        }

        #endregion

        #region >> Constructors

        /// <summary>
        /// Initializes a new <see cref="DataObjectList"/> instance.
        /// </summary>
        public DataObjectList()
        {
        }

        /// <summary>
        /// Initializes a new <see cref="DataObjectList"/> instance.
        /// </summary>
        /// <param name="dolData">Raw DOL data.</param>
        public DataObjectList(byte[] dolData)
            : this()
        {
            Tlv = new TlvData { Value = dolData };
        }

        #endregion

        #region >> Methods

        /// <summary>
        /// Enumerates DO.
        /// </summary>
        /// <returns><see cref="DataObjectList.DataObjectDefinition"/> instances.</returns>
        public IEnumerable<DataObjectDefinition> GetDataObjectDefinitions()
        {
            UInt32 offset = 0;
            var tlvParser = new TlvData();
            while (offset < Tlv.Value.Length)
            {
                offset = tlvParser.ParseT(Tlv.Value, offset);
                offset = tlvParser.ParseL(Tlv.Value, offset);
                yield return new DataObjectDefinition(tlvParser.Tag, tlvParser.Length);
            }
        }

        /// <summary>
        /// Parses an array of Bytes as a sequence of value, using the format defined by the <see cref="DataObjectList"/>.
        /// </summary>
        /// <param name="data">Array of Bytes to be parsed.</param>
        /// <returns>The list of <see cref="TlvData"/> objects obtained after parsing.</returns>
        public List<TlvData> ParseRawData(byte[] data)
        {
            var tlvDataList = new List<TlvData>();

            UInt32 offset = 0;
            foreach (var dod in GetDataObjectDefinitions())
            {
                var tlvData = new TlvData { Tag = dod.Tag, Length = dod.Length, Value = new byte[dod.Length] };
                Array.Copy(data, offset, tlvData.Value, 0, dod.Length);
                tlvDataList.Add(tlvData);

                offset += dod.Length;
            }

            return tlvDataList;
        }

        /// <summary>
        /// Builds a raw byte[] as the concatenation of tag values defined by the DOL.
        /// </summary>
        /// <remarks>
        /// If a DOD needs a tag not found in <paramref name="tlvData"/>, an exception is raised.
        /// </remarks>
        /// <param name="tlvData">TLV data available, to be used to search the tag values.</param>
        /// <returns>The byte[] built.</returns>
        public byte[] BuildData(IEnumerable<TlvData> tlvData)
        {
            // Compute total length of final array
            var length = GetDataObjectDefinitions().Aggregate<DataObjectDefinition, uint>(0, (current, dod) => current + dod.Length);
            // Initialize final array
            var data = new byte[length];
            // Build content
            UInt32 offset = 0;
            foreach (var dod in GetDataObjectDefinitions())
            {
                // Search the object
                var tagFound = false;
                foreach (var tlvSubData in tlvData)
                {
                    if (tlvSubData != null && tlvSubData.Tag == dod.Tag)
                    {
                        Array.Copy(tlvSubData.Value, 0, data, offset, dod.Length);
                        tagFound = true;
                    }
                }

                if (!tagFound)
                {
                    throw new Exception(String.Format("DataObjectList.buildData(): tag '{0:T}' not found in available TLVData (DOL: {1}]", dod, this));
                }

                offset += dod.Length;
            }
            return data;
        }

        #endregion

        #region >> Object

        /// <inheritdoc />
        public override string ToString()
        {
            return GetDataObjectDefinitions()
                .Aggregate("", (current, dataObject) => current + String.Format("{0} ", dataObject));
        }

        #endregion
    }
}