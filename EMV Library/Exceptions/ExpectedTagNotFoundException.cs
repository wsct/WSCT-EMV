using System;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Exceptions
{
    public class ExpectedTagNotFoundException : Exception
    {
        public uint TagID { get; }

        public ExpectedTagNotFoundException(uint tagId)
            : base($"Tag {new TlvData { Tag = tagId }:T} not found in available data.")
        {
            TagID = tagId;
        }
    }
}
