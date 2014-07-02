using System;
using System.Collections.Generic;
using System.Linq;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents the Application File Locator of an EMV application.
    /// </summary>
    public class ApplicationFileLocator : BinaryTlvObject
    {
        #region >> Nested classes

        #endregion

        #region >> Constructors

        /// <summary>
        /// Initializes a new <see cref="ApplicationFileLocator"/> instance.
        /// </summary>
        public ApplicationFileLocator()
        {
            Tlv = new TlvData { Tag = 0x94 };
        }

        /// <summary>
        /// Initializes a new <see cref="ApplicationFileLocator"/> instance.
        /// </summary>
        /// <param name="aflData">Raw AFL data.</param>
        public ApplicationFileLocator(byte[] aflData)
            : this()
        {
            Tlv.Length = (uint)aflData.Length;
            Tlv.Value = aflData;
        }

        #endregion

        #region >> Methods

        /// <summary>
        /// AFL entries enumerator.
        /// </summary>
        public IEnumerable<AflEntry> Files
        {
            get
            {
                if (Tlv == null || Tlv.Value == null)
                {
                    yield break;
                }

                byte offset = 0;
                while (offset < Tlv.Value.Length)
                {
                    yield return new AflEntry(Tlv.Value, offset);
                    offset += 4;
                }
            }
            set
            {
                IEnumerable<byte> aflValue = new Byte[0];
                aflValue = value.Aggregate(aflValue, (current, entry) => current.Concat(entry.ToEnumerable()));
                Tlv.Value = aflValue.ToArray();
            }
        }

        #endregion
    }
}