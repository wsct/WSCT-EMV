using System;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents the Application File Locator of an EMV application
    /// </summary>
    public class ApplicationFileLocator : BinaryTLVObject
    {
        #region >> Nested classes

        /// <summary>
        /// Represents identification informations for one file in the AFL
        /// </summary>
        public class FileIdentification
        {
            /// <summary>
            /// SFI of the file
            /// </summary>
            public Byte sfi;
            /// <summary>
            /// First record to be read
            /// </summary>
            public Byte firstRecord;
            /// <summary>
            /// Last record to be read
            /// </summary>
            public Byte lastRecord;
            /// <summary>
            /// Last record to be used in offline data authentication (from <c>firstRecord</c> to <c>firstRecord+offlineNumberOfRecords</c>)
            /// </summary>
            public Byte offlineNumberOfRecords;
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="fourBytes">Four bytes identifying the file</param>
            public FileIdentification(Byte[] fourBytes)
                : this(fourBytes, 0)
            {
            }
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="fourBytes">Four bytes identifying the file</param>
            /// <param name="offset">Offset in the array <paramref>fourBytes</paramref></param>
            public FileIdentification(Byte[] fourBytes, Byte offset)
            {
                sfi = (Byte)(fourBytes[offset + 0] >> 3);
                firstRecord = fourBytes[offset + 1];
                lastRecord = fourBytes[offset + 2];
                offlineNumberOfRecords = fourBytes[offset + 3];
            }
            /// <summary>
            /// Represents the object in String format
            /// </summary>
            /// <returns>String representation</returns>
            public override string ToString()
            {
                return String.Format("sfi:{0} from {1} to {2} (used for offline auth:{3})",
                    sfi, firstRecord, lastRecord, offlineNumberOfRecords); ;
            }
        }

        #endregion

        #region >> Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public ApplicationFileLocator()
            : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="aflData">Raw AFL data</param>
        public ApplicationFileLocator(Byte[] aflData)
            : this()
        {
            tlv = new WSCT.Helpers.BasicEncodingRules.TLVData();
            tlv.value = aflData;
        }

        #endregion

        #region >> Methods

        /// <summary>
        /// Enumerates files 
        /// </summary>
        /// <returns><see cref="ApplicationFileLocator.FileIdentification">FileIdentification</see> instances</returns>
        public System.Collections.IEnumerable getFiles()
        {
            Byte offset = 0;
            while (offset < tlv.value.Length)
            {
                yield return new ApplicationFileLocator.FileIdentification(tlv.value, offset);
                offset += 4;
            }
        }

        #endregion

    }
}
