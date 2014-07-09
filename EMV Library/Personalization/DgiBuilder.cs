using System;
using System.Collections.Generic;
using System.Linq;
using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Personalization
{
    /// <summary>
    /// Builder of EMV DGI based on personalization models and data.
    /// </summary>
    public class DgiBuilder
    {
        private readonly EmvPersonalizationData data;

        #region >> Constructors

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="data"></param>
        public DgiBuilder(EmvPersonalizationData data)
        {
            this.data = data;
        }

        #endregion

        /// <summary>
        /// Builds UDR to be used with PUT DATA command for given record .
        /// </summary>
        /// <param name="recordModel"></param>
        /// <returns></returns>
        public string GetCommand(RecordModel recordModel)
        {
            var fields = new List<TagModel>();

            if (recordModel.Fields != null)
            {
                fields.AddRange(recordModel.Fields.Select(t => new TagModel { Tag = t }));
            }

            var tagModel = new TagModel { Tag = "70", Fields = fields };
            var command = GetCommand(tagModel);

            return String.Format("{0:X2}{1:X2}{2}{3}", recordModel.Sfi, recordModel.Index, TlvDataHelper.ToBerEncodedL((uint)command.Length / 2).ToHexa('\0'), command);
        }

        /// <summary>
        /// Builds UDR to be used with PUT DATA command for given FCI .
        /// </summary>
        /// <param name="fciModel"></param>
        /// <returns></returns>
        public string GetCommand(FciModel fciModel)
        {
            var tagModel = new TagModel { Tag = "6F", Fields = fciModel.Tags };
            var command = GetCommand(tagModel);

            return String.Format("{0}{1}{2}", fciModel.Dgi, TlvDataHelper.ToBerEncodedL((uint)command.Length / 2).ToHexa('\0'), command);
        }

        /// <summary>
        /// Builds UDR to be used with PUT DATA command for given tag.
        /// </summary>
        /// <param name="tagModel"></param>
        /// <returns></returns>
        public string GetCommand(TagModel tagModel)
        {
            return GetCommandR(tagModel)
                .ToByteArray()
                .ToHexa('\0');
        }

        private TlvData GetCommandR(TagModel tagModel)
        {
            var tlv = new TlvData { Tag = Convert.ToUInt32(tagModel.Tag, 16) };

            if (tagModel.Fields == null)
            {
                tlv.Value = data.UnmanagedAttributes[tagModel.Tag].ToObject<string>().FromHexa();
            }
            else
            {
                tlv.InnerTlvs = tagModel.Fields.Select(GetCommandR).ToList();
            }

            return tlv;
        }
    }
}
