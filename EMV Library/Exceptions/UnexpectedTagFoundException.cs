using System;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Exceptions
{
    public class UnexpectedTagFoundException : Exception
    {
        public uint TagID { get; }

        public UnexpectedTagFoundException(uint tagId)
            : base($"Unexpected tag {new TlvData { Tag = tagId }:T} found.")
        {
            TagID = tagId;
        }

        public UnexpectedTagFoundException(uint tagId, string message)
            : base(message)
        {
            TagID = tagId;
        }
    }
}
